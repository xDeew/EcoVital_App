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
        readonly string _apiBaseUrl = "https://vivaservice.azurewebsites.net/api/UserInfoes/";

        public async Task<UserInfo> Login(string usernameOrEmail, string password)
        {
            usernameOrEmail = usernameOrEmail.ToLower();
            try
            {
                if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                {
                    var client = new HttpClient();
                    client.BaseAddress = new Uri(_apiBaseUrl);


                    var response = await client.GetAsync($"GetUserByEmailOrUsername/{usernameOrEmail}");
                    Console.WriteLine(usernameOrEmail);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine(usernameOrEmail);
                        var users = await response.Content.ReadFromJsonAsync<List<UserInfo>>();
                        UserInfo userInfo = users?.FirstOrDefault();
                        if (userInfo != null)
                        {
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


                            if (userInfo.Password == password)
                            {
                                return
                                    await Task.FromResult(
                                        userInfo);
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


                    var content = new StringContent(JsonConvert.SerializeObject(new { email, username, password }),
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
                    response = await client
                        .GetAsync("");
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
                    await response.Content.ReadAsStringAsync();

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
            else
            {
                throw new Exception("Server returned status code: " + response.StatusCode);
            }
        }
    }
}