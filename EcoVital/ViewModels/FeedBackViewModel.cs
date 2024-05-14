using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using EcoVital.Models;
using EcoVital.Services;
using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace EcoVital.ViewModels
{
    public class FeedbackViewModel : BaseViewModel
    {
        private readonly FeedbackService _feedbackService;
        public Feedback CurrentFeedback { get; set; } = new Feedback();

        public ObservableCollection<Feedback> Feedbacks { get; private set; }
        public ICommand LoadFeedbacksCommand { get; }
        public ICommand PostFeedbackCommand { get; }
        public ICommand DeleteFeedbackCommand { get; }

        public FeedbackViewModel(FeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
            Feedbacks = new ObservableCollection<Feedback>();

            LoadFeedbacksCommand = new Command(async () => await LoadFeedbacksAsync());
            PostFeedbackCommand = new Command<Feedback>(async (feedback) => await PostFeedbackAsync(feedback));
            DeleteFeedbackCommand = new Command<int>(async (feedbackId) => await DeleteFeedbackAsync(feedbackId));
        }

        private async Task LoadFeedbacksAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var feedbackList = await _feedbackService.GetAllFeedbacksAsync();
                Feedbacks.Clear();
                foreach (var feedback in feedbackList)
                {
                    Feedbacks.Add(feedback);
                }
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

        public async Task PostFeedbackAsync(Feedback feedback)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Debug.WriteLine($"Email: {feedback.Email}");
                Debug.WriteLine($"Message: {feedback.Message}");
                Debug.WriteLine($"Type: {feedback.Type}");

                if (string.IsNullOrEmpty(feedback.Message) ||
                    string.IsNullOrEmpty(feedback.Type))
                {
                    await Application.Current.MainPage.DisplayAlert("Error",
                        "Por favor, completa todos los campos requeridos.", "OK");

                    return;
                }

                var success = await _feedbackService.PostFeedbackAsync(feedback);
                if (success)
                {
                    Feedbacks.Add(feedback);
                    await Application.Current.MainPage.DisplayAlert("Éxito", "Tu comentario ha sido enviado con éxito.",
                        "OK");

                    // clean all the fields
                    CurrentFeedback.Email = string.Empty;
                    CurrentFeedback.Message = string.Empty;
                    CurrentFeedback.Type = string.Empty;
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

        private async Task DeleteFeedbackAsync(int feedbackId)
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
}