using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EcoVital.Models;

public sealed class Feedback : INotifyPropertyChanged
{
    string _email = string.Empty;
    int _feedbackId;
    string _message = string.Empty;
    string _type;

    public int FeedbackId
    {
        get => _feedbackId;
        set
        {
            _feedbackId = value;
            OnPropertyChanged();
        }
    }

    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged();
        }
    }

    public string Type
    {
        get => _type;
        set
        {
            _type = value;
            OnPropertyChanged();
        }
    }

    public string Message
    {
        get => _message;
        set
        {
            _message = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}