using System.Windows.Input;
using EcoVital.Services;
using EcoVital.Views;
using Microsoft.Toolkit.Mvvm.Input;

namespace EcoVital.ViewModels;

public class ForgotPasswordPageViewModel : BaseViewModel
{
    readonly ILoginRepository _loginRepository;

    public ForgotPasswordPageViewModel()
    {
        _loginRepository = new LoginService();
        GoBackCommand = new RelayCommand(Cancel);
        SendCommand = new RelayCommand(Send);
    }

    public ForgotPasswordPageViewModel(ILoginRepository loginRepository)
    {
        _loginRepository = loginRepository;
    }

    public ICommand SendCommand { get; set; }

    public ICommand GoBackCommand { get; set; }
    public string Email { get; set; }

    void Cancel()
    {
        Shell.Current.GoToAsync("LoginPage");
    }

    public async void Send()
    {
        // Verificamos si el correo electrónico no está vacío
        if (string.IsNullOrWhiteSpace(Email))
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Por favor, proporciona un correo electrónico.",
                "OK");

            return;
        }

        // Luego verificamos si el correo electrónico es válido
        //var emailRegex = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$";
        //if (!Regex.IsMatch(Email, emailRegex))
        //{
        //    await App.Current.MainPage.DisplayAlert("Error", "Por favor, proporciona un correo electrónico válido.", "OK");
        //      return;
        //   }


        try
        {
            var user = await _loginRepository.GetUserByEmail(Email);

            if (user == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error",
                    "No existe ningún correo registrado con el proporcionado.", "OK");

                return;
            }

            App.UserEmail = Email;

            var securityQuestion = await _loginRepository.GetSecurityQuestionByUserId(user.UserId);

            if (securityQuestion != null)
            {
                var securityAnswerPage = new SecurityAnswerPage(user.UserId, securityQuestion);


                await Shell.Current.Navigation.PushAsync(securityAnswerPage);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se encontró la pregunta de seguridad.",
                    "OK");
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Un error ocurrió: {ex.Message}", "OK");
        }
    }
}