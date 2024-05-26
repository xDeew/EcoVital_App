namespace EcoVital.Views;

/// <summary>
/// Página de información sobre la aplicación.
/// </summary>
public partial class AboutPage
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="AboutPage"/>.
    /// </summary>
    public AboutPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Maneja el evento de clic del botón de feedback.
    /// </summary>
    /// <param name="sender">El origen del evento.</param>
    /// <param name="e">Los datos del evento.</param>
    async void OnFeedbackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(FeedbackPage));
    }
}