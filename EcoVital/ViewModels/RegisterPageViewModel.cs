using System.Windows.Input;
using EcoVital.Services;
using EcoVital.UserControl;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Newtonsoft.Json;

namespace EcoVital.ViewModels;

/// <summary>
/// ViewModel para gestionar el registro de nuevos usuarios.
/// </summary>
public partial class RegisterPageViewModel : BaseViewModel
{
    readonly ILoadingService _loadingService = new LoadingService();
    readonly ILoginRepository _loginRepository = new LoginService();

    [ObservableProperty] string _confirmPassword;
    [ObservableProperty] string _email;
    [ObservableProperty] string _password;
    [ObservableProperty] string _securityAnswer;
    [ObservableProperty] string _securityQuestion;
    [ObservableProperty] string _userName;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="RegisterPageViewModel"/>.
    /// </summary>
    public RegisterPageViewModel()
    {
        GoBackCommand = new RelayCommand(Cancel);
    }

    /// <summary>
    /// Comando para volver a la página de inicio de sesión.
    /// </summary>
    public ICommand GoBackCommand { get; set; }

    /// <summary>
    /// Cancela la operación y navega a la página de inicio de sesión.
    /// </summary>
    void Cancel()
    {
        Shell.Current.GoToAsync("LoginPage");
    }

    /// <summary>
    /// Comando para registrar un nuevo usuario.
    /// </summary>
    [ICommand]
    public async Task Register()
    {
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await Application.Current.MainPage.DisplayAlert("Error",
                "Se requiere conexión a Internet para registrarse.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Todos los campos son obligatorios.", "OK");
            await _loadingService.HideLoading();
            return;
        }

        if (string.IsNullOrEmpty(Email) || !Email.Contains("@"))
        {
            await Application.Current.MainPage.DisplayAlert("Error",
                "Por favor, proporciona un correo electrónico válido.", "OK");
            return;
        }

        if (!IsPasswordSecure(Password))
        {
            await Application.Current.MainPage.DisplayAlert("Contraseña insegura",
                "Tu contraseña debe tener al menos 6 caracteres, incluir al menos un carácter en mayúsculas y un símbolo.",
                "OK");
            await _loadingService.HideLoading();
            return;
        }

        if (Password != ConfirmPassword)
        {
            await Application.Current.MainPage.DisplayAlert("Error",
                "La contraseña y la confirmación de contraseña no coinciden.", "OK");
            await _loadingService.HideLoading();
            return;
        }

        var userExists = await _loginRepository.UserExists(UserName.ToLower()) ||
                         await _loginRepository.UserExists(Email.ToLower());

        if (userExists)
        {
            await Application.Current.MainPage.DisplayAlert("Error",
                "Un usuario con el mismo nombre de usuario y/o correo electrónico ya existe.", "OK");
            return;
        }

        var userInfo = await _loginRepository.Register(Email, UserName, Password);

        if (userInfo != null)
        {
            if (Preferences.ContainsKey(nameof(App.UserInfo))) Preferences.Remove(nameof(App.UserInfo));

            var userDetails = JsonConvert.SerializeObject(userInfo);
            Preferences.Set(nameof(App.UserInfo), userDetails);
            App.UserInfo = userInfo;

            Shell.Current.FlyoutHeader = new FlyoutHeaderControl();

            await _loadingService.ShowLoading();

            await Application.Current.MainPage.DisplayAlert("Registro exitoso",
                "El usuario se ha registrado correctamente.", "OK");

            await _loadingService.HideLoading();

            await Shell.Current.GoToAsync("SecurityQuestionPage");

            UserName = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
            SecurityQuestion = string.Empty;
            SecurityAnswer = string.Empty;
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Registro fallido",
                "No se pudo completar el registro. Por favor, inténtalo de nuevo.", "OK");
        }
    }

    /// <summary>
    /// Verifica si la contraseña es segura.
    /// </summary>
    /// <param name="password">La contraseña a verificar.</param>
    /// <returns><c>true</c> si la contraseña es segura; de lo contrario, <c>false</c>.</returns>
    public static bool IsPasswordSecure(string password)
    {
        if (password.Length < 6) return false;
        if (!password.Any(char.IsUpper)) return false;
        return password.Any(char.IsSymbol) || password.Any(char.IsPunctuation);
    }
}