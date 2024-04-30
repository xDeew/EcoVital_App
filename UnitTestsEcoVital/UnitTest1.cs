using EcoVital;
using Moq;
using Xunit;
using EcoVital.ViewModels;
using EcoVital.Services;
using EcoVital.Models;

namespace UnitTestsEcoVital
{
    public class LoginPageViewModelTests
    {
        private readonly Mock<ILoginRepository> _loginRepository;
        private readonly Mock<ILoadingService> _loadingService;
        private readonly LoginPageViewModel _viewModel;

        public LoginPageViewModelTests()
        {
            _loginRepository = new Mock<ILoginRepository>();
            _loadingService = new Mock<ILoadingService>();
            _viewModel = new LoginPageViewModel(_loginRepository.Object, _loadingService.Object);
        }

        async Task LoginForTesting(string usernameOrEmail, string password)
        {
            if (!string.IsNullOrWhiteSpace(usernameOrEmail) && !string.IsNullOrWhiteSpace(password))
            {
                bool userExists = await _loginRepository.Object.UserExists(usernameOrEmail);
                if (!userExists)
                {
                    return;
                }

                UserInfo userInfo = await _loginRepository.Object.Login(usernameOrEmail, password);
                if (userInfo == null)
                {
                    return;
                }

                App.UserInfo = userInfo;
            }
        }

        [Fact]
        public async Task Login_WithValidCredentials_ShouldReturnUserInfo()
        {
            // Arrange
            var userInfo = new UserInfo
            {
                UserName = "justTesting",
                Password = "IamTesting123",
                Email = "Iamtesting@outlook.es"
            };
            _loginRepository.Setup(repo => repo.UserExists("justTesting")).ReturnsAsync(true);
            _loginRepository.Setup(repo => repo.Login("justTesting", "IamTesting123")).ReturnsAsync(userInfo);

            // Act
            await LoginForTesting("justTesting", "IamTesting123");

            // Assert
            _loginRepository.Verify(repo => repo.Login("justTesting", "IamTesting123"), Times.Once);
            Assert.NotNull(App.UserInfo);
            Assert.Equal("justTesting", App.UserInfo.UserName);
            Assert.Equal("IamTesting123", App.UserInfo.Password);
            Assert.Equal("Iamtesting@outlook.es", App.UserInfo.Email);
        }
    }
}