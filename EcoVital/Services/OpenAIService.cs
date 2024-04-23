using System.Text;
using Newtonsoft.Json;

namespace EcoVital.Services;

public class OpenAiService
{
    private readonly HttpClient _httpClient;

    public OpenAiService()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",
                "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");
    }

    public async Task<string> GetResponseAsync(string prompt)
    {
        var data = new
        {
            model = "gpt-3.5-turbo", 
            messages = new[]
            {
                new { role = "user", content = prompt }
            }
        };

        var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

       
        var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

            return result.choices[0].message.content;
        }
        else
        {
            var errorResponse = await response.Content.ReadAsStringAsync();

            return $"Lo siento, hubo un error al obtener una respuesta. Detalles: {errorResponse}";
        }
    }
}