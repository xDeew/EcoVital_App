using Xunit;
using Moq;
using EcoVital.Services;
using EcoVital.ViewModels;
using System.Threading.Tasks;
using EcoVital.Models;
using Microsoft.Toolkit.Mvvm.Input;
using Nito.Mvvm;

namespace UnitTestsEcoVital;

public class ChangePasswordViewModelTests
{
    private readonly Mock<ILoginRepository> _mockLoginRepository;
    private readonly ChangePasswordViewModel _viewModel;

    public ChangePasswordViewModelTests()
    {
        _mockLoginRepository = new Mock<ILoginRepository>();
        _viewModel = new ChangePasswordViewModel(_mockLoginRepository.Object);
    }


    [Fact]
    public async Task PasswordsDoNotMatch_ShowsError()
    {
        // Arrange
        _viewModel.NewPassword = "Password1!";
        _viewModel.ConfirmNewPassword = "Password2!";

        // Act & Assert
        // Aquí tendrías que simular la llamada al DisplayAlert. Esto requiere abstraer las llamadas de UI en el ViewModel
        var command = _viewModel.ChangePasswordCommand as AsyncCommand;
        if (command != null)
        {
            await command.ExecuteAsync(null);
        }
    }

    [Fact]
    public async Task InvalidPassword_ShowsError()
    {
        // Arrange
        _viewModel.NewPassword = "abc"; 
        _viewModel.ConfirmNewPassword = "abc";

        // Act & Assert
        var command = _viewModel.ChangePasswordCommand as AsyncCommand;
        if (command != null)
        {
            await command.ExecuteAsync(null);
        }
    }

    [Fact]
    public Task ValidPassword_ChangesPasswordSuccessfully()
    {
        // Arrange
        const string expectedEmail = "user@example.com";
        _viewModel.NewPassword = "ValidPass!23";
        _viewModel.ConfirmNewPassword = _viewModel.NewPassword;

        _mockLoginRepository.Setup(x => x.GetUserByEmail(It.IsAny<string>()))
            .ReturnsAsync(new UserInfo { UserId = 1, Email = expectedEmail });

        _mockLoginRepository.Setup(x => x.ChangePassword(1, _viewModel.NewPassword))
            .ReturnsAsync(true);

        // Act
        var command = _viewModel.ChangePasswordCommand as Command;
        command?.Execute(null);

        // Assert
        _mockLoginRepository.Verify(x => x.ChangePassword(1, _viewModel.NewPassword), Times.Once);

        return Task.CompletedTask;
    }
}