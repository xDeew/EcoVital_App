using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json; 
using System.Threading.Tasks;
using EcoVital.Models;

namespace EcoVital.Services
{
    public class ActivityService
    {
        private readonly HttpClient _client;
        private readonly string _apiBaseUrl = "https://vivaservice.azurewebsites.net/api/activity";
        private readonly string _apiBaseUrlUnion = "https://vivaservice.azurewebsites.net/api/UserActivityRecords";

        public ActivityService(HttpClient client) 
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<List<ActivityRecord>> GetActivityRecordsAsync()
        {
            return await _client.GetFromJsonAsync<List<ActivityRecord>>(_apiBaseUrl);
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
                // Retorna una lista vacía si el usuario no tiene registros, evitando el error 404.
                return new List<UserActivityRecord>();
            }

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<UserActivityRecord>>();
            var async = result;

            if (async != null)
            {
                return async;
            }

            return new List<UserActivityRecord>(); // Retorna una lista vacía si no hay registros.
        }
    }
}