using System.Diagnostics;
using EcoVital.ViewModels;

namespace EcoVital.Views;

/// <summary>
/// Página para administrar recordatorios de salud.
/// </summary>
public partial class HealthRemindersPage : ContentPage
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="HealthRemindersPage"/>.
    /// </summary>
    public HealthRemindersPage()
    {
        InitializeComponent();
        BindingContext = new HealthRemindersViewModel();
    }

    /// <summary>
    /// Maneja el evento de agregar un nuevo recordatorio.
    /// </summary>
    /// <param name="sender">El origen del evento.</param>
    /// <param name="e">Los datos del evento.</param>
    void AddReminder(object? sender, EventArgs e)
    {
        ((HealthRemindersViewModel)BindingContext).AddReminder(null);
    }

    /// <summary>
    /// Método llamado cuando la página aparece.
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();

        var viewModel = BindingContext as HealthRemindersViewModel;

        if (viewModel == null) Debug.WriteLine("El ViewModel es nulo");
    }
}