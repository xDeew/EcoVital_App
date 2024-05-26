using EcoVital.Services;
using EcoVital.ViewModels;

namespace EcoVital.Views;

/// <summary>
/// Página para registrar actividades.
/// </summary>
public partial class ActivityRecord
{
    readonly ActivityRecordViewModel _viewModel;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ActivityRecord"/>.
    /// </summary>
    public ActivityRecord()
    {
        InitializeComponent();
        var client = new HttpClient();
        _viewModel = new ActivityRecordViewModel(new ActivityService(client), new UserGoalService(client));
        BindingContext = _viewModel;
    }

    /// <summary>
    /// Maneja el evento de aparición de la página.
    /// </summary>
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadActivitiesCommand.ExecuteAsync(null);
    }

    /// <summary>
    /// Maneja el evento de clic del botón de información.
    /// </summary>
    /// <param name="sender">El origen del evento.</param>
    /// <param name="e">Los datos del evento.</param>
    async void OnInfoButtonClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Información",
            "Para seleccionar una actividad, toca la imagen de la actividad correspondiente.", "OK");
    }
}