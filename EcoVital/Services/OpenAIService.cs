using System.Net.Http.Headers;
using System.Text;
using EcoVital.Config;
using Newtonsoft.Json;

namespace EcoVital.Services;

/// <summary>
/// Proporciona servicios para interactuar con la API de OpenAI.
/// </summary>
public sealed class OpenAiService
{
    readonly HttpClient _httpClient;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="OpenAiService"/>.
    /// </summary>
    public OpenAiService()
    {
        var apiKey = ApiConfig.ApiKey;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
    }

    /// <summary>
    /// Obtiene una respuesta de la API de OpenAI para el prompt proporcionado.
    /// </summary>
    /// <param name="prompt">El prompt para el cual se desea obtener una respuesta.</param>
    /// <returns>Una tarea que representa la operación asincrónica y contiene la respuesta de la API de OpenAI.</returns>
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

        var errorResponse = await response.Content.ReadAsStringAsync();

        return $"Lo siento, hubo un error al obtener una respuesta. Detalles: {errorResponse}";
    }
}