using EcoVital;
using EcoVital.Models;
using EcoVital.Services;
using EcoVital.ViewModels;
using Moq;

namespace UnitTestsEcoVital;

public class LoginRegisterTests
{
    readonly Mock<ILoadingService> _loadingService;
    readonly Mock<ILoginRepository> _loginRepository;
    readonly RegisterPageViewModel _registerPageViewModel;
    readonly SecurityQuestionPageViewModelTests _securityQuestionPageViewModelTests;

    public LoginRegisterTests()
    {
        _loginRepository = new Mock<ILoginRepository>();
        _loadingService = new Mock<ILoadingService>();
        _registerPageViewModel = new RegisterPageViewModel();
        _securityQuestionPageViewModelTests = new SecurityQuestionPageViewModelTests();
    }


    async Task LoginForTesting(string usernameOrEmail, string password)
    {
        if (!string.IsNullOrWhiteSpace(usernameOrEmail) && !string.IsNullOrWhiteSpace(password))
        {
            var userExists = await _loginRepository.Object.UserExists(usernameOrEmail);

            if (!userExists) return;

            var userInfo = await _loginRepository.Object.Login(usernameOrEmail, password);

            if (userInfo == null) return;

            App.UserInfo = userInfo;
        }
    }

    [Fact]
    public async Task Login_WithValidCredentials_ShouldReturnUserInfo()
    {
        string username;
        // Arrange
        var userInfo = new UserInfo
        {
            UserName = "justTesting",
            Password = "IamTesting123",
            Email = "Iamtesting@outlook.es"
        };

        _loginRepository.Setup(repo => repo.UserExists("justTesting")).ReturnsAsync(true);
        _loginRepository.Setup(repo => repo.Login("justTesting", "IamTesting123")).ReturnsAsync(userInfo);

        App.UserInfo = new UserInfo
        {
            UserName = "justTesting",
            Password = "IamTesting123"
        };

        // Act
        username = App.UserInfo.UserName;
        await LoginForTesting(username, "IamTesting123");
        // Assert
        _loginRepository.Verify(repo => repo.Login("justTesting", "IamTesting123"), Times.Once);
        Assert.NotNull(App.UserInfo);
        Assert.Equal(userInfo.UserName, App.UserInfo.UserName);
        Assert.Equal(userInfo.Password, App.UserInfo.Password);
        Assert.Equal(userInfo.Email, App.UserInfo.Email);
    }

    async Task RegisterForTesting(string email, string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
            return;

        if (string.IsNullOrEmpty(email) || !email.Contains("@")) return;

        if (_registerPageViewModel.IsPasswordSecure(password)) return;


        var userInfo = await _loginRepository.Object.Register(email, username, password);

        if (userInfo == null) return;

        App.UserInfo = userInfo;
    }

    [Fact]
    public async Task Register_WithValidInputs_ShouldReturnUserInfo()
    {
        // Arrange
        var userInfo = new UserInfo
        {
            UserName = "testUser",
            Password = "TestPassword123",
            Email = "testuser@outlook.es"
        };

        _loginRepository.Setup(repo => repo.UserExists("testUser")).ReturnsAsync(false);
        _loginRepository.Setup(repo => repo.Register("testuser@outlook.es", "testUser", "TestPassword123"))
            .ReturnsAsync(userInfo);

        // Act
        await RegisterForTesting("testuser@outlook.es", "testUser", "TestPassword123");

        // Assert
        _loginRepository.Verify(repo => repo.Register("testuser@outlook.es", "testUser", "TestPassword123"),
            Times.Once);

        Assert.NotNull(App.UserInfo);
        Assert.Equal("testUser", App.UserInfo.UserName);
        Assert.Equal("TestPassword123", App.UserInfo.Password);
        Assert.Equal("testuser@outlook.es", App.UserInfo.Email);
    }


    async Task SecurityQuestionForTesting(string question, string answer)
    {
        // Initialize App.UserInfo if it's null
        if (App.UserInfo == null)
            App.UserInfo = new UserInfo
            {
                UserId = 1,
                UserName = "testUser",
                Password = "TestPassword123",
                Email = "testuser@outlook.es"
            };

        // Arrange
        var securityQuestion = new SecurityQuestion
        {
            UserId = App.UserInfo.UserId,
            QuestionText = question,
            Answer = answer
        };

        // Act
        // ya que estoy creando manualmente el objeto de la pregunta de seguridad, no necesito hacer un setup

        // Assert

        Assert.NotNull(securityQuestion);
        Assert.Equal(App.UserInfo.UserId, securityQuestion.UserId);
        Assert.Equal(question, securityQuestion.QuestionText);
        Assert.Equal(answer, securityQuestion.Answer);
    }

    [Fact]
    public async Task SecurityQuestionForTesting_ShouldCreateSecurityQuestion()
    {
        // Arrange
        var question = "¿Cuál es el nombre de tu primer profesor?";
        var answer = "Profesor X";

        // Act
        await SecurityQuestionForTesting(question, answer);

        // Assert
        // Las verificaciones se realizan dentro del método SecurityQuestionForTesting
    }
}