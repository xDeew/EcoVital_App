using System.Windows.Input;
using EcoVital.Services;
using EcoVital.UserControl;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Newtonsoft.Json;

namespace EcoVital.ViewModels;

public partial class LoginRegister : BaseViewModel
{
    readonly ILoadingService _loadingService;

    readonly ILoginRepository _loginRepository = new LoginService();

    [ObservableProperty] string _password;
    [ObservableProperty] string _usernameOrEmail;


    public LoginRegister(ILoginRepository loginRepository, ILoadingService loadingService)
    {
        _loginRepository = loginRepository ?? throw new ArgumentNullException(nameof(loginRepository));
        _loadingService = loadingService ?? throw new ArgumentNullException(nameof(loadingService));
        ForgotPasswordCommand = new RelayCommand(NavigateToForgotPasswordPage);
    }

    public bool IsLoadingEnabled { get; set; } = true;

    public bool IsLoginSuccessful { get; private set; }

    public bool IsRememberMeChecked { get; set; }


    public ICommand ForgotPasswordCommand { get; set; }


    async void NavigateToForgotPasswordPage()
    {
        UsernameOrEmail = string.Empty;
        Password = string.Empty;
        await Shell.Current.GoToAsync("ForgotPasswordPage");
    }

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

        if (!string.IsNullOrWhiteSpace(UsernameOrEmail) && !string.IsNullOrWhiteSpace(Password))
        {
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
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error",
                "Por favor, ingresa tanto el usuario como la contraseña.", "OK");
        }

        if (IsLoadingEnabled) await _loadingService.HideLoading();

        IsLoginSuccessful = false;
    }


    [ICommand]
    public async void Register()
    {
        UsernameOrEmail = string.Empty;
        Password = string.Empty;
        await Shell.Current.GoToAsync("RegisterPage");
    }
}