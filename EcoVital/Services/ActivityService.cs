using System.Net;
using System.Net.Http.Json;
using EcoVital.Models;

namespace EcoVital.Services;

/// <summary>
/// Proporciona servicios relacionados con actividades y registros de actividad de usuarios.
/// </summary>
public class ActivityService
{
    readonly string _apiBaseUrl = "https://vivaserviceapi.azurewebsites.net/api/activity";
    readonly string _apiBaseUrlUnion = "https://vivaserviceapi.azurewebsites.net/api/UserActivityRecords";
    readonly HttpClient _client;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ActivityService"/>.
    /// </summary>
    /// <param name="client">Instancia de <see cref="HttpClient"/> para realizar solicitudes HTTP.</param>
    /// <exception cref="ArgumentNullException">Lanzada si el parámetro <paramref name="client"/> es <c>null</c>.</exception>
    public ActivityService(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    /// <summary>
    /// Obtiene una lista de registros de actividad.
    /// </summary>
    /// <returns>Una lista de <see cref="ActivityRecord"/>.</returns>
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

    /// <summary>
    /// Registra un nuevo registro de actividad del usuario.
    /// </summary>
    /// <param name="userActivityRecord">El registro de actividad del usuario a registrar.</param>
    /// <returns>El registro de actividad del usuario registrado.</returns>
    /// <exception cref="InvalidOperationException">Lanzada si no se puede leer el contenido de la respuesta.</exception>
    public async Task<UserActivityRecord> RegisterUserActivityRecordAsync(UserActivityRecord userActivityRecord)
    {
        var response = await _client.PostAsJsonAsync(_apiBaseUrlUnion, userActivityRecord);
        response.EnsureSuccessStatusCode();

        var async = await response.Content.ReadFromJsonAsync<UserActivityRecord>();

        if (async == null) throw new InvalidOperationException();

        return async;
    }

    /// <summary>
    /// Obtiene los registros de actividad de un usuario específico.
    /// </summary>
    /// <param name="userId">El identificador del usuario.</param>
    /// <returns>Una colección de <see cref="UserActivityRecord"/>.</returns>
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

    /// <summary>
    /// Elimina un registro de actividad del usuario.
    /// </summary>
    /// <param name="recordId">El identificador del registro de actividad a eliminar.</param>
    public async Task DeleteUserActivityRecordAsync(int recordId)
    {
        var url = $"{_apiBaseUrlUnion}/{recordId}";
        var response = await _client.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
    }
}