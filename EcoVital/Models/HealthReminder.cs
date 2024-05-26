using EcoVital.ViewModels;

namespace EcoVital.Models;

/// <summary>
/// Representa un recordatorio de salud.
/// </summary>
public class HealthReminder : BaseViewModel
{
    /// <summary>
    /// Obtiene o establece el tipo de recordatorio.
    /// </summary>
    public string ReminderType { get; set; }

    /// <summary>
    /// Obtiene o establece la hora del recordatorio.
    /// </summary>
    public string ReminderTime { get; set; }

    /// <summary>
    /// Obtiene o establece el mensaje del recordatorio.
    /// </summary>
    public string ReminderMessage { get; set; }

    /// <summary>
    /// Obtiene o establece la fuente de la imagen del recordatorio.
    /// </summary>
    public string ImageSource { get; set; }
}