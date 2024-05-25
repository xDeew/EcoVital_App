using System.Net;
using System.Net.Http.Json;
using EcoVital.Models;

namespace EcoVital.Services;

public class ActivityService
{
    readonly string _apiBaseUrl = "https://vivaserviceapi.azurewebsites.net/api/activity";
    readonly string _apiBaseUrlUnion = "https://vivaserviceapi.azurewebsites.net/api/UserActivityRecords";
    readonly HttpClient _client;

    public ActivityService(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }


    public async Task<List<ActivityRecord>> GetActivityRecordsAsync()
    {
        var activityRecords = await _client.GetFromJsonAsync<List<ActivityRecord>>(_apiBaseUrl);

        var random = new Random();

        foreach (var activityRecord in activityRecords)
        {
            activityRecord.Date = DateTime.Today;

            activityRecord.Date = activityRecord.Date.AddHours(random.Next(0, 24));
        }

        return activityRecords;
    }

    public async Task<UserActivityRecord> RegisterUserActivityRecordAsync(UserActivityRecord userActivityRecord)
    {
        var response = await _client.PostAsJsonAsync(_apiBaseUrlUnion, userActivityRecord);
        response.EnsureSuccessStatusCode();

        var async = await response.Content.ReadFromJsonAsync<UserActivityRecord>();

        if (async == null) throw new InvalidOperationException();

        return async;
    }

    public async Task<IEnumerable<UserActivityRecord>> GetUserActivityRecordsAsync(int userId)
    {
        var url = $"{_apiBaseUrlUnion}/ByUser/{userId}";
        var response = await _client.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.NotFound) return new List<UserActivityRecord>();

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<IEnumerable<UserActivityRecord>>();
        var async = result;

        if (async != null) return async;

        return new List<UserActivityRecord>();
    }

    public async Task DeleteUserActivityRecordAsync(int recordId)
    {
        var url = $"{_apiBaseUrlUnion}/{recordId}";
        var response = await _client.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
    }
}