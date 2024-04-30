namespace EcoVital.Models;

public class UserInfo
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }

    public int FailedPasswordRecoveryAttempts { get; set; }
}