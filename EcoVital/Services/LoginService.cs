using System.Net;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using EcoVital.Models;
using Newtonsoft.Json;
using NetworkAccess = Microsoft.Maui.Networking.NetworkAccess;

namespace EcoVital.Services;

/// <summary>
/// Proporciona servicios relacionados con el inicio de sesión y el registro de usuarios.
/// </summary>
public class LoginService : ILoginRepository
{
    const string ApiBaseUrl = "https://vivaserviceapi.azurewebsites.net/api/UserInfoes/";

    /// <summary>
    /// Inicia sesión con el nombre de usuario o correo electrónico y la contraseña proporcionados.
    /// </summary>
    /// <param name="usernameOrEmail">El nombre de usuario o correo electrónico del usuario.</param>
    /// <param name="password">La contraseña del usuario.</param>
    /// <returns>Una tarea que representa la operación asincrónica y contiene la información del usuario.</returns>
    public async Task<UserInfo> Login(string usernameOrEmail, string password)
    {
        usernameOrEmail = usernameOrEmail.ToLower();
        try
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                throw new Exception("Sin conexión a internet");

            var client = new HttpClient { BaseAddress = new Uri(ApiBaseUrl) };

            var response = await client.GetAsync($"GetUserByEmailOrUsername/{usernameOrEmail}");
            Console.WriteLine(usernameOrEmail);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Server retornó código de estado: " + response.StatusCode);

            var users = await response.Content.ReadFromJsonAsync<List<UserInfo>>();
            var userInfo = users?.FirstOrDefault();

            if (userInfo == null) return null!;
            using (var sha256Hash = SHA256.Create())
            {
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder();
                foreach (var t in bytes) builder.Append(t.ToString("x2"));

                password = builder.ToString();
            }

            if (userInfo.Password == password)
                return await Task.FromResult(userInfo);

            return null!;
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null) throw ex.InnerException;

            throw;
        }
    }

    /// <summary>
    /// Registra un nuevo usuario con el correo electrónico, nombre de usuario y contraseña proporcionados.
    /// </summary>
    /// <param name="email">El correo electrónico del usuario.</param>
    /// <param name="username">El nombre de usuario.</param>
    /// <param name="password">La contraseña del usuario.</param>
    /// <returns>Una tarea que representa la operación asincrónica y contiene la información del usuario registrado.</returns>
    public async Task<UserInfo> Register(string email, string username, string password)
    {
        email = email.ToLower();
        username = username.ToLower();

        try
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                throw new Exception("Sin conexión a internet");

            var client = new HttpClient { BaseAddress = new Uri(ApiBaseUrl) };

            using (var sha256Hash = SHA256.Create())
            {
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder();
                foreach (var t in bytes) builder.Append(t.ToString("x2"));

                password = builder.ToString();
            }

            var content = new StringContent(JsonConvert.SerializeObject(new { email, username, password }),
                Encoding.UTF8, "application/json");

            HttpResponseMessage response;
            try
            {
                response = await client.PostAsync("", content);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al conectar al servidor: " + ex.Message);
            }

            if (response.IsSuccessStatusCode)
            {
                var userInfo = await response.Content.ReadFromJsonAsync<UserInfo>();

                if (userInfo != null) return await Task.FromResult(userInfo);

                return null!;
            }

            throw new Exception("Server retornó código de estado: " + response.StatusCode);
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null) throw ex.InnerException;

            throw;
        }
    }

    /// <summary>
    /// Envía una pregunta de seguridad.
    /// </summary>
    /// <param name="securityQuestion">La pregunta de seguridad a enviar.</param>
    /// <returns>Una tarea que representa la operación asincrónica y contiene el resultado de la solicitud HTTP.</returns>
    public async Task<HttpResponseMessage> SendSecurityQuestion(object securityQuestion)
    {
        var client = new HttpClient
            { BaseAddress = new Uri("https://vivaserviceapi.azurewebsites.net/api/SecurityQuestions/") };

        var json = JsonConvert.SerializeObject(securityQuestion);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("", data);

        return response;
    }

    /// <summary>
    /// Verifica si un usuario existe por su nombre de usuario o correo electrónico.
    /// </summary>
    /// <param name="userNameOrEmail">El nombre de usuario o correo electrónico del usuario.</param>
    /// <returns>Una tarea que representa la operación asincrónica y contiene un valor que indica si el usuario existe.</returns>
    public async Task<bool> UserExists(string userNameOrEmail)
    {
        userNameOrEmail = userNameOrEmail.ToLower();

        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            throw new Exception("No internet connection");

        var client = new HttpClient { BaseAddress = new Uri(ApiBaseUrl) };

        HttpResponseMessage response;
        try
        {
            response = await client.GetAsync("");
        }
        catch (Exception ex)
        {
            throw new Exception("Error occurred while connecting to the server: " + ex.Message);
        }

        if (!response.IsSuccessStatusCode)
            throw new Exception("Server returned status code: " + response.StatusCode);

        var users = await response.Content.ReadFromJsonAsync<List<UserInfo>>();

        return users != null && users.Any(user => user.UserName == userNameOrEmail || user.Email == userNameOrEmail);
    }

    /// <summary>
    /// Obtiene la información del usuario por su correo electrónico.
    /// </summary>
    /// <param name="email">El correo electrónico del usuario.</param>
    /// <returns>Una tarea que representa la operación asincrónica y contiene la información del usuario.</returns>
    public async Task<UserInfo> GetUserByEmail(string email)
    {
        var client = new HttpClient();
        var response = await client.GetAsync($"{ApiBaseUrl}GetUserByEmail/{email}");

        if (response.IsSuccessStatusCode)
        {
            var userInfoList = await response.Content.ReadFromJsonAsync<List<UserInfo>>();

            return userInfoList.FirstOrDefault();
        }

        await Shell.Current.DisplayAlert("Error", $"{response.StatusCode}", "OK");

        throw new Exception($"Error: {response.StatusCode}");
    }

    /// <summary>
    /// Obtiene la pregunta de seguridad por el identificador del usuario.
    /// </summary>
    /// <param name="userId">El identificador del usuario.</param>
    /// <returns>Una tarea que representa la operación asincrónica y contiene la pregunta de seguridad.</returns>
    public async Task<string> GetSecurityQuestionByUserId(int userId)
    {
        var client = new HttpClient();
        var response =
            await client.GetAsync(
                $"https://vivaserviceapi.azurewebsites.net/api/SecurityQuestions/GetSecurityQuestionByUserId/{userId}");

        if (response.IsSuccessStatusCode)
        {
            var securityQuestion = await response.Content.ReadAsStringAsync();

            return securityQuestion;
        }

        if (response.StatusCode == HttpStatusCode.NotFound) return null!;

        throw new Exception($"Error: {response.StatusCode}");
    }

    /// <summary>
    /// Obtiene la pregunta de seguridad por la pregunta y el identificador del usuario.
    /// </summary>
    /// <param name="question">La pregunta de seguridad.</param>
    /// <param name="userId">El identificador del usuario.</param>
    /// <returns>Una tarea que representa la operación asincrónica y contiene la pregunta de seguridad.</returns>
    public async Task<SecurityQuestion> GetSecurityQuestionByQuestion(string question, int userId)
    {
        var client = new HttpClient();
        var response =
            await client.GetAsync(
                $"https://vivaserviceapi.azurewebsites.net/api/SecurityQuestions/GetSecurityQuestionByQuestion/{Uri.EscapeDataString(question)}/{userId}");

        if (response.IsSuccessStatusCode)
        {
            var securityQuestion = await response.Content.ReadFromJsonAsync<SecurityQuestion>();

            return securityQuestion!;
        }

        throw new Exception($"Error: {response.StatusCode}");
    }

    /// <summary>
    /// Cambia la contraseña del usuario.
    /// </summary>
    /// <param name="userId">El identificador del usuario.</param>
    /// <param name="newPassword">La nueva contraseña del usuario.</param>
    /// <returns>Una tarea que representa la operación asincrónica y contiene un valor que indica si el cambio de contraseña fue exitoso.</returns>
    public async Task<bool> ChangePassword(int userId, string newPassword)
    {
        var client = new HttpClient();
        var url = $"{ApiBaseUrl}{userId}/ChangePassword";
        client.BaseAddress = new Uri(url);

        using (var sha256 = SHA256.Create())
        {
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(newPassword));
            var builder = new StringBuilder();
            foreach (var t in bytes) builder.Append(t.ToString("x2"));

            newPassword = builder.ToString();
        }

        var content =
            new StringContent(JsonConvert.SerializeObject(new ChangePasswordRequest { NewPassword = newPassword }),
                Encoding.UTF8, "application/json");

        HttpResponseMessage response;
        try
        {
            response = await client.PostAsync("", content);
        }
        catch (Exception ex)
        {
            throw new Exception("Error occurred while connecting to the server: " + ex.Message);
        }

        if (response.IsSuccessStatusCode) return true;

        throw new Exception("Server returned status code: " + response.StatusCode);
    }
}