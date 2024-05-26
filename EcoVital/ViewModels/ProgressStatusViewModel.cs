using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using EcoVital.Models;
using EcoVital.Services;

namespace EcoVital.ViewModels;

/// <summary>
/// ViewModel para gestionar el estado de progreso de las actividades del usuario.
/// </summary>
public class ProgressStatusViewModel : BaseViewModel
{
    readonly ActivityService _activityService;
    ObservableCollection<UserActivityRecord> _registeredActivities;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ProgressStatusViewModel"/>.
    /// </summary>
    /// <param name="activityService">El servicio de actividades.</param>
    public ProgressStatusViewModel(ActivityService activityService)
    {
        _activityService = activityService ?? throw new ArgumentNullException(nameof(activityService));
        UpdateProgressCommand = new RelayCommand<double>(UpdateProgress);
        SelectActivityCommand = new RelayCommand<UserActivityRecord>(SelectActivity);
        _registeredActivities = new ObservableCollection<UserActivityRecord>();
        _registeredActivities.CollectionChanged += (s, e) =>
        {
            OnPropertyChanged(nameof(HasActivities));
            OnPropertyChanged(nameof(HasNoActivities));
        };
    }

    /// <summary>
    /// Indica si hay actividades registradas.
    /// </summary>
    public bool HasActivities => RegisteredActivities?.Any() == true;

    /// <summary>
    /// Indica si no hay actividades registradas.
    /// </summary>
    public bool HasNoActivities => !HasActivities;

    /// <summary>
    /// Comando para seleccionar una actividad.
    /// </summary>
    public ICommand SelectActivityCommand { get; set; }

    /// <summary>
    /// Colección de actividades registradas del usuario.
    /// </summary>
    public ObservableCollection<UserActivityRecord> RegisteredActivities
    {
        get => _registeredActivities;
        set
        {
            if (SetProperty(ref _registeredActivities, value))
            {
                OnPropertyChanged(nameof(HasActivities));
                OnPropertyChanged(nameof(HasNoActivities));
            }
        }
    }

    /// <summary>
    /// Comando para actualizar el progreso de una actividad.
    /// </summary>
    public RelayCommand<double> UpdateProgressCommand { get; private set; }

    /// <summary>
    /// Selecciona o deselecciona una actividad.
    /// </summary>
    /// <param name="activity">La actividad a seleccionar o deseleccionar.</param>
    public void SelectActivity(UserActivityRecord activity)
    {
        activity.IsSelected = !activity.IsSelected;
        OnPropertyChanged(nameof(RegisteredActivities));
    }

    /// <summary>
    /// Actualiza el progreso de las actividades seleccionadas.
    /// </summary>
    /// <param name="progressPercentage">El porcentaje de progreso.</param>
    public async void UpdateProgress(double progressPercentage)
    {
        var selectedActivities = RegisteredActivities.Where(a => a.IsSelected).ToList();
        if (!selectedActivities.Any())
        {
            Debug.WriteLine("No activities selected.");
            return;
        }

        var tasks = new List<Task>();
        foreach (var activity in selectedActivities)
        {
            activity.Progress = progressPercentage;
            if (activity.Progress >= 0.995)
            {
                ShowCongratulationMessage();
                tasks.Add(RemoveActivityAsync(activity));
            }
        }

        await Task.WhenAll(tasks);
        OnPropertyChanged(nameof(RegisteredActivities));
    }

    /// <summary>
    /// Elimina una actividad de la lista de actividades registradas.
    /// </summary>
    /// <param name="activity">La actividad a eliminar.</param>
    /// <returns>Una tarea que representa la operación asincrónica de eliminación de la actividad.</returns>
    async Task RemoveActivityAsync(UserActivityRecord activity)
    {
        await _activityService.DeleteUserActivityRecordAsync(activity.UserActivityId);
        RegisteredActivities.Remove(activity);
        OnPropertyChanged(nameof(RegisteredActivities));
        OnPropertyChanged(nameof(HasActivities));
        OnPropertyChanged(nameof(HasNoActivities));

        var userGoalService = new UserGoalService(new HttpClient());
        var userGoal = await userGoalService.GetUserGoalByActivityIdAsync(activity.ActivityRecordId);
        await userGoalService.DeleteUserGoalAsync(userGoal.GoalId);
    }

    /// <summary>
    /// Muestra un mensaje de felicitación al completar una actividad.
    /// </summary>
    void ShowCongratulationMessage()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Application.Current?.MainPage?.DisplayAlert("¡Felicidades!", "¡Completaste la actividad!", "OK");
        });
    }

    /// <summary>
    /// Carga las actividades registradas del usuario.
    /// </summary>
    /// <param name="userId">El identificador del usuario.</param>
    /// <returns>Una tarea que representa la operación asincrónica de carga de actividades.</returns>
    public async Task LoadRegisteredActivities(int userId)
    {
        try
        {
            var userActivities = await _activityService.GetUserActivityRecordsAsync(userId);
            var allActivities = await _activityService.GetActivityRecordsAsync();

            var updatedActivities = new ObservableCollection<UserActivityRecord>();

            foreach (var userActivity in userActivities)
            {
                var existingActivity = RegisteredActivities.FirstOrDefault(a =>
                    a.ActivityRecordId == userActivity.ActivityRecordId && a.UserId == userActivity.UserId);

                if (existingActivity != null)
                {
                    updatedActivities.Add(existingActivity);
                }
                else
                {
                    var activityRecord = allActivities.FirstOrDefault(a => a.RecordId == userActivity.ActivityRecordId);

                    if (activityRecord != null)
                    {
                        userActivity.ActivityType = activityRecord.ActivityType;
                        userActivity.ImageUrl = GetImageUrlForActivityType(activityRecord.ActivityType);
                        userActivity.Progress = 0;
                        updatedActivities.Add(userActivity);
                    }
                }
            }

            RegisteredActivities = updatedActivities;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading activities: {ex.Message}");
        }
    }

    /// <summary>
    /// Obtiene la URL de la imagen para un tipo de actividad específico.
    /// </summary>
    /// <param name="activityType">El tipo de actividad.</param>
    /// <returns>La URL de la imagen correspondiente al tipo de actividad.</returns>
    public string GetImageUrlForActivityType(string activityType)
    {
        return activityType switch
        {
            "Yoga" => "yoga_image.jpeg",
            "Running" => "running.jpg",
            "Swimming" => "swimming_image.jpg",
            "Gym" => "strength_training.jpg",
            "Cycling" => "cycling.jpeg",
            "Pilates" => "pilates.jpg",
            "Hiking" => "hiking.jpg",
            "Meditation" => "meditation.jpg",
            "Soccer" => "soccer.jpg",
            "HIIT" => "hiit.png",
            "Dancing" => "dance.jpg",
            "Kayaking" => "kayak.jpg",
            _ => "kayaking.jpg"
        };
    }
}