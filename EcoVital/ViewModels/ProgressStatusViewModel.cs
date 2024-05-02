using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using EcoVital.Models;
using EcoVital.Services;
using CommunityToolkit.Mvvm.Input;

namespace EcoVital.ViewModels
{
    public class ProgressStatusViewModel : BaseViewModel
    {
        readonly ActivityService _activityService;
        ObservableCollection<UserActivityRecord> _registeredActivities;
        public bool HasActivities => RegisteredActivities?.Any() == true;
        public bool HasNoActivities => !HasActivities;
        public ICommand SelectActivityCommand { get; set; }

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

        public RelayCommand<double> UpdateProgressCommand { get; private set; }


        public ProgressStatusViewModel(ActivityService activityService)
        {
            _activityService = activityService ?? throw new ArgumentNullException(nameof(activityService));
            UpdateProgressCommand = new RelayCommand<double>(UpdateProgress);
            SelectActivityCommand = new RelayCommand<UserActivityRecord>(SelectActivity!);
            _registeredActivities = new ObservableCollection<UserActivityRecord>();
            _registeredActivities.CollectionChanged += (s, e) => 
            {
                OnPropertyChanged(nameof(HasActivities));
                OnPropertyChanged(nameof(HasNoActivities));
            };
        }

        
        void SelectActivity(UserActivityRecord activity)
        {
            // if (activity == null) return;

            activity.IsSelected = !activity.IsSelected;

            OnPropertyChanged(nameof(RegisteredActivities));
        }

        async void UpdateProgress(double progressPercentage)
        {
            var selectedActivities = RegisteredActivities.Where(a => a.IsSelected).ToList();
            if (!selectedActivities.Any())
            {
                Debug.WriteLine("No activities selected.");
                return;
            }

            List<Task> tasks = new List<Task>();
            foreach (var activity in selectedActivities)
            {
                activity.Progress = progressPercentage;
                if (activity.Progress >= 0.995)
                {

                    ShowCongratulationMessage();  
                    // Agregar la tarea sin esperar inmediatamente
                    var task = RemoveActivityAsync(activity);
                    tasks.Add(task);
                }
            }

            // Esperar todas las tareas de eliminación
            await Task.WhenAll(tasks);

            OnPropertyChanged(nameof(RegisteredActivities));
        }

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


        void ShowCongratulationMessage()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Application.Current?.MainPage?.DisplayAlert("¡Felicidades!", "¡Completaste la actividad!", "OK");
            });
        }

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
                        var activityRecord =
                            allActivities.FirstOrDefault(a => a.RecordId == userActivity.ActivityRecordId);

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


        public string GetImageUrlForActivityType(string activityType)
        {
            switch (activityType)
            {
                case "Yoga":
                    return "yoga_image.jpeg";
                case "Running":
                    return "running.jpg";
                case "Swimming":
                    return "swimming_image.jpg";
                case "Gym":
                    return "strength_training.jpg";
                case "Cycling":
                    return "cycling.jpeg";
                case "Pilates":
                    return "pilates.jpg";
                case "Hiking":
                    return "hiking.jpg";
                case "Meditation":
                    return "meditation.jpg";
                case "Soccer":
                    return "soccer.jpg";
                case "HIIT":
                    return "hiit.png";
                case "Dancing":
                    return "dance.jpg";
                case "Kayaking":
                    return "kayak.jpg";
                default:
                    return "kayaking.jpg";
            }
        }
    }
}