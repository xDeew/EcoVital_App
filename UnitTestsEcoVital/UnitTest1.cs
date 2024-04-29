using EcoVital;
using Xunit;
using NSubstitute;
using EcoVital.ViewModels;
using EcoVital.Services;
using EcoVital.Models;
using FluentAssertions;

namespace UnitTestsEcoVital
{
    public class LoginPageViewModelTests
    {
        [Fact]
        public async Task Login_ValidCredentials_ReturnsUserInfo()
        {
            // Arrange
            var loginRepository = Substitute.For<ILoginRepository>();
            var userInfo = new UserInfo { UserName = "TestUser", Password = "TestPassword" };
            loginRepository.Login("TestUser", "TestPassword").Returns(Task.FromResult(userInfo));

            var viewModel = new LoginPageViewModel(loginRepository)
            {
                UsernameOrEmail = "TestUser",
                Password = "TestPassword"
            };

            // Act
            await viewModel.Login();

            // Assert
            await loginRepository.Received().Login("TestUser", "TestPassword");
            App.UserInfo.UserName.Should().Be("TestUser");
        }
    }
}