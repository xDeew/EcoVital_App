namespace EcoVital.Models;

/// <summary>
/// Representa un mensaje en el chatbot.
/// </summary>
public class Message
{
    /// <summary>
    /// Obtiene o establece el texto del mensaje.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Indica si el mensaje es del usuario.
    /// </summary>
    public bool IsUserMessage { get; set; }
}