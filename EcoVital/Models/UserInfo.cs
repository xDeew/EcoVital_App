namespace EcoVital.Models;

/// <summary>
/// Representa la información de un usuario.
/// </summary>
public class UserInfo
{
    /// <summary>
    /// Obtiene o establece el identificador del usuario.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Obtiene o establece el nombre de usuario.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Obtiene o establece la contraseña del usuario.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Obtiene o establece el correo electrónico del usuario.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Obtiene o establece el número de intentos fallidos de recuperación de contraseña.
    /// </summary>
    public int FailedPasswordRecoveryAttempts { get; set; }
}