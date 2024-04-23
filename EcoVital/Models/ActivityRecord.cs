using CommunityToolkit.Mvvm.ComponentModel;

namespace EcoVital.Models;

public class ActivityRecord : ObservableObject
{
    public int RecordId { get; set; }
    public int UserId { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public string ActivityType { get; set; }
    public int DurationMinutes { get; set; }
    public string ImageUrl { get; set; }

    private bool _isSelected;

    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }

    public ActivityRecord()
    {
        IsSelected = false;
    }
}