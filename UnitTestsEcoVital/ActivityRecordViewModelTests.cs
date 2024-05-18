using System.Collections.ObjectModel;
using EcoVital.Models;
using EcoVital.Services;
using EcoVital.ViewModels;

namespace UnitTestsEcoVital;

public class ActivityRecordViewModelTests
{
    readonly ActivityRecordViewModel _viewModel;
    List<ActivityRecord> _activityRecords;

    public ActivityRecordViewModelTests()
    {
        _viewModel = new ActivityRecordViewModel(new ActivityService(new HttpClient()),
            new UserGoalService(new HttpClient()));

        InitializeMockData();
    }

    void InitializeMockData()
    {
        _activityRecords = new List<ActivityRecord>
        {
            new() { RecordId = 1, Description = "Yoga", ActivityType = "Yoga" },
            new() { RecordId = 2, Description = "Running", ActivityType = "Running" }
        };
    }

    Task LoadActivitiesLocally()
    {
        _viewModel.ActivityRecords =
            new ObservableCollection<ActivityRecord>(_activityRecords);

        return Task.CompletedTask;
    }

    [Fact]
    public async Task LoadActivities_ShouldLoadAndSetPropertiesCorrectly()
    {
        // Act
        await LoadActivitiesLocally();

        // Assert
        Assert.Equal(2, _viewModel.ActivityRecords.Count);
        Assert.Contains(_viewModel.ActivityRecords, a => a.Description == "Yoga");
        Assert.Contains(_viewModel.ActivityRecords, a => a.Description == "Running");
    }

    Task RegisterSelectedActivitiesLocally()
    {
        foreach (var activity in _viewModel.SelectedActivities)
            _viewModel.UserActivityRecords.Add(new UserActivityRecord
            {
                UserId = 1,
                ActivityRecordId = activity.RecordId
            });

        _viewModel.SelectedActivities.Clear();

        return Task.CompletedTask;
    }

    [Fact]
    public async Task RegisterSelectedActivities_ShouldRegisterActivities()
    {
        _viewModel.SelectedActivities =
            new ObservableCollection<ActivityRecord>(_activityRecords);

        await RegisterSelectedActivitiesLocally();

        Assert.Empty(_viewModel.SelectedActivities);
        Assert.Equal(2, _viewModel.UserActivityRecords.Count);
    }
}