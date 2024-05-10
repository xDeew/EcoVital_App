using Xunit;
using EcoVital.ViewModels;
using EcoVital.Models;
using Moq;
using EcoVital.Services;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestsEcoVital
{
    public class ProgressStatusViewModelTests
    {
        private readonly ProgressStatusViewModel _viewModel;
        private readonly Mock<ActivityService> _mockActivityService;

        public ProgressStatusViewModelTests()
        {
            _mockActivityService = new Mock<ActivityService>(new HttpClient());
            _viewModel = new ProgressStatusViewModel(_mockActivityService.Object);
        }

        [Fact]
        public void SelectActivity_TogglesIsSelected()
        {
            var activity = new UserActivityRecord { IsSelected = false };

            _viewModel.SelectActivity(activity);

            Assert.True(activity.IsSelected);
        }

        [Fact]
        public async Task UpdateProgress_UpdatesProgressOfSelectedActivities()
        {
            var activity1 = new UserActivityRecord { IsSelected = true, Progress = 0 };
            var activity2 = new UserActivityRecord { IsSelected = false, Progress = 0 };
            _viewModel.RegisteredActivities.Add(activity1);
            _viewModel.RegisteredActivities.Add(activity2);

            _viewModel.UpdateProgress(0.5);

            Assert.Equal(0.5, activity1.Progress);
            Assert.Equal(0, activity2.Progress);
        }
    }
}