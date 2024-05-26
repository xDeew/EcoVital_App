using System.Windows.Input;
using EcoVital.Services;
using EcoVital.Views;
using Microsoft.Toolkit.Mvvm.Input;

namespace EcoVital.ViewModels;

/// <summary>
/// ViewModel para gestionar la lógica de la página de "Olvidé mi contraseña".
/// </summary>
public class ForgotPasswordPageViewModel : BaseViewModel
{
    readonly ILoginRepository _loginRepository;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ForgotPasswordPageViewModel"/> con el repositorio de inicio de sesión predeterminado.
    /// </summary>
    public ForgotPasswordPageViewModel()
    {
        _loginRepository = new LoginService();
        GoBackCommand = new RelayCommand(Cancel);
        SendCommand = new RelayCommand(Send);
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ForgotPasswordPageViewModel"/> con un repositorio de inicio de sesión proporcionado.
    /// </summary>
    /// <param name="loginRepository">El repositorio de inicio de sesión.</param>
    public ForgotPasswordPageViewModel(ILoginRepository loginRepository)
    {
        _loginRepository = loginRepository;
    }

    /// <summary>
    /// Comando para enviar el correo electrónico de restablecimiento de contraseña.
    /// </summary>
    public ICommand SendCommand { get; set; }

    /// <summary>
    /// Comando para regresar a la página de inicio de sesión.
    /// </summary>
    public ICommand GoBackCommand { get; set; }

    /// <summary>
    /// Obtiene o establece el correo electrónico del usuario.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Cancela la operación y navega a la página de inicio de sesión.
    /// </summary>
    void Cancel()
    {
        Shell.Current.GoToAsync("LoginPage");
    }

    /// <summary>
    /// Envía el correo electrónico de restablecimiento de contraseña.
    /// </summary>
    public async void Send()
    {
        // Verificamos si el correo electrónico no está vacío
        if (string.IsNullOrWhiteSpace(Email))
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Por favor, proporciona un correo electrónico.", "OK");
            return;
        }

        // Luego verificamos si el correo electrónico es válido
        //var emailRegex = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$";
        //if (!Regex.IsMatch(Email, emailRegex))
        //{
        //    await App.Current.MainPage.DisplayAlert("Error", "Por favor, proporciona un correo electrónico válido.", "OK");
        //    return;
        //}

        try
        {
            var user = await _loginRepository.GetUserByEmail(Email);

            if (user == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No existe ningún correo registrado con el proporcionado.", "OK");
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
                await Application.Current.MainPage.DisplayAlert("Error", "No se encontró la pregunta de seguridad.", "OK");
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Un error ocurrió: {ex.Message}", "OK");
        }
    }
}