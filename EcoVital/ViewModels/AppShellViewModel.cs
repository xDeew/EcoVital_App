using Microsoft.Toolkit.Mvvm.Input;

namespace EcoVital.ViewModels;

/// <summary>
/// ViewModel para la shell de la aplicación que maneja la lógica de cierre de sesión.
/// </summary>
public partial class AppShellViewModel : BaseViewModel
{
    /// <summary>
    /// Comando para cerrar sesión.
    /// </summary>
    [ICommand]
    async void SignOut()
    {
        // Elimina la información del usuario almacenada en las preferencias
        if (Preferences.ContainsKey(nameof(App.UserInfo))) Preferences.Remove(nameof(App.UserInfo));

        // Elimina la preferencia de "Recordar usuario"
        if (Preferences.ContainsKey("IsRememberMeChecked")) Preferences.Remove("IsRememberMeChecked");

        // Deshabilita el comportamiento del Flyout
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;

        // Navega a la raíz de la navegación y redirige a la página de inicio de sesión
        await Shell.Current.Navigation.PopToRootAsync();
        await Shell.Current.GoToAsync("LoginPage", true);

        // Establece el color de fondo de la shell
        Shell.Current.BackgroundColor = Color.FromHex("#76C893");

        // Deshabilita el comportamiento del Flyout nuevamente
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;

        // Establece la preferencia de "Recordar usuario" como falsa
        Preferences.Set("IsRememberMeChecked", false);
    }
}