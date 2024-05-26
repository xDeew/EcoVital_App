using EcoVital.Models;
using EcoVital.Services;
using EcoVital.ViewModels;

namespace EcoVital.Views;

/// <summary>
/// Página para enviar comentarios.
/// </summary>
public partial class FeedbackPage : ContentPage
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="FeedbackPage"/>.
    /// </summary>
    public FeedbackPage()
    {
        InitializeComponent();
        BindingContext = new FeedbackViewModel(new FeedbackService(new HttpClient()));
    }

    /// <summary>
    /// Maneja el evento de clic en el botón de enviar comentarios.
    /// </summary>
    /// <param name="sender">El origen del evento.</param>
    /// <param name="e">Los datos del evento.</param>
    async void OnSendFeedbackClicked(object sender, EventArgs e)
    {
        if (BindingContext is not FeedbackViewModel viewModel) return;
        await viewModel.PostFeedbackAsync(viewModel.CurrentFeedback);
        // Limpiar después de enviar
        viewModel.CurrentFeedback = new Feedback();
    }
}