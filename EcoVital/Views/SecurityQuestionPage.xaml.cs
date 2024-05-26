using EcoVital.ViewModels;

namespace EcoVital.Views;

/// <summary>
/// Página para configurar la pregunta de seguridad del usuario.
/// </summary>
public partial class SecurityQuestionPage : ContentPage
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SecurityQuestionPage"/>.
    /// </summary>
    public SecurityQuestionPage()
    {
        InitializeComponent();
        BindingContext = new SecurityQuestionPageViewModel();
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