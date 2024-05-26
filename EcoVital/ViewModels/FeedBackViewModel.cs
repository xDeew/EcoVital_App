using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using EcoVital.Models;
using EcoVital.Services;

namespace EcoVital.ViewModels;

/// <summary>
/// ViewModel para gestionar el feedback de los usuarios.
/// </summary>
public class FeedbackViewModel : BaseViewModel
{
    readonly FeedbackService _feedbackService;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="FeedbackViewModel"/>.
    /// </summary>
    /// <param name="feedbackService">El servicio de feedback.</param>
    public FeedbackViewModel(FeedbackService feedbackService)
    {
        _feedbackService = feedbackService;
        Feedbacks = new ObservableCollection<Feedback>();

        LoadFeedbacksCommand = new Command(async () => await LoadFeedbacksAsync());
        PostFeedbackCommand = new Command<Feedback>(async feedback => await PostFeedbackAsync(feedback));
        DeleteFeedbackCommand = new Command<int>(async feedbackId => await DeleteFeedbackAsync(feedbackId));
    }

    /// <summary>
    /// Obtiene o establece el feedback actual.
    /// </summary>
    public Feedback CurrentFeedback { get; set; } = new();

    /// <summary>
    /// Obtiene la colección de feedbacks.
    /// </summary>
    public ObservableCollection<Feedback> Feedbacks { get; }

    /// <summary>
    /// Comando para cargar los feedbacks.
    /// </summary>
    public ICommand LoadFeedbacksCommand { get; }

    /// <summary>
    /// Comando para enviar un feedback.
    /// </summary>
    public ICommand PostFeedbackCommand { get; }

    /// <summary>
    /// Comando para eliminar un feedback.
    /// </summary>
    public ICommand DeleteFeedbackCommand { get; }

    /// <summary>
    /// Carga los feedbacks de los usuarios.
    /// </summary>
    /// <returns>Una tarea que representa la operación asincrónica de cargar los feedbacks.</returns>
    async Task LoadFeedbacksAsync()
    {
        if (IsBusy)
            return;

        IsBusy = true;

        try
        {
            var feedbackList = await _feedbackService.GetAllFeedbacksAsync();
            Feedbacks.Clear();
            foreach (var feedback in feedbackList) Feedbacks.Add(feedback);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error al cargar los comentarios: {ex.Message}");
            await Application.Current.MainPage.DisplayAlert("Error",
                "No se pudieron cargar los comentarios. Por favor, inténtalo de nuevo más tarde.", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// Envía un feedback.
    /// </summary>
    /// <param name="feedback">El feedback a enviar.</param>
    /// <returns>Una tarea que representa la operación asincrónica de enviar el feedback.</returns>
    public async Task PostFeedbackAsync(Feedback feedback)
    {
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await Application.Current.MainPage.DisplayAlert("Error",
                "Se requiere conexión a Internet para enviar feedback.", "OK");

            return;
        }

        if (IsBusy)
            return;

        IsBusy = true;

        try
        {
            Debug.WriteLine($"Email: {feedback.Email}");
            Debug.WriteLine($"Message: {feedback.Message}");
            Debug.WriteLine($"Type: {feedback.Type}");

            if (string.IsNullOrEmpty(feedback.Message) || string.IsNullOrEmpty(feedback.Type))
            {
                await Application.Current.MainPage.DisplayAlert("Error",
                    "Por favor, completa todos los campos requeridos.", "OK");

                // Limpiar todos los campos
                CurrentFeedback.Email = string.Empty;
                CurrentFeedback.Message = string.Empty;

                return;
            }

            var success = await _feedbackService.PostFeedbackAsync(feedback);
            if (success)
            {
                Feedbacks.Add(feedback);
                await Application.Current.MainPage.DisplayAlert("Éxito", "Tu comentario ha sido enviado con éxito.",
                    "OK");

                // Limpiar todos los campos
                CurrentFeedback.Email = string.Empty;
                CurrentFeedback.Message = string.Empty;
                CurrentFeedback.Type = "Default"; // reemplazar "Default" con tu tipo predeterminado
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error",
                    "No se pudo enviar el comentario. Por favor, inténtalo de nuevo.", "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error al enviar el comentario: {ex.Message}");
            await Application.Current.MainPage.DisplayAlert("Error",
                "Ocurrió un error al enviar tu comentario. Por favor, inténtalo de nuevo más tarde.", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// Elimina un feedback.
    /// </summary>
    /// <param name="feedbackId">El identificador del feedback a eliminar.</param>
    /// <returns>Una tarea que representa la operación asincrónica de eliminar el feedback.</returns>
    async Task DeleteFeedbackAsync(int feedbackId)
    {
        if (IsBusy)
            return;

        IsBusy = true;

        try
        {
            var success = await _feedbackService.DeleteFeedbackAsync(feedbackId);
            if (success)
            {
                var feedback = Feedbacks.FirstOrDefault(f => f.FeedbackId == feedbackId);
                if (feedback != null)
                {
                    Feedbacks.Remove(feedback);
                    await Application.Current.MainPage.DisplayAlert("Éxito",
                        "El comentario ha sido eliminado con éxito.", "OK");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error",
                    "No se pudo eliminar el comentario. Por favor, inténtalo de nuevo.", "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error al eliminar el comentario: {ex.Message}");
            await Application.Current.MainPage.DisplayAlert("Error",
                "Ocurrió un error al eliminar tu comentario. Por favor, inténtalo de nuevo más tarde.", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}