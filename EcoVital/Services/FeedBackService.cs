using System.Diagnostics;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using EcoVital.Models;

namespace EcoVital.Services;

public class FeedbackService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "https://vivaservice.azurewebsites.net/api/feedback";

    public FeedbackService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

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
            var response = await _httpClient.PostAsync("https://vivaservice.azurewebsites.net/api/Feedback", content);
            var responseBody = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"Código de respuesta: {response.StatusCode}");
            Debug.WriteLine($"Cuerpo de respuesta: {responseBody}");

            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException httpRequestException)
        {
            Debug.WriteLine($"Error de solicitud HTTP: {httpRequestException.Message}");
            if (httpRequestException.InnerException != null)
            {
                Debug.WriteLine($"Detalles internos: {httpRequestException.InnerException.Message}");
            }

            return false;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error al enviar la solicitud: {ex.Message}");

            return false;
        }
    }


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