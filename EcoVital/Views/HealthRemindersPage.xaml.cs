using EcoVital.ViewModels;

namespace EcoVital.Views;

public partial class HealthRemindersPage
{
    public HealthRemindersPage()
    {
        InitializeComponent();
        BindingContext = new HealthRemindersViewModel();
    }

    void AddReminder(object? sender, EventArgs e)
    {
        ((HealthRemindersViewModel)BindingContext).AddReminder(null);
    }
}