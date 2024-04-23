using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using EcoVital.Services;
using EcoVital.Views;

namespace EcoVital.ViewModels
{
    public partial class SecurityAnswerPageViewModel : BaseViewModel
    {
        public ICommand CheckAnswerCommand { get; set; }
        private string _question;

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
            Question = securityQuestion; // Asigna la pregunta de seguridad a la propiedad Question
            _loginRepository = new LoginService();
            CheckAnswerCommand = new RelayCommand(CheckAnswer);

            // Inicializa la información del usuario
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

            if (App.UserInfo.FailedPasswordRecoveryAttempts >= 3)
            {
                // Verifica si han pasado 30 minutos desde el último intento fallido
                if (DateTime.Now - App.UserInfo.LastFailedPasswordRecoveryAttempt < TimeSpan.FromMinutes(30))
                {
                    // Si no han pasado 30 minutos, rechaza el intento de recuperación de contraseña
                    await Shell.Current.DisplayAlert("Error",
                        "Has excedido el número máximo de intentos de recuperación de contraseña. Por favor, intenta de nuevo en 30 minutos.",
                        "OK");

                    return;
                }
                else
                {
                    // Si han pasado 30 minutos, reinicia el contador de intentos fallidos
                    App.UserInfo.FailedPasswordRecoveryAttempts = 0;
                }
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

            // Hashear la respuesta proporcionada
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

            // Comparar la respuesta hasheada con la almacenada en la base de datos
            if (securityQuestionByQuestion.Answer == Answer)
            {
                App.UserInfo.FailedPasswordRecoveryAttempts = 0;

                // le pasamos el id del usuario a la página de cambio de contraseña porque lo necesitamos para cambiar la contraseña
                var route = new ShellNavigationState($"{nameof(ChangePasswordPage)}?userId={_userId}");
                await Shell.Current.GoToAsync(route);
            }
            else
            {
                // Si la respuesta de seguridad es incorrecta, incrementa el contador de intentos fallidos y registra la hora del intento fallido
                App.UserInfo.FailedPasswordRecoveryAttempts++;
                App.UserInfo.LastFailedPasswordRecoveryAttempt = DateTime.Now;

                // Muestra un mensaje de error al usuario
                await Shell.Current.DisplayAlert("Error",
                    "La respuesta proporcionada no coincide con la respuesta registrada.", "OK");
            }
        }
    }
}