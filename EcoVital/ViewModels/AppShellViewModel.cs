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
            Debug.WriteLine("Método SignOut");

            await LoadingService.ShowLoading();

            // Elimina la información de usuario almacenada y cualquier preferencia relacionada con "Recordarme"
            if (Preferences.ContainsKey(nameof(App.UserInfo)))
            {
                Preferences.Remove(nameof(App.UserInfo));
            }

            // Aquí es donde remueves la preferencia de "Recordarme"
            if (Preferences.ContainsKey("IsRememberMeChecked"))
            {
                Preferences.Remove("IsRememberMeChecked");
            }

            // Deshabilitar el Flyout para evitar la navegación durante el proceso de cierre de sesión
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;

            await LoadingService.HideLoading();

            // Navega al usuario hacia la página de inicio de sesión
            await Shell.Current.GoToAsync("///LoginPage");


            // Mantén el Flyout deshabilitado en la página de inicio de sesión
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;

            // Establecer "Recordarme" en false cuando el usuario se desconecta
            Preferences.Set("IsRememberMeChecked", false);
        }
    }
}