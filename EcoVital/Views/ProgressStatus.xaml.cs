using EcoVital.Services;
using EcoVital.ViewModels;

namespace EcoVital.Views;

/// <summary>
/// Página que muestra el progreso de las actividades del usuario.
/// </summary>
public partial class ProgressStatus : ContentPage
{
    ProgressStatusViewModel _viewModel;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ProgressStatus"/>.
    /// </summary>
    public ProgressStatus()
    {
        InitializeComponent();
        InitializeViewModel();
    }

    /// <summary>
    /// Inicializa el ViewModel de la página.
    /// </summary>
    void InitializeViewModel()
    {
        // Se obtiene la instancia de ActivityService a través de DependencyService
        // para poder inyectarla en el ViewModel

        var activityService = DependencyService.Get<ActivityService>();
        if (activityService == null)
            // Inicializar el servicio si no se ha registrado
            activityService = new ActivityService(new HttpClient());

        _viewModel = new ProgressStatusViewModel(activityService);
        BindingContext = _viewModel;
    }

    /// <summary>
    /// Método llamado cuando la página aparece.
    /// </summary>
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadRegisteredActivities(App.UserInfo.UserId);
    }
}