using System.Diagnostics;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using EcoVital.Models;

namespace EcoVital.Services;

/// <summary>
/// Proporciona servicios relacionados con el feedback.
/// </summary>
public class FeedbackService
{
    readonly string _baseUrl = "https://vivaserviceapi.azurewebsites.net/api/feedback";
    readonly HttpClient _httpClient;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="FeedbackService"/>.
    /// </summary>
    /// <param name="httpClient">Instancia de <see cref="HttpClient"/> para realizar solicitudes HTTP.</param>
    public FeedbackService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Obtiene todos los feedbacks.
    /// </summary>
    /// <returns>Una lista de <see cref="Feedback"/>.</returns>
    public async Task<List<Feedback>> GetAllFeedbacksAsync()
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<List<Feedback>>(_baseUrl);

            return result ?? new List<Feedback>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");

            return new List<Feedback>();
        }
    }

    /// <summary>
    /// Obtiene un feedback por su identificador.
    /// </summary>
    /// <param name="id">El identificador del feedback.</param>
    /// <returns>El <see cref="Feedback"/> solicitado.</returns>
    public async Task<Feedback> GetFeedbackByIdAsync(int id)
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<Feedback>($"{_baseUrl}/{id}");

            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");

            return null;
        }
    }

    /// <summary>
    /// Envía un nuevo feedback.
    /// </summary>
    /// <param name="feedback">El feedback a enviar.</param>
    /// <returns><c>true</c> si el feedback se envió correctamente; de lo contrario, <c>false</c>.</returns>
    public async Task<bool> PostFeedbackAsync(Feedback feedback)
    {
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        var json = JsonSerializer.Serialize(feedback, jsonOptions);
        Debug.WriteLine($"JSON a enviar: {json}");

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var response = await _httpClient.PostAsync("https://vivaserviceapi.azurewebsites.net/api/Feedback", content);
            var responseBody = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"Código de respuesta: {response.StatusCode}");
            Debug.WriteLine($"Cuerpo de respuesta: {responseBody}");

            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException httpRequestException)
        {
            Debug.WriteLine($"Error de solicitud HTTP: {httpRequestException.Message}");
            if (httpRequestException.InnerException != null)
                Debug.WriteLine($"Detalles internos: {httpRequestException.InnerException.Message}");

            return false;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error al enviar la solicitud: {ex.Message}");

            return false;
        }
    }

    /// <summary>
    /// Elimina un feedback por su identificador.
    /// </summary>
    /// <param name="id">El identificador del feedback a eliminar.</param>
    /// <returns><c>true</c> si el feedback se eliminó correctamente; de lo contrario, <c>false</c>.</returns>
    public async Task<bool> DeleteFeedbackAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");

            return false;
        }
    }
}