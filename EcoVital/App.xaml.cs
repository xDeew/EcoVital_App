using System.Diagnostics;
using EcoVital.Models;
using EcoVital.UserControl;
using EcoVital.ViewModels;
using EcoVital.Views;
using Newtonsoft.Json;

namespace EcoVital;

public partial class App : Application
{
    public static UserInfo UserInfo;
    public static string UserEmail;
    public static HomePageViewModel HomePageViewModel { get; set; } = new HomePageViewModel();

    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
        CheckInitialLoginState();
    }
    
    protected override async void OnStart()
    {
        base.OnStart();
        await Shell.Current.GoToAsync("LoginPage");
        
    }

    private async void CheckInitialLoginState()
    {
        var isRememberMeChecked = Preferences.Get("IsRememberMeChecked", false);
        var userDetails = Preferences.Get(nameof(UserInfo), string.Empty);

        if (isRememberMeChecked && !string.IsNullOrWhiteSpace(userDetails))
        {
            try
            {
                UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(userDetails);
                if (userInfo != null)
                {
                    App.UserInfo = userInfo;
                    Shell.Current.FlyoutHeader = new FlyoutHeaderControl();
                    Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
                
                    // Actualiza el nombre de usuario en HomePageViewModel
                    App.HomePageViewModel.UserName = userInfo.UserName;

                    await Shell.Current.GoToAsync($"//{nameof(HomePage)}");

                    return;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deserializando UserInfo: {ex}");
                // Manejar el error adecuadamente
            }
        }

        // Dirige al usuario a la LoginPage si "Recordarme" no está marcado o si no hay información de usuario
        await Shell.Current.GoToAsync("LoginPage");
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
    }
}