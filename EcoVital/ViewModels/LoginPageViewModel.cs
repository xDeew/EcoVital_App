using System.Diagnostics;
using System.Windows.Input;
using EcoVital.Models;
using EcoVital.Services;
using EcoVital.UserControl;
using EcoVital.Views;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Newtonsoft.Json;
using Microsoft.Maui.Storage;

namespace EcoVital.ViewModels;

public partial class LoginPageViewModel : BaseViewModel
{
    // Esto es un atributo que se utiliza para generar propiedades observables que notifican a los enlaces de datos cuando cambian
    [ObservableProperty] 
    private string _usernameOrEmail;

    [ObservableProperty] private string _password;

    public bool IsRememberMeChecked { get; set; }


    public ICommand ForgotPasswordCommand { get; set; }

    readonly ILoginRepository _loginRepository = new LoginService();

    public LoginPageViewModel()
    {
        ForgotPasswordCommand = new RelayCommand(NavigateToForgotPasswordPage);
    }

    private async void NavigateToForgotPasswordPage()
    {
        // Limpiar campos de login su hubiera
        UsernameOrEmail = string.Empty;
        Password = string.Empty;
        await Shell.Current.GoToAsync("ForgotPasswordPage");
    }
    
    [ICommand]
    public async void Login()
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
                // La contraseña es incorrecta o el usuario no existe
                await Application.Current.MainPage.DisplayAlert("Inicio de sesión fallido",
                    "La contraseña es incorrecta. Por favor, inténtalo de nuevo.", "OK");

                await LoadingService.HideLoading();

                return;
            }

            // Usuario autenticado correctamente
            if (Preferences.ContainsKey(nameof(App.UserInfo)))
            {
                Preferences.Remove(nameof(App.UserInfo));
            }

            string userDetails = JsonConvert.SerializeObject(userInfo);
            Preferences.Set(nameof(App.UserInfo), userDetails);
            App.UserInfo = userInfo;

            // Actualiza el nombre de usuario en HomePageViewModel
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