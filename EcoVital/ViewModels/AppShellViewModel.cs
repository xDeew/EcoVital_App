using System.Diagnostics;
using EcoVital.Services;
using Microsoft.Toolkit.Mvvm.Input;

namespace EcoVital.ViewModels
{
    public partial class AppShellViewModel : BaseViewModel
    {
        [ICommand]
        async void SignOut()
        {
            await LoadingService.ShowLoading();


            if (Preferences.ContainsKey(nameof(App.UserInfo)))
            {
                Preferences.Remove(nameof(App.UserInfo));
            }


            if (Preferences.ContainsKey("IsRememberMeChecked"))
            {
                Preferences.Remove("IsRememberMeChecked");
            }


            Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;

            await LoadingService.HideLoading();


            await Shell.Current.GoToAsync("///LoginPage");


            Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;

            Preferences.Set("IsRememberMeChecked", false);
        }
    }
}