using System.Net.Http.Json;
using EcoVital.Models;

namespace EcoVital.Services
{
    public class ActivityService
    {
        readonly HttpClient _client;
        readonly string _apiBaseUrl = "https://vivaservice.azurewebsites.net/api/activity";
        readonly string _apiBaseUrlUnion = "https://vivaservice.azurewebsites.net/api/UserActivityRecords";

        public ActivityService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<List<ActivityRecord>> GetActivityRecordsAsync()
        {
            var activityRecords = await _client.GetFromJsonAsync<List<ActivityRecord>>(_apiBaseUrl);

            // Crear un generador de números aleatorios
            var random = new Random();

            foreach (var activityRecord in activityRecords)
            {
                // Actualizar la fecha a la fecha actual
                activityRecord.Date = DateTime.Today;

                // Simular las horas agregando un número aleatorio de horas a la fecha actual
                activityRecord.Date = activityRecord.Date.AddHours(random.Next(0, 24));
            }

            return activityRecords;
        }
        
        public async Task<UserActivityRecord> RegisterUserActivityRecordAsync(UserActivityRecord userActivityRecord)
        {
            var response = await _client.PostAsJsonAsync(_apiBaseUrlUnion, userActivityRecord);
            response.EnsureSuccessStatusCode();

            var async = await response.Content.ReadFromJsonAsync<UserActivityRecord>();

            if (async == null)
            {
                throw new InvalidOperationException();
            }

            return async;
        }

        public async Task<IEnumerable<UserActivityRecord>> GetUserActivityRecordsAsync(int userId)
        {
            var url = $"{_apiBaseUrlUnion}/ByUser/{userId}";
            var response = await _client.GetAsync(url);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new List<UserActivityRecord>();
            }

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<UserActivityRecord>>();
            var async = result;

            if (async != null)
            {
                return async;
            }

            return new List<UserActivityRecord>();
        }
    }
}