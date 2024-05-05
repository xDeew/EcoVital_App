using System.Diagnostics;
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

    protected override void OnAppearing()
    {
        base.OnAppearing();


        var viewModel = BindingContext as HealthRemindersViewModel;

        // Verifica si el ViewModel es nulo
        if (viewModel == null)
        {
            Debug.WriteLine("El ViewModel es nulo");
        }
    }
}