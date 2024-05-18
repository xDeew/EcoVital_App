using Microsoft.Toolkit.Mvvm.Input;

namespace EcoVital.ViewModels;

public partial class AppShellViewModel : BaseViewModel
{
    [ICommand]
    async void SignOut()
    {
        if (Preferences.ContainsKey(nameof(App.UserInfo))) Preferences.Remove(nameof(App.UserInfo));


        if (Preferences.ContainsKey("IsRememberMeChecked")) Preferences.Remove("IsRememberMeChecked");


        Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;


        await Shell.Current.Navigation.PopToRootAsync();

        await Shell.Current.GoToAsync("LoginPage", true);


        Shell.Current.BackgroundColor = Color.FromHex("#76C893");

        Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;

        Preferences.Set("IsRememberMeChecked", false);
    }
}