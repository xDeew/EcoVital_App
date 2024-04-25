using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using EcoVital.Models;
using EcoVital.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace EcoVital.ViewModels
{
    public class ActivityRecordViewModel : BaseViewModel
    {
        private readonly ActivityService _activityService;
        private ObservableCollection<UserActivityRecord> _userActivityRecords;
        private ObservableCollection<ActivityRecord> _activityRecords;

        public ObservableCollection<UserActivityRecord> UserActivityRecords
        {
            get => _userActivityRecords;
            private set => SetProperty(ref _userActivityRecords, value);
        }

        public ObservableCollection<ActivityRecord> ActivityRecords
        {
            get => _activityRecords;
            private set => SetProperty(ref _activityRecords, value);
        }

        public ObservableCollection<ActivityRecord> SelectedActivities { get; set; }
        public IAsyncRelayCommand LoadActivitiesCommand { get; }
        public IAsyncRelayCommand<UserActivityRecord> RegisterUserActivityCommand { get; }
        public ICommand RegisterSelectedActivitiesCommand { get; set; }
        public ICommand SelectActivityCommand { get; set; }


        public ActivityRecordViewModel(ActivityService activityService)
        {
            _activityService = activityService;
            if (_activityService == null)
            {
                throw new ArgumentNullException(nameof(activityService));
            }

            _activityRecords = new ObservableCollection<ActivityRecord>();
            _userActivityRecords = new ObservableCollection<UserActivityRecord>();

            LoadActivitiesCommand = new AsyncRelayCommand(LoadActivitiesAsync);
            RegisterUserActivityCommand = new AsyncRelayCommand<UserActivityRecord>(RegisterUserActivityAsync);

            RegisterSelectedActivitiesCommand = new AsyncRelayCommand(RegisterSelectedActivitiesAsync);
            SelectedActivities = new ObservableCollection<ActivityRecord>();

           
            SelectActivityCommand = new RelayCommand<ActivityRecord>(SelectActivity);
        }


        private void SelectActivity(ActivityRecord activityRecord)
        {
            activityRecord.IsSelected = !activityRecord.IsSelected;

            if (!SelectedActivities.Contains(activityRecord))
            {
                SelectedActivities.Add(activityRecord);
            }
            else
            {
                SelectedActivities.Remove(activityRecord);
            }
        }

        private async Task RegisterSelectedActivitiesAsync()
        {
            if (SelectedActivities.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Aviso", "No has seleccionado ninguna actividad.",
                    "OK");

                return;
            }

            try
            {
                foreach (var activity in SelectedActivities)
                {
                    
                    var existingActivity = UserActivityRecords.FirstOrDefault(a =>
                        a.ActivityRecordId == activity.RecordId && a.UserId == App.UserInfo.UserId);

                    
                    if (existingActivity != null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Aviso",
                            $"La actividad '{activity.Description}' ya ha sido registrada.", "OK");

                        
                        activity.IsSelected = false;
                        SelectedActivities
                            .Remove(activity); 

                        return;
                    }

                    var userActivityRecord = new UserActivityRecord
                    {
                        UserId = App.UserInfo.UserId,
                        ActivityRecordId = activity.RecordId,
                    };

                    
                    var registeredActivity = await _activityService.RegisterUserActivityRecordAsync(userActivityRecord);
                    UserActivityRecords.Add(registeredActivity);
                }

                await Application.Current.MainPage.DisplayAlert("Éxito", "Actividades registradas con éxito.", "OK");
                await SaveSelectedActivitiesAsync();
                
                foreach (var activity in SelectedActivities)
                {
                    activity.IsSelected = false;
                }

                SelectedActivities.Clear();
                OnPropertyChanged(nameof(SelectedActivities));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error",
                    $"Error al registrar actividades: {ex.Message}", "OK");
            }
        }
        

        private async Task LoadActivitiesAsync()
        {
            await LoadUserActivityRecordsAsync(); 
            var activityRecords = await _activityService.GetActivityRecordsAsync();
            ActivityRecords.Clear();
            foreach (var activityRecord in activityRecords)
            {
                activityRecord.ImageUrl = GetImageUrlForActivityType(activityRecord.ActivityType);
                activityRecord.IsSelected = false; 

                ActivityRecords.Add(activityRecord);
            }

            OnPropertyChanged(nameof(SelectedActivities));

           
            Preferences.Remove("SelectedActivities");

            await LoadSelectedActivitiesAsync(); 
        }


        public async Task LoadUserActivityRecordsAsync()
        {
            var userActivities = await _activityService.GetUserActivityRecordsAsync(App.UserInfo.UserId);
            UserActivityRecords.Clear();
            foreach (var activity in userActivities)
            {
                UserActivityRecords.Add(activity);
            }
        }


        private async Task LoadSelectedActivitiesAsync()
        {
            var selectedIdsString = Preferences.Get("SelectedActivities", string.Empty);
            if (!string.IsNullOrEmpty(selectedIdsString))
            {
                var selectedActivityIds = selectedIdsString.Split(',').Select(int.Parse).ToList();
                foreach (var activity in ActivityRecords)
                {
                    if (selectedActivityIds.Contains(activity.RecordId) && !UserActivityRecords.Any(a =>
                            a.ActivityRecordId == activity.RecordId && a.UserId == App.UserInfo.UserId))
                    {
                        activity.IsSelected = true;
                        SelectedActivities.Add(activity);
                    }
                }
            }
        }


        private async Task SaveSelectedActivitiesAsync()
        {
            var selectedActivityIds = SelectedActivities.Select(a => a.RecordId.ToString()).ToArray();
            var selectedIdsString = string.Join(",", selectedActivityIds);
            Preferences.Set("SelectedActivities", selectedIdsString);
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


        private async Task RegisterUserActivityAsync(UserActivityRecord userActivityRecord)
        {
            // The following will register an activity for a user
            var registeredActivity = await _activityService.RegisterUserActivityRecordAsync(userActivityRecord);
            UserActivityRecords.Add(registeredActivity);
        }
    }
}