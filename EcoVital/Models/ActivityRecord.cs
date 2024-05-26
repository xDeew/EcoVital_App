using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace EcoVital.Models;

/// <summary>
/// Representa un registro de actividad en la aplicación EcoVital.
/// </summary>
public class ActivityRecord : ObservableObject
{
    private bool _isSelected;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ActivityRecord"/>.
    /// </summary>
    public ActivityRecord()
    {
        IsSelected = false;
    }

    /// <summary>
    /// Obtiene o establece el identificador del registro.
    /// </summary>
    /// <value>El identificador del registro.</value>
    public int RecordId { get; set; }

    /// <summary>
    /// Obtiene o establece la descripción de la actividad.
    /// </summary>
    /// <value>La descripción de la actividad.</value>
    public string Description { get; set; }

    /// <summary>
    /// Obtiene o establece la fecha de la actividad.
    /// </summary>
    /// <value>La fecha de la actividad.</value>
    public DateTime Date { get; set; }

    /// <summary>
    /// Obtiene o establece el tipo de actividad.
    /// </summary>
    /// <value>El tipo de actividad.</value>
    public string ActivityType { get; set; }

    /// <summary>
    /// Obtiene o establece la URL de la imagen asociada con la actividad.
    /// </summary>
    /// <value>La URL de la imagen asociada con la actividad.</value>
    public string ImageUrl { get; set; }

    /// <summary>
    /// Obtiene o establece un valor que indica si el registro está seleccionado.
    /// </summary>
    /// <value><c>true</c> si el registro está seleccionado; en caso contrario, <c>false</c>.</value>
    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }
}