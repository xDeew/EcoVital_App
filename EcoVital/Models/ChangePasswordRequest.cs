namespace EcoVital.Models
{
    /// <summary>
    /// Representa una solicitud para cambiar la contraseña de un usuario.
    /// </summary>
    public class ChangePasswordRequest
    {
        /// <summary>
        /// Obtiene o establece la nueva contraseña del usuario.
        /// </summary>
        /// <value>
        /// La nueva contraseña que el usuario desea establecer.
        /// </value>
        public string NewPassword { get; set; }
    }
}