using EcoVital.Services;
using EcoVital.Views;
using Moq;

namespace UnitTestsEcoVital;

public class ChatBotPageTests
{
    [Fact]
    public void Constructor_InitializesWithWelcomeMessage()
    {
        var page = new ChatBotPage(new OpenAiService());

        Assert.Single(page.Messages);
        Assert.False(page.Messages[0].IsUserMessage);
        Assert.Contains("bienvenido", page.Messages[0].Text.ToLower());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void OnSendClicked_DoesNotAddEmptyMessages(string input)
    {
        var page = new ChatBotPage(new OpenAiService());
        page.UserInput.Text = input;

        page.OnSendClicked(null, null);

        Assert.Single(page.Messages); // Only the initial welcome message should be there
    }

    [Fact]
    public void OnSendClicked_AddsUserMessage()
    {
        var page = new ChatBotPage(new OpenAiService());
        page.UserInput.Text = "Hello, how are you?";

        page.OnSendClicked(null, null);

        Assert.Equal(2, page.Messages.Count);
        Assert.True(page.Messages[1].IsUserMessage);
        Assert.Equal("Hello, how are you?", page.Messages[1].Text);
    }

    [Fact]
    public async Task OnSendClicked_UsesOpenAiServiceForHighRelevance()
    {
        var mockService = new Mock<OpenAiService>();
        mockService.Setup(x => x.GetResponseAsync(It.IsAny<string>())).ReturnsAsync("Mock response");

        var page = new ChatBotPage(mockService.Object);
        page.UserInput.Text = "salud";

        page.OnSendClicked(null, null);

        mockService.Verify(x => x.GetResponseAsync(It.IsAny<string>()), Times.Once);
        Assert.Contains("Mock response", page.Messages[2].Text);
    }


    [Fact]
    public void OnSendClicked_HandlesLowRelevanceWithStandardMessage()
    {
        var page = new ChatBotPage(new OpenAiService());
        page.UserInput.Text = "Tell me a joke";

        page.OnSendClicked(null, null);

        Assert.Equal(3, page.Messages.Count);
        Assert.Contains("Solo puedo responder preguntas relacionadas con el bienestar y la salud.",
            page.Messages[2].Text);
    }

    // Additional tests can be added to cover more scenarios and ensure the chatbot's reliability.
}