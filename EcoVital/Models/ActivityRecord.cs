using CommunityToolkit.Mvvm.ComponentModel;

namespace EcoVital.Models;

public class ActivityRecord : ObservableObject
{
    bool _isSelected;

    public ActivityRecord()
    {
        IsSelected = false;
    }

    public int RecordId { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public string ActivityType { get; set; }
    public string ImageUrl { get; set; }

    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }
}