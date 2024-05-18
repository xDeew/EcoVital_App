using EcoVital.ViewModels;

namespace UnitTestsEcoVital;

public class SecurityQuestionPageViewModelTests
{
    readonly SecurityQuestionPageViewModel _viewModel;

    public SecurityQuestionPageViewModelTests()
    {
        _viewModel = new SecurityQuestionPageViewModel();
    }

    [Fact]
    public void SecurityQuestions_InitializedCorrectly()
    {
        Assert.NotEmpty(_viewModel.SecurityQuestions);
    }

    [Fact]
    public void ContinueCommand_InitializedCorrectly()
    {
        Assert.NotNull(_viewModel.ContinueCommand);
    }

    [Fact]
    public void ContinueCommand_CanExecute()
    {
        _viewModel.SelectedSecurityQuestion = _viewModel.SecurityQuestions[0];
        _viewModel.SecurityAnswer = "Test answer";

        var canExecute = _viewModel.ContinueCommand.CanExecute(null);
        Assert.True(canExecute);
    }
}