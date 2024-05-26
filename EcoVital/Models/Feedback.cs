using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EcoVital.Models;

/// <summary>
/// Representa un feedback proporcionado por un usuario.
/// </summary>
public sealed class Feedback : INotifyPropertyChanged
{
    string _email = string.Empty;
    int _feedbackId;
    string _message = string.Empty;
    string _type;

    /// <summary>
    /// Obtiene o establece el identificador del feedback.
    /// </summary>
    public int FeedbackId
    {
        get => _feedbackId;
        set
        {
            _feedbackId = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Obtiene o establece el correo electrónico del usuario que proporciona el feedback.
    /// </summary>
    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Obtiene o establece el tipo de feedback.
    /// </summary>
    public string Type
    {
        get => _type;
        set
        {
            _type = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Obtiene o establece el mensaje del feedback.
    /// </summary>
    public string Message
    {
        get => _message;
        set
        {
            _message = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Evento que se produce cuando cambia el valor de una propiedad.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Genera el evento <see cref="PropertyChanged"/> para notificar un cambio en una propiedad.
    /// </summary>
    /// <param name="propertyName">Nombre de la propiedad que cambió.</param>
    void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}