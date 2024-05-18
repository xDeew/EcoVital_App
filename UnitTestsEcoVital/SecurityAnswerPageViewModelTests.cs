using EcoVital.ViewModels;

namespace UnitTestsEcoVital;

public class SecurityAnswerPageViewModelTests
{
    readonly SecurityAnswerPageViewModel _viewModel;

    public SecurityAnswerPageViewModelTests()
    {
        _viewModel = new SecurityAnswerPageViewModel(1, "¿Cuál es tu color favorito?");
    }

    [Fact]
    public void Question_InitializedCorrectly()
    {
        Assert.Equal("¿Cuál es tu color favorito?", _viewModel.Question);
    }

    [Fact]
    public void CheckAnswerCommand_InitializedCorrectly()
    {
        Assert.NotNull(_viewModel.CheckAnswerCommand);
    }

    [Fact]
    public void CheckAnswerCommand_CanExecute()
    {
        _viewModel.Answer = "Azul";

        var canExecute = _viewModel.CheckAnswerCommand.CanExecute(null);
        Assert.True(canExecute);
    }
}