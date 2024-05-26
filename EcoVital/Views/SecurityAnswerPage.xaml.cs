using EcoVital.ViewModels;

namespace EcoVital.Views;

/// <summary>
/// Página para responder a la pregunta de seguridad.
/// </summary>
public partial class SecurityAnswerPage : ContentPage
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SecurityAnswerPage"/>.
    /// </summary>
    public SecurityAnswerPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SecurityAnswerPage"/> con el ID de usuario y la pregunta de seguridad.
    /// </summary>
    /// <param name="userId">El ID del usuario.</param>
    /// <param name="securityQuestion">La pregunta de seguridad del usuario.</param>
    public SecurityAnswerPage(int userId, string securityQuestion)
    {
        InitializeComponent();
        BindingContext = new SecurityAnswerPageViewModel(userId, securityQuestion);
    }

    /// <summary>
    /// Maneja el evento cuando se hace clic en el botón de retroceso.
    /// </summary>
    /// <param name="sender">El origen del evento.</param>
    /// <param name="e">Los datos del evento.</param>
    void OnBackButtonClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("LoginPage");
    }
}