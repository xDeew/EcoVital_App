namespace EcoVital.Services;

public class HealthReminderService
{
    readonly HttpClient _client;
    readonly string _apiBaseUrl = "https://vivaservice.azurewebsites.net/api/healthreminder";
}