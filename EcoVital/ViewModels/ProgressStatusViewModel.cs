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

        public ICommand SelectActivityCommand { get; set; }

        public ObservableCollection<UserActivityRecord> RegisteredActivities
        {
            get => _registeredActivities;
            set => SetProperty(ref _registeredActivities, value);
        }

        public RelayCommand<double> UpdateProgressCommand { get; private set; }


        public ProgressStatusViewModel(ActivityService activityService)
        {
            _activityService = activityService ?? throw new ArgumentNullException(nameof(activityService));
            _registeredActivities = new ObservableCollection<UserActivityRecord>();
            UpdateProgressCommand = new RelayCommand<double>(UpdateProgress);
            SelectActivityCommand = new RelayCommand<UserActivityRecord>(SelectActivity!);
        }

        void SelectActivity(UserActivityRecord activity)
        {
            // if (activity == null) return;

            activity.IsSelected = !activity.IsSelected;

            OnPropertyChanged(nameof(RegisteredActivities));
        }

        void UpdateProgress(double progressPercentage)
        {
            var selectedActivities = RegisteredActivities.Where(a => a.IsSelected).ToList();
            if (!selectedActivities.Any())
            {
                Debug.WriteLine("No activities selected.");

                return;
            }

            foreach (var activity in selectedActivities)
            {
                activity.Progress = progressPercentage;
            }


            OnPropertyChanged(nameof(RegisteredActivities));
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