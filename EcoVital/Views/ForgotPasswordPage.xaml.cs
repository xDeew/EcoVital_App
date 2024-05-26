using EcoVital.ViewModels;

namespace EcoVital.Views;

/// <summary>
/// Página para recuperar la contraseña.
/// </summary>
public partial class ForgotPasswordPage : ContentPage
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ForgotPasswordPage"/>.
    /// </summary>
    public ForgotPasswordPage()
    {
        InitializeComponent();
        BindingContext = new ForgotPasswordPageViewModel();
    }
}