using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using EcoVital.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoVital.Services
{
    public class UserGoalService
    {
        private readonly HttpClient _client;
        private readonly string _apiBaseUrl = "https://vivaservice.azurewebsites.net/api/UserGoal/";

        public UserGoalService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<UserGoal>> GetUserGoalsAsync()
        {
            return await _client.GetFromJsonAsync<List<UserGoal>>(_apiBaseUrl);
        }

        public async Task<UserGoal> GetUserGoalAsync(int id)
        {
            var url = $"{_apiBaseUrl}{id}";
            return await _client.GetFromJsonAsync<UserGoal>(url);
        }

        public async Task PostUserGoalAsync(int userId, UserGoal userGoal, int activityRecordId)
        {
            userGoal.ActivityRecordId = activityRecordId;
            var url = $"{_apiBaseUrl}{userId}";
            var response = await _client.PostAsJsonAsync(url, userGoal);
            response.EnsureSuccessStatusCode();
            await response.Content.ReadFromJsonAsync<UserGoal>();
        }
        
        public async Task<UserGoal> UpdateGoalAsync(UserGoal userGoal)
        {
            var url = $"{_apiBaseUrl}UpdateGoal";
            var response = await _client.PostAsJsonAsync(url, userGoal);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UserGoal>();
        }

        public async Task<UserGoal> GetUserGoalByActivityIdAsync(int activityId)
        {
            var url = $"{_apiBaseUrl}Activity/{activityId}";
            return await _client.GetFromJsonAsync<UserGoal>(url);
        }
        public async Task DeleteUserGoalAsync(int id)
        {
            var url = $"{_apiBaseUrl}{id}";
            var response = await _client.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
        }
        
    }
}