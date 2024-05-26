using EcoVital.ViewModels;

namespace EcoVital.Views;

/// <summary>
/// Página de contacto.
/// </summary>
public partial class ContactPage : ContentPage
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ContactPage"/>.
    /// </summary>
    public ContactPage()
    {
        InitializeComponent();
        BindingContext = new ContactPageViewModel();
    }
}