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

    [Fact]
    public void AddReminderCommand_InitializedCorrectly()
    {
        Assert.NotNull(_viewModel.AddReminderCommand);
    }

    [Fact]
    public void AddReminderCommand_CanExecute()
    {
        var reminder = new HealthReminder
        {
            ReminderType = "Test",
            ReminderMessage = "Test message",
            ImageSource = "test.png",
            ReminderTime = "12:00"
        };

        var canExecute = _viewModel.AddReminderCommand.CanExecute(reminder);
        Assert.True(canExecute);
    }
}