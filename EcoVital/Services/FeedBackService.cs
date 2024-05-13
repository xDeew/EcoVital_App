using System.Net.Http.Json;
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

    // Obtener todos los feedbacks
    public async Task<List<Feedback>> GetAllFeedbacksAsync()
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<List<Feedback>>(_baseUrl);

            return result ?? new List<Feedback>();
        }
        catch (Exception ex)
        {
            // Manejo de errores
            Console.WriteLine($"An error occurred: {ex.Message}");

            return new List<Feedback>();
        }
    }

    // Obtener un feedback por ID
    public async Task<Feedback> GetFeedbackByIdAsync(int id)
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<Feedback>($"{_baseUrl}/{id}");

            return result;
        }
        catch (Exception ex)
        {
            // Manejo de errores
            Console.WriteLine($"An error occurred: {ex.Message}");

            return null;
        }
    }

    // Enviar un nuevo feedback
    public async Task<bool> PostFeedbackAsync(Feedback feedback)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, feedback);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            // Manejo de errores
            Console.WriteLine($"An error occurred: {ex.Message}");

            return false;
        }
    }

    // Eliminar un feedback
    public async Task<bool> DeleteFeedbackAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            // Manejo de errores
            Console.WriteLine($"An error occurred: {ex.Message}");

            return false;
        }
    }
}