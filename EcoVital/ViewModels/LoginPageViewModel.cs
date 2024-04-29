using System.Diagnostics;
using System.Windows.Input;
using EcoVital.Models;
using EcoVital.Services;
using EcoVital.UserControl;
using EcoVital.Views;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Newtonsoft.Json;

namespace EcoVital.ViewModels;

public partial class LoginPageViewModel : BaseViewModel
{
    [ObservableProperty] private string _usernameOrEmail;

    [ObservableProperty] private string _password;

    public bool IsRememberMeChecked { get; set; }


    public ICommand ForgotPasswordCommand { get; set; }

    readonly ILoginRepository _loginRepository = new LoginService();

    public LoginPageViewModel(ILoginRepository loginRepository)
    {
        _loginRepository = loginRepository;
        if (_loginRepository == null)
        {
            throw new ArgumentNullException(nameof(loginRepository));
        }
    }
    
    public LoginPageViewModel()
    {
        
        ForgotPasswordCommand = new RelayCommand(NavigateToForgotPasswordPage);
    }

    
    private async void NavigateToForgotPasswordPage()
    {
        UsernameOrEmail = string.Empty;
        Password = string.Empty;
        await Shell.Current.GoToAsync("ForgotPasswordPage");
    }

    [ICommand]
    public async Task Login()
    {
        Debug.WriteLine("Entrando al método Login");
        await LoadingService.ShowLoading();
        if (!string.IsNullOrWhiteSpace(UsernameOrEmail) && !string.IsNullOrWhiteSpace(Password))
        {
            bool userExists = await _loginRepository.UserExists(UsernameOrEmail);
            if (!userExists)
            {
                await Application.Current.MainPage.DisplayAlert("Inicio de sesión fallido",
                    "El usuario " + UsernameOrEmail + " no se ha encontrado.", "OK");
                await LoadingService.HideLoading();
                return;
            }

            UserInfo userInfo = await _loginRepository.Login(UsernameOrEmail, Password);
            if (userInfo == null)
            {
                await Application.Current.MainPage.DisplayAlert("Inicio de sesión fallido",
                    "La contraseña es incorrecta. Por favor, inténtalo de nuevo.", "OK");
                await LoadingService.HideLoading();
                return;
            }

            if (Preferences.ContainsKey(nameof(App.UserInfo)))
            {
                Preferences.Remove(nameof(App.UserInfo));
            }

            string userDetails = JsonConvert.SerializeObject(userInfo);
            Preferences.Set(nameof(App.UserInfo), userDetails);
            App.UserInfo = userInfo;
            App.HomePageViewModel.UserName = userInfo.UserName;
            Preferences.Set("IsRememberMeChecked", IsRememberMeChecked);

            Shell.Current.FlyoutHeader = new FlyoutHeaderControl();
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
            App.HomePageViewModel.UserName = userInfo.UserName;

            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
            UsernameOrEmail = string.Empty;
            Password = string.Empty;
            IsRememberMeChecked = false;
            await LoadingService.HideLoading();
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error",
                "Por favor, ingresa tanto el usuario como la contraseña.", "OK");
        }
        await LoadingService.HideLoading();
    }



    [ICommand]
    public async void Register()
    {
        UsernameOrEmail = string.Empty;
        Password = string.Empty;
        await Shell.Current.GoToAsync("RegisterPage");
    }
}