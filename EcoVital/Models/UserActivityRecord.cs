using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EcoVital.Models;

/// <summary>
/// Representa un registro de actividad del usuario.
/// </summary>
public class UserActivityRecord : INotifyPropertyChanged
{
    bool _isSelected;
    double _progress;

    /// <summary>
    /// Obtiene o establece el identificador de la actividad del usuario.
    /// </summary>
    public int UserActivityId { get; set; }

    /// <summary>
    /// Obtiene o establece el identificador del usuario.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Obtiene o establece el identificador del registro de actividad.
    /// </summary>
    public int ActivityRecordId { get; set; }

    /// <summary>
    /// Obtiene o establece la URL de la imagen asociada con la actividad.
    /// </summary>
    public string ImageUrl { get; set; }

    /// <summary>
    /// Obtiene o establece el tipo de actividad.
    /// </summary>
    public string ActivityType { get; set; }

    /// <summary>
    /// Obtiene o establece el progreso de la actividad.
    /// </summary>
    public double Progress
    {
        get => _progress;
        set
        {
            if (_progress != value)
            {
                _progress = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Obtiene o establece un valor que indica si el registro está seleccionado.
    /// </summary>
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Se produce cuando cambia el valor de una propiedad.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Genera el evento <see cref="PropertyChanged"/> para notificar un cambio en una propiedad.
    /// </summary>
    /// <param name="propertyName">Nombre de la propiedad que cambió.</param>
    public virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}