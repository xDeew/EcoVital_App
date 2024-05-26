namespace EcoVital.Models;

/// <summary>
/// Representa una meta del usuario.
/// </summary>
public class UserGoal
{
    /// <summary>
    /// Obtiene o establece el identificador de la meta.
    /// </summary>
    public int GoalId { get; set; }

    /// <summary>
    /// Obtiene o establece la fecha objetivo para alcanzar la meta.
    /// </summary>
    public DateTime TargetDate { get; set; }

    /// <summary>
    /// Obtiene o establece un valor que indica si la meta ha sido alcanzada.
    /// </summary>
    public bool IsAchieved { get; set; }

    /// <summary>
    /// Obtiene o establece el identificador del usuario asociado a la meta.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Obtiene o establece el identificador del registro de actividad asociado a la meta.
    /// </summary>
    public int ActivityRecordId { get; set; }
}