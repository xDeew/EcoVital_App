using EcoVital.ViewModels;

namespace EcoVital.Models;

public class HealthReminder : BaseViewModel
{
    public string ReminderType { get; set; }
    public string ReminderTime { get; set; }
    public string ReminderMessage { get; set; }
    public string ImageSource { get; set; }
}