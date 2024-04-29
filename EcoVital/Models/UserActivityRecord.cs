using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EcoVital.Models;

public class UserActivityRecord : INotifyPropertyChanged
{
    public int UserId { get; set; }
    public int ActivityRecordId { get; set; }
    public string ImageUrl { get; set; }
    public string ActivityType { get; set; }
    private double _progress;

    public double Progress
    {
        get { return _progress; }
        set
        {
            if (_progress != value)
            {
                _progress = value;
                OnPropertyChanged();
            }
        }
    }


    bool _isSelected;

    public bool IsSelected
    {
        get { return _isSelected; }
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