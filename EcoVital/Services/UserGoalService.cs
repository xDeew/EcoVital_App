using System.Net.Http.Json;
using EcoVital.Models;

namespace EcoVital.Services;

/// <summary>
/// Proporciona servicios para gestionar las metas de los usuarios relacionadas con actividades.
/// </summary>
public class UserGoalService
{
    readonly string _apiBaseUrl = "https://vivaserviceapi.azurewebsites.net/api/UserGoal/";
    readonly HttpClient _client;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="UserGoalService"/>.
    /// </summary>
    /// <param name="client">Instancia de <see cref="HttpClient"/> para realizar solicitudes HTTP.</param>
    public UserGoalService(HttpClient client)
    {
        _client = client;
    }

    /// <summary>
    /// Obtiene todas las metas de los usuarios.
    /// </summary>
    /// <returns>Una lista de <see cref="UserGoal"/>.</returns>
    public async Task<List<UserGoal>> GetUserGoalsAsync() =>
        await _client.GetFromJsonAsync<List<UserGoal>>(_apiBaseUrl);

    /// <summary>
    /// Obtiene una meta de usuario por su identificador.
    /// </summary>
    /// <param name="id">El identificador de la meta del usuario.</param>
    /// <returns>La meta de usuario solicitada.</returns>
    public async Task<UserGoal> GetUserGoalAsync(int id)
    {
        var url = $"{_apiBaseUrl}{id}";

        return await _client.GetFromJsonAsync<UserGoal>(url);
    }

    /// <summary>
    /// Publica una nueva meta de usuario.
    /// </summary>
    /// <param name="userId">El identificador del usuario.</param>
    /// <param name="userGoal">La meta del usuario a publicar.</param>
    /// <param name="activityRecordId">El identificador del registro de actividad asociado a la meta.</param>
    public async Task PostUserGoalAsync(int userId, UserGoal userGoal, int activityRecordId)
    {
        userGoal.ActivityRecordId = activityRecordId;
        var url = $"{_apiBaseUrl}{userId}";
        var response = await _client.PostAsJsonAsync(url, userGoal);
        response.EnsureSuccessStatusCode();
        await response.Content.ReadFromJsonAsync<UserGoal>();
    }

    /// <summary>
    /// Actualiza una meta de usuario existente.
    /// </summary>
    /// <param name="userGoal">La meta del usuario a actualizar.</param>
    /// <returns>La meta de usuario actualizada.</returns>
    public async Task<UserGoal> UpdateGoalAsync(UserGoal userGoal)
    {
        var url = $"{_apiBaseUrl}UpdateGoal";
        var response = await _client.PostAsJsonAsync(url, userGoal);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<UserGoal>();
    }

    /// <summary>
    /// Obtiene una meta de usuario por el identificador de la actividad asociada.
    /// </summary>
    /// <param name="activityId">El identificador de la actividad.</param>
    /// <returns>La meta de usuario solicitada.</returns>
    public async Task<UserGoal> GetUserGoalByActivityIdAsync(int activityId)
    {
        var url = $"{_apiBaseUrl}Activity/{activityId}";

        return await _client.GetFromJsonAsync<UserGoal>(url);
    }

    /// <summary>
    /// Elimina una meta de usuario por su identificador.
    /// </summary>
    /// <param name="id">El identificador de la meta del usuario a eliminar.</param>
    public async Task DeleteUserGoalAsync(int id)
    {
        var url = $"{_apiBaseUrl}{id}";
        var response = await _client.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
    }
}