using System.Net;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using EcoVital.Models;
using Newtonsoft.Json;
using NetworkAccess = Microsoft.Maui.Networking.NetworkAccess;


namespace EcoVital.Services
{
    public class LoginService : ILoginRepository
    {
        private readonly string _apiBaseUrl = "https://vivaservice.azurewebsites.net/api/UserInfoes/";

        public async Task<UserInfo> Login(string usernameOrEmail, string password)
        {
            usernameOrEmail = usernameOrEmail.ToLower();
            try
            {
                if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                {
                    var client = new HttpClient();
                    client.BaseAddress = new Uri(_apiBaseUrl); // Set the base address of the API to the HttpClient

                    // Send a GET request to 'GetUserByEmailOrUsername/{emailOrUsername}'
                    var response = await client.GetAsync($"GetUserByEmailOrUsername/{usernameOrEmail}");
                    Console.WriteLine(usernameOrEmail);
                    // Then check if the response is successful
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine(usernameOrEmail);
                        var users = await response.Content.ReadFromJsonAsync<List<UserInfo>>();
                        UserInfo userInfo = users?.FirstOrDefault();
                        if (userInfo != null)
                        {
                            // Hasheamos la contraseña para garantizar la seguridad de la información
                            using (SHA256 sha256Hash = SHA256.Create())
                            {
                                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                                StringBuilder builder = new StringBuilder();
                                for (int i = 0; i < bytes.Length; i++)
                                {
                                    builder.Append(bytes[i].ToString("x2"));
                                }

                                password = builder.ToString();
                            }

                            // Verificar si la contraseña coincide con la contraseña almacenada en la base de datos
                            if (userInfo.Password == password)
                            {
                                return
                                    await Task.FromResult(
                                        userInfo); // Devuelve el objeto UserInfo si la contraseña coincide
                            }
                            else
                            {
                                return null;
                            }
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        throw new Exception("Server returned status code: " + response.StatusCode);
                    }
                }
                else
                {
                    throw new Exception("No internet connection");
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    throw ex.InnerException;
                }
                else
                {
                    throw ex;
                }
            }

            return null;
        }

        public async Task<UserInfo> Register(string email, string username, string password)
        {
            email = email.ToLower();
            username = username.ToLower();

            try
            {
                if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                {
                    var client = new HttpClient();
                    string url = "https://vivaservice.azurewebsites.net/api/UserInfoes/";
                    client.BaseAddress = new Uri(url);

                    // Hashear la contraseña antes de enviarla
                    using (SHA256 sha256Hash = SHA256.Create())
                    {
                        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                        StringBuilder builder = new StringBuilder();
                        for (int i = 0; i < bytes.Length; i++)
                        {
                            builder.Append(bytes[i].ToString("x2"));
                        }

                        password = builder.ToString();
                    }

                    // Crear contenido JSON con nombre de usuario, correo electrónico y contraseña
                    var content = new StringContent(JsonConvert.SerializeObject(new { email, username, password }),
                        Encoding.UTF8, "application/json");

                    HttpResponseMessage response;
                    try
                    {
                        // Enviar una solicitud POST
                        response = await client.PostAsync("", content);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error occurred while connecting to the server: " + ex.Message);
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        var userInfo = await response.Content.ReadFromJsonAsync<UserInfo>();
                        if (userInfo != null)
                        {
                            return await Task.FromResult(userInfo);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        throw new Exception("Server returned status code: " + response.StatusCode);
                    }
                }
                else
                {
                    throw new Exception("No internet connection");
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    throw ex.InnerException;
                }
                else
                {
                    throw ex;
                }
            }
        }

        public async Task<HttpResponseMessage> SendSecurityQuestion(object securityQuestion)
        {
            var client = new HttpClient();
            string url = "https://vivaservice.azurewebsites.net/api/SecurityQuestions/";
            client.BaseAddress = new Uri(url);

            var json = JsonConvert.SerializeObject(securityQuestion);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("", data);

            return response;
        }

        public async Task<bool> UserExists(string userNameOrEmail)
        {
            userNameOrEmail = userNameOrEmail.ToLower();

            if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
            {
                var client = new HttpClient();
                string url = "https://vivaservice.azurewebsites.net/api/UserInfoes/";
                client.BaseAddress = new Uri(url);

                HttpResponseMessage response;
                try
                {
                    // Send a GET request
                    response = await client
                        .GetAsync(""); // the requestUri is empty because the base address is already set
                }
                catch (Exception ex)
                {
                    throw new Exception("Error occurred while connecting to the server: " + ex.Message);
                }

                if (response.IsSuccessStatusCode)
                {
                    var users = await response.Content.ReadFromJsonAsync<List<UserInfo>>();
                    if (users != null)
                    {
                        // If the server returns a list of users, search through the list to check if the user exists
                        foreach (var user in users)
                        {
                            if (user.UserName == userNameOrEmail || user.Email == userNameOrEmail)
                            {
                                return true;
                            }
                        }

                        return false;
                    }
                    else
                    {
                        // If the server does not return a list of users, then the user does not exist
                        return false;
                    }
                }
                else
                {
                    throw new Exception("Server returned status code: " + response.StatusCode);
                }
            }
            else
            {
                throw new Exception("No internet connection");
            }
        }

        public async Task<UserInfo> GetUserByEmail(string email)
        {
            var client = new HttpClient();
            var response = await client.GetAsync($"{_apiBaseUrl}GetUserByEmail/{email}");

            if (response.IsSuccessStatusCode)
            {
                var userInfoList = await response.Content.ReadFromJsonAsync<List<UserInfo>>();

                return userInfoList.FirstOrDefault();
            }
            else
            {
                Shell.Current.DisplayAlert("Error", $"{response.StatusCode}", "OK");

                throw new Exception($"Error: {response.StatusCode}");
            }

            return null;
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
                    await response.Content.ReadAsStringAsync(); // Lee la respuesta como una cadena de texto

                return securityQuestion;
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            else
            {
                throw new Exception($"Error: {response.StatusCode}");
            }
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

                return securityQuestion;
            }
            else
            {
                throw new Exception($"Error: {response.StatusCode}");
            }
        }

        public async Task<bool> ChangePassword(int userId, string newPassword)
        {
            var client = new HttpClient();
            string url = _apiBaseUrl + userId + "/ChangePassword";
            client.BaseAddress = new Uri(url);

            // Hashear la nueva contraseña antes de enviarla
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(newPassword));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                newPassword = builder.ToString();
            }

            // Crear contenido JSON con la nueva contraseña
            var content =
                new StringContent(JsonConvert.SerializeObject(new ChangePasswordRequest { NewPassword = newPassword }),
                    Encoding.UTF8, "application/json");

            HttpResponseMessage response;
            try
            {
                // Enviar una solicitud POST
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
            else
            {
                throw new Exception("Server returned status code: " + response.StatusCode);
            }
        }
    }
}