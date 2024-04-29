using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;
using EcoVital.Models;
using EcoVital.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace EcoVital.ViewModels
{
    public partial class SecurityQuestionPageViewModel : BaseViewModel
    {
        [ObservableProperty] string _selectedSecurityQuestion;
        [ObservableProperty] string _securityAnswer;
        [ObservableProperty] List<string> _securityQuestions;

        string _hashedSecurityAnswer;

        readonly ILoginRepository _loginRepository;
        public ICommand ContinueCommand { get; set; }

        public SecurityQuestionPageViewModel()
        {
            _loginRepository = new LoginService();
            ContinueCommand = new RelayCommand(Continue);
            _securityQuestions = new List<string>
            {
                "¿Cuál es tu color favorito?",
                "¿Cuál es el nombre de tu primera mascota?",
                "¿Cuál es tu ciudad natal?",
                "¿Cuál es tu comida favorita?",
            };
        }


        public async void Continue()
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(SecurityAnswer));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                _hashedSecurityAnswer = builder.ToString();
            }

            var securityQuestion = new SecurityQuestion
            {
                SecurityQuestionId = 0,
                QuestionText = SelectedSecurityQuestion,
                Answer = _hashedSecurityAnswer,
                UserId = App.UserInfo.UserId
            };

            var result = await _loginRepository.SendSecurityQuestion(securityQuestion);

            if (result.IsSuccessStatusCode)
            {
                await App.Current.MainPage.DisplayAlert("Éxito",
                    $"Pregunta guardada correctamente para el usuario {App.UserInfo.UserName}.", "OK");


                SelectedSecurityQuestion = null;
                SecurityAnswer = null;
                await Shell.Current.GoToAsync("///LoginPage");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error",
                    $"No se pudo guardar la pregunta de seguridad. Error: {result.ReasonPhrase}", "OK");
            }
        }
    }
}