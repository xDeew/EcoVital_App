using EcoVital.Services;
using EcoVital.ViewModels;

namespace EcoVital.Views;

/// <summary>
/// Página de inicio de sesión de la aplicación.
/// </summary>
public partial class LoginPage
{
    readonly ILoadingService _loadingService = new LoadingService();
    readonly ILoginRepository _loginRepository = new LoginService();

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="LoginPage"/>.
    /// </summary>
    public LoginPage()
    {
        InitializeComponent();
        BindingContext = new LoginRegister(_loginRepository, _loadingService);
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
    }

    /// <summary>
    /// Método llamado cuando la página aparece.
    /// </summary>
    /// <param name="sender">El origen del evento.</param>
    /// <param name="eventArgs">Los datos del evento.</param>
    protected void OnAppearing(object? sender, EventArgs eventArgs)
    {
        base.OnAppearing();
        // Desactivar el Flyout
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
    }

    /// <summary>
    /// Método llamado cuando la página aparece.
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Desactivar el Flyout
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
    }

    /// <summary>
    /// Maneja el evento cuando se presiona el botón de retroceso.
    /// </summary>
    /// <returns>True si se cancela la acción de volver atrás; de lo contrario, false.</returns>
    protected override bool OnBackButtonPressed() =>
        // True = Cancela la acción de volver atrás
        // False = Permite la acción normal de volver atrás
        true;
}