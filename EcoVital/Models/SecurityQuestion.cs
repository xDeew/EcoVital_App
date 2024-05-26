namespace EcoVital.Models;

/// <summary>
/// Representa una pregunta de seguridad.
/// </summary>
public class SecurityQuestion
{
    /// <summary>
    /// Obtiene o establece el identificador de la pregunta de seguridad.
    /// </summary>
    public int SecurityQuestionId { get; set; }

    /// <summary>
    /// Obtiene o establece el texto de la pregunta de seguridad.
    /// </summary>
    public string QuestionText { get; set; }

    /// <summary>
    /// Obtiene o establece la respuesta a la pregunta de seguridad.
    /// </summary>
    public string Answer { get; set; }

    /// <summary>
    /// Obtiene o establece el identificador del usuario asociado a la pregunta de seguridad.
    /// </summary>
    public int UserId { get; set; }
}