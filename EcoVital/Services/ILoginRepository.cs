using EcoVital.Models;

namespace EcoVital.Services
{
    public interface ILoginRepository
    {
        Task<UserInfo> Login(string usernameOrEmail, string password);

        Task<UserInfo> Register(string email, string username, string password);

        Task<HttpResponseMessage> SendSecurityQuestion(object securityQuestion);

        Task<bool> UserExists(string usernameOrEmail);

        Task<UserInfo> GetUserByEmail(string email);

        Task<string> GetSecurityQuestionByUserId(int userId);

        Task<SecurityQuestion> GetSecurityQuestionByQuestion(string question, int userId);

        Task<bool> ChangePassword(int userId, string newPassword);
    }
}