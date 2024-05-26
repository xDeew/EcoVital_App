using System.Windows.Input;
using EcoVital.Services;
using EcoVital.UserControl;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Newtonsoft.Json;

namespace EcoVital.ViewModels;

/// <summary>
/// ViewModel para gestionar el inicio de sesión y registro.
/// </summary>
public partial class LoginRegister : BaseViewModel
{
    readonly ILoadingService _loadingService;
    readonly ILoginRepository _loginRepository;

    [ObservableProperty] string _password;
    [ObservableProperty] string _usernameOrEmail;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="LoginRegister"/>.
    /// </summary>
    /// <param name="loginRepository">El repositorio de inicio de sesión.</param>
    /// <param name="loadingService">El servicio de carga.</param>
    public LoginRegister(ILoginRepository loginRepository, ILoadingService loadingService)
    {
        _loginRepository = loginRepository ?? throw new ArgumentNullException(nameof(loginRepository));
        _loadingService = loadingService ?? throw new ArgumentNullException(nameof(loadingService));
        ForgotPasswordCommand = new RelayCommand(NavigateToForgotPasswordPage);
    }

    /// <summary>
    /// Indica si la carga está habilitada.
    /// </summary>
    public bool IsLoadingEnabled { get; set; } = true;

    /// <summary>
    /// Indica si el inicio de sesión fue exitoso.
    /// </summary>
    public bool IsLoginSuccessful { get; private set; }

    /// <summary>
    /// Indica si la opción de "Recordar usuario" está seleccionada.
    /// </summary>
    public bool IsRememberMeChecked { get; set; }

    /// <summary>
    /// Comando para navegar a la página de "Olvidé mi contraseña".
    /// </summary>
    public ICommand ForgotPasswordCommand { get; set; }

    /// <summary>
    /// Navega a la página de "Olvidé mi contraseña".
    /// </summary>
    async void NavigateToForgotPasswordPage()
    {
        UsernameOrEmail = string.Empty;
        Password = string.Empty;
        await Shell.Current.GoToAsync("ForgotPasswordPage");
    }

    /// <summary>
    /// Comando para iniciar sesión.
    /// </summary>
    [ICommand]
    async Task Login()
    {
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await Application.Current.MainPage.DisplayAlert("Error",
                "Se requiere conexión a Internet para iniciar sesión.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(UsernameOrEmail) || string.IsNullOrWhiteSpace(Password))
        {
            await Application.Current.MainPage.DisplayAlert("Error",
                "Por favor, ingresa tanto el usuario como la contraseña.", "OK");
            return;
        }

        UsernameOrEmail = UsernameOrEmail.Trim();
        Password = Password.Trim();

        if (IsLoadingEnabled) await _loadingService.ShowLoading();

        var userExists = await _loginRepository.UserExists(UsernameOrEmail);
        if (!userExists)
        {
            await Application.Current.MainPage.DisplayAlert("Inicio de sesión fallido",
                "El usuario " + UsernameOrEmail + " no se ha encontrado.", "OK");
            await _loadingService.HideLoading();
            IsLoginSuccessful = false;
            return;
        }

        var userInfo = await _loginRepository.Login(UsernameOrEmail, Password);
        if (userInfo == null)
        {
            await Application.Current.MainPage.DisplayAlert("Inicio de sesión fallido",
                "La contraseña es incorrecta. Por favor, inténtalo de nuevo.", "OK");
            await _loadingService.HideLoading();
            IsLoginSuccessful = false;
            return;
        }

        if (Preferences.ContainsKey(nameof(App.UserInfo))) Preferences.Remove(nameof(App.UserInfo));

        var userDetails = JsonConvert.SerializeObject(userInfo);
        Preferences.Set(nameof(App.UserInfo), userDetails);
        App.UserInfo = userInfo;
        App.HomePageViewModel.UserName = userInfo.UserName;
        Preferences.Set("IsRememberMeChecked", IsRememberMeChecked);

        Shell.Current.FlyoutHeader = new FlyoutHeaderControl();
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
        App.HomePageViewModel.UserName = userInfo.UserName;

        await Shell.Current.GoToAsync("//HomePage");

        UsernameOrEmail = string.Empty;
        Password = string.Empty;
        IsRememberMeChecked = false;

        if (IsLoadingEnabled) await _loadingService.HideLoading();

        IsLoginSuccessful = App.UserInfo != null;
    }

    /// <summary>
    /// Comando para registrar un nuevo usuario.
    /// </summary>
    [ICommand]
    public async void Register()
    {
        UsernameOrEmail = string.Empty;
        Password = string.Empty;
        await Shell.Current.GoToAsync("RegisterPage");
    }
}