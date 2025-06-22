# EcoVital

**EcoVital** is a personal chatbot project built using OpenAI's API. This project is intended for educational and portfolio purposes only.

## Configuration

To use the chatbot, you need an OpenAI API key. Follow these steps to configure it:

1. Create a folder named `Config` at the root of the project.
2. Inside the `Config` folder, create a file called `ApiConfig.cs`.
3. Define the `ApiConfig` class in `ApiConfig.cs` with the following content:

```csharp
namespace EcoVital.Config
{
    public static class ApiConfig
    {
        public static string ApiKey { get; } = "sk-xxxxxx"; // Replace with your actual API key
    }
}
