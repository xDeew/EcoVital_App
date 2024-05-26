using EcoVital.Models;

namespace EcoVital.Services;

/// <summary>
/// Define una interfaz para las operaciones de inicio de sesión y registro de usuarios.
/// </summary>
public interface ILoginRepository
{
    /// <summary>
    /// Inicia sesión con el nombre de usuario o correo electrónico y la contraseña proporcionados.
    /// </summary>
    /// <param name="usernameOrEmail">El nombre de usuario o correo electrónico del usuario.</param>
    /// <param name="password">La contraseña del usuario.</param>
    /// <returns>Una tarea que representa la operación asincrónica y contiene la información del usuario.</returns>
    Task<UserInfo> Login(string usernameOrEmail, string password);

    /// <summary>
    /// Registra un nuevo usuario con el correo electrónico, nombre de usuario y contraseña proporcionados.
    /// </summary>
    /// <param name="email">El correo electrónico del usuario.</param>
    /// <param name="username">El nombre de usuario.</param>
    /// <param name="password">La contraseña del usuario.</param>
    /// <returns>Una tarea que representa la operación asincrónica y contiene la información del usuario registrado.</returns>
    Task<UserInfo> Register(string email, string username, string password);

    /// <summary>
    /// Envía una pregunta de seguridad.
    /// </summary>
    /// <param name="securityQuestion">La pregunta de seguridad a enviar.</param>
    /// <returns>Una tarea que representa la operación asincrónica y contiene el resultado de la solicitud HTTP.</returns>
    Task<HttpResponseMessage> SendSecurityQuestion(object securityQuestion);

    /// <summary>
    /// Verifica si un usuario existe por su nombre de usuario o correo electrónico.
    /// </summary>
    /// <param name="usernameOrEmail">El nombre de usuario o correo electrónico del usuario.</param>
    /// <returns>Una tarea que representa la operación asincrónica y contiene un valor que indica si el usuario existe.</returns>
    Task<bool> UserExists(string usernameOrEmail);

    /// <summary>
    /// Obtiene la información del usuario por su correo electrónico.
    /// </summary>
    /// <param name="email">El correo electrónico del usuario.</param>
    /// <returns>Una tarea que representa la operación asincrónica y contiene la información del usuario.</returns>
    Task<UserInfo> GetUserByEmail(string email);

    /// <summary>
    /// Obtiene la pregunta de seguridad por el identificador del usuario.
    /// </summary>
    /// <param name="userId">El identificador del usuario.</param>
    /// <returns>Una tarea que representa la operación asincrónica y contiene la pregunta de seguridad.</returns>
    Task<string> GetSecurityQuestionByUserId(int userId);

    /// <summary>
    /// Obtiene la pregunta de seguridad por la pregunta y el identificador del usuario.
    /// </summary>
    /// <param name="question">La pregunta de seguridad.</param>
    /// <param name="userId">El identificador del usuario.</param>
    /// <returns>Una tarea que representa la operación asincrónica y contiene la pregunta de seguridad.</returns>
    Task<SecurityQuestion> GetSecurityQuestionByQuestion(string question, int userId);

    /// <summary>
    /// Cambia la contraseña del usuario.
    /// </summary>
    /// <param name="userId">El identificador del usuario.</param>
    /// <param name="newPassword">La nueva contraseña del usuario.</param>
    /// <returns>Una tarea que representa la operación asincrónica y contiene un valor que indica si el cambio de contraseña fue exitoso.</returns>
    Task<bool> ChangePassword(int userId, string newPassword);
}