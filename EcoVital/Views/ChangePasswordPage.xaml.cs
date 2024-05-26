using EcoVital.Services;
using EcoVital.ViewModels;

namespace EcoVital.Views;

/// <summary>
/// Página para cambiar la contraseña.
/// </summary>
public partial class ChangePasswordPage : ContentPage
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ChangePasswordPage"/>.
    /// </summary>
    public ChangePasswordPage()
    {
        InitializeComponent();
        BindingContext = new ChangePasswordViewModel(new LoginService());
    }
}