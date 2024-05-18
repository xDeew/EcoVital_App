using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EcoVital.Models;

public class UserActivityRecord : INotifyPropertyChanged
{
    bool _isSelected;
    double _progress;
    public int UserActivityId { get; set; }
    public int UserId { get; set; }
    public int ActivityRecordId { get; set; }
    public string ImageUrl { get; set; }
    public string ActivityType { get; set; }

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


    public event PropertyChangedEventHandler? PropertyChanged;

    public virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}