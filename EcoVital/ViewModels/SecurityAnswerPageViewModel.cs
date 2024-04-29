using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using EcoVital.Services;
using EcoVital.Views;

namespace EcoVital.ViewModels;

public partial class SecurityAnswerPageViewModel : BaseViewModel
{
    public ICommand CheckAnswerCommand { get; set; }
    string _question;

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

    public string Answer { get; set; }

    private readonly ILoginRepository _loginRepository;
    private int _userId;

    public SecurityAnswerPageViewModel(int userId, string securityQuestion)
    {
        _userId = userId;
        Question = securityQuestion;
        _loginRepository = new LoginService();
        CheckAnswerCommand = new RelayCommand(CheckAnswer);


        InitializeUserInfo();
    }

    private async void InitializeUserInfo()
    {
        App.UserInfo = await _loginRepository.GetUserByEmail(App.UserEmail);
    }

    private async void CheckAnswer()
    {
        if (string.IsNullOrWhiteSpace(Answer))
        {
            await App.Current.MainPage.DisplayAlert("Error",
                "Por favor, proporciona una respuesta a la pregunta de seguridad.", "OK");

            return;
        }

        var securityQuestion = await _loginRepository.GetSecurityQuestionByUserId(_userId);


        int failedAttempts = Preferences.Get($"{App.UserEmail}_FailedPasswordRecoveryAttempts", 0);
        DateTime lastFailedAttempt =
            Preferences.Get($"{App.UserEmail}_LastFailedPasswordRecoveryAttempt", DateTime.MinValue);


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


        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(Answer));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

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