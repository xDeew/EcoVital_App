using Xunit;
using Moq;
using EcoVital.ViewModels;
using EcoVital.Services;
using EcoVital.Models;

namespace UnitTestsEcoVital
{
    public class ForgotPasswordPageViewModelTests
    {
        private readonly ForgotPasswordPageViewModel _viewModel;
        private readonly Mock<ILoginRepository> _loginRepositoryMock;

        public ForgotPasswordPageViewModelTests()
        {
            _loginRepositoryMock = new Mock<ILoginRepository>();
            _viewModel = new ForgotPasswordPageViewModel(_loginRepositoryMock.Object);
        }

        [Fact]
        public async Task Send_WithEmptyEmail_ShouldNotCallGetUserByEmail()
        {
            // Arrange
            _viewModel.Email = string.Empty;

            // Act
             _viewModel.Send();

            // Assert
            _loginRepositoryMock.Verify(x => x.GetUserByEmail(It.IsAny<string>()), Times.Never);
        }

      
        
        [Fact]
        public async Task Send_WithValidEmailAndExistingUser_ShouldCallGetUserByEmail()
        {
            // Arrange
            var mockService = new Mock<ILoginRepository>();
            mockService.Setup(x => x.GetUserByEmail(It.IsAny<string>()))
                .ReturnsAsync(new UserInfo()); // Ensure GetUserByEmail returns a non-null value

            var viewModel = new ForgotPasswordPageViewModel(mockService.Object);
            viewModel.Email = "test@example.com";

            // Act
            viewModel.Send();

            // Assert
            mockService.Verify(x => x.GetUserByEmail(It.IsAny<string>()), Times.Once);
        }
        
        [Fact]
        public Task Send_WithValidEmailAndNonExistingUser_ShouldNotCallGetSecurityQuestionByUserId()
        {
            // Arrange
            _viewModel.Email = "test@test.com";
            _loginRepositoryMock.Setup(x => x.GetUserByEmail(It.IsAny<string>())).ReturnsAsync((UserInfo)null);

            // Act
            _viewModel.Send();

            // Assert
            _loginRepositoryMock.Verify(x => x.GetSecurityQuestionByUserId(It.IsAny<int>()), Times.Never);

            return Task.CompletedTask;
        }

        
    }
}