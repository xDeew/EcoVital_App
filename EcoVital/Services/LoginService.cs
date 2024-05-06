using System.Net;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using EcoVital.Models;
using Newtonsoft.Json;
using NetworkAccess = Microsoft.Maui.Networking.NetworkAccess;


namespace EcoVital.Services;

public class LoginService : ILoginRepository
{
    const string ApiBaseUrl = "https://vivaservice.azurewebsites.net/api/UserInfoes/";

    public async Task<UserInfo> Login(string usernameOrEmail, string password)
    {
        usernameOrEmail = usernameOrEmail.ToLower();
        try
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                throw new Exception("Sin conexión a internet");

            var client = new HttpClient();
            client.BaseAddress = new Uri(ApiBaseUrl);


            var response = await client.GetAsync($"GetUserByEmailOrUsername/{usernameOrEmail}");
            Console.WriteLine(usernameOrEmail);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Server retornó código de estado: " + response.StatusCode);

            Console.WriteLine(usernameOrEmail);
            var users = await response.Content.ReadFromJsonAsync<List<UserInfo>>();
            UserInfo userInfo = users?.FirstOrDefault();

            if (userInfo == null) return null!;
            using (SHA256 sha256Hash = SHA256.Create())
            {
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder();
                foreach (var t in bytes)
                {
                    builder.Append(t.ToString("x2"));
                }

                password = builder.ToString();
            }


            if (userInfo.Password == password)
            {
                return
                    await Task.FromResult(
                        userInfo);
            }

            return null!;
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                throw ex.InnerException;
            }

            throw ex;
        }
    }

    public async Task<UserInfo> Register(string email, string username, string password)
    {
        email = email.ToLower();
        username = username.ToLower();

        try
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                throw new Exception("Sin conexión a internet");

            var client = new HttpClient();
            var url = "https://vivaservice.azurewebsites.net/api/UserInfoes/";
            client.BaseAddress = new Uri(url);


            using (SHA256 sha256Hash = SHA256.Create())
            {
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder();
                foreach (var t in bytes)
                {
                    builder.Append(t.ToString("x2"));
                }

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
                if (userInfo != null)
                {
                    return await Task.FromResult(userInfo);
                }

                return null!;
            }

            throw new Exception("Server retornó código de estado: " + response.StatusCode);
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                throw ex.InnerException;
            }

            throw ex;
        }
    }

    public async Task<HttpResponseMessage> SendSecurityQuestion(object securityQuestion)
    {
        var client = new HttpClient();
        var url = "https://vivaservice.azurewebsites.net/api/SecurityQuestions/";
        client.BaseAddress = new Uri(url);

        var json = JsonConvert.SerializeObject(securityQuestion);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("", data);

        return response;
    }

    public async Task<bool> UserExists(string userNameOrEmail)
    {
        userNameOrEmail = userNameOrEmail.ToLower();

        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            throw new Exception("No internet connection");

        var client = new HttpClient();
            
        client.BaseAddress = new Uri(ApiBaseUrl);

        HttpResponseMessage response;
        try
        {
            response = await client
                .GetAsync("");
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

    public async Task<string> GetSecurityQuestionByUserId(int userId)
    {
        var client = new HttpClient();
        var response =
            await client.GetAsync(
                "https://vivaservice.azurewebsites.net/api/SecurityQuestions/GetSecurityQuestionByUserId/" +
                userId);

        if (response.IsSuccessStatusCode)
        {
            var securityQuestion =
                await response.Content.ReadAsStringAsync();

            return securityQuestion;
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null!;
        }

        throw new Exception($"Error: {response.StatusCode}");
    }

    public async Task<SecurityQuestion> GetSecurityQuestionByQuestion(string question, int userId)
    {
        var client = new HttpClient();
        var response =
            await client.GetAsync(
                $"https://vivaservice.azurewebsites.net/api/SecurityQuestions/GetSecurityQuestionByQuestion/{Uri.EscapeDataString(question)}/{userId}");


        if (response.IsSuccessStatusCode)
        {
            var securityQuestion = await response.Content.ReadFromJsonAsync<SecurityQuestion>();

            return securityQuestion!;
        }

        throw new Exception($"Error: {response.StatusCode}");
    }

    public async Task<bool> ChangePassword(int userId, string newPassword)
    {
        var client = new HttpClient();
        var url = ApiBaseUrl + userId + "/ChangePassword";
        client.BaseAddress = new Uri(url);


        using (var sha256 = SHA256.Create())
        {
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(newPassword));
            var builder = new StringBuilder();
            foreach (var t in bytes)
            {
                builder.Append(t.ToString("x2"));
            }

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

        if (response.IsSuccessStatusCode)
        {
            return true;
        }

        throw new Exception("Server returned status code: " + response.StatusCode);
    }
}