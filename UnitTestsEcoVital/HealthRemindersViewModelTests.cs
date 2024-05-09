using EcoVital.Models;
using EcoVital.ViewModels;

namespace UnitTestsEcoVital;

public class HealthRemindersViewModelTests
{
    readonly HealthRemindersViewModel _viewModel;

    public HealthRemindersViewModelTests()
    {
        _viewModel = new HealthRemindersViewModel();
    }

    [Fact]
    public void Reminders_InitializedCorrectly()
    {
        Assert.NotEmpty(_viewModel.Reminders);
    }
}