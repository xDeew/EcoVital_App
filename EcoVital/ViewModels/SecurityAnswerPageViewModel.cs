using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using EcoVital.Services;
using EcoVital.Views;

namespace EcoVital.ViewModels;

/// <summary>
/// ViewModel para gestionar la verificación de la respuesta a la pregunta de seguridad.
/// </summary>
public class SecurityAnswerPageViewModel : BaseViewModel
{
    readonly ILoginRepository _loginRepository;
    string _question;
    readonly int _userId;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SecurityAnswerPageViewModel"/>.
    /// </summary>
    /// <param name="userId">El identificador del usuario.</param>
    /// <param name="securityQuestion">La pregunta de seguridad.</param>
    public SecurityAnswerPageViewModel(int userId, string securityQuestion)
    {
        _userId = userId;
        Question = securityQuestion;
        _loginRepository = new LoginService();
        CheckAnswerCommand = new RelayCommand(CheckAnswer);

        InitializeUserInfo();
    }

    /// <summary>
    /// Comando para verificar la respuesta a la pregunta de seguridad.
    /// </summary>
    public ICommand CheckAnswerCommand { get; set; }

    /// <summary>
    /// Obtiene o establece la pregunta de seguridad.
    /// </summary>
    public string Question
    {
        get => _question;
        set
        {
            if (_question != value)
            {
                _question = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Obtiene o establece la respuesta a la pregunta de seguridad.
    /// </summary>
    public string Answer { get; set; }

    /// <summary>
    /// Inicializa la información del usuario.
    /// </summary>
    async void InitializeUserInfo()
    {
        App.UserInfo = await _loginRepository.GetUserByEmail(App.UserEmail);
    }

    /// <summary>
    /// Verifica la respuesta a la pregunta de seguridad.
    /// </summary>
    async void CheckAnswer()
    {
        if (string.IsNullOrWhiteSpace(Answer))
        {
            await Application.Current.MainPage.DisplayAlert("Error",
                "Por favor, proporciona una respuesta a la pregunta de seguridad.", "OK");
            return;
        }

        var securityQuestion = await _loginRepository.GetSecurityQuestionByUserId(_userId);

        var failedAttempts = Preferences.Get($"{App.UserEmail}_FailedPasswordRecoveryAttempts", 0);
        var lastFailedAttempt = Preferences.Get($"{App.UserEmail}_LastFailedPasswordRecoveryAttempt", DateTime.MinValue);

        if (failedAttempts >= 3)
        {
            if (DateTime.Now - lastFailedAttempt < TimeSpan.FromMinutes(30))
            {
                await Shell.Current.DisplayAlert("Error",
                    "Has excedido el número máximo de intentos de recuperación de contraseña. Por favor, intenta de nuevo en 30 minutos.",
                    "OK");
                return;
            }

            failedAttempts = 0;
            Preferences.Set($"{App.UserEmail}_FailedPasswordRecoveryAttempts", failedAttempts);
        }

        if (securityQuestion == null)
        {
            await Shell.Current.DisplayAlert("Error", "No se encontró la pregunta de seguridad.", "OK");
            return;
        }

        var securityQuestionByQuestion = await _loginRepository.GetSecurityQuestionByQuestion(Question, _userId);
        if (securityQuestionByQuestion == null)
        {
            await Shell.Current.DisplayAlert("Error",
                "La pregunta proporcionada no coincide con la pregunta registrada.", "OK");
            return;
        }

        using (var sha256Hash = SHA256.Create())
        {
            var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(Answer));
            var builder = new StringBuilder();
            for (var i = 0; i < bytes.Length; i++) builder.Append(bytes[i].ToString("x2"));

            Answer = builder.ToString();
        }

        if (securityQuestionByQuestion.Answer == Answer)
        {
            App.UserInfo.FailedPasswordRecoveryAttempts = 0;

            var route = new ShellNavigationState($"{nameof(ChangePasswordPage)}?userId={_userId}");
            await Shell.Current.GoToAsync(route);
        }
        else
        {
            failedAttempts++;
            Preferences.Set($"{App.UserEmail}_FailedPasswordRecoveryAttempts", failedAttempts);
            Preferences.Set($"{App.UserEmail}_LastFailedPasswordRecoveryAttempt", DateTime.Now);

            await Shell.Current.DisplayAlert("Error",
                "La respuesta proporcionada no coincide con la respuesta registrada.", "OK");
        }
    }
}