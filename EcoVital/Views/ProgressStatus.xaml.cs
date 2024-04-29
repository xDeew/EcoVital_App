using System.Diagnostics;
using EcoVital.Services;
using EcoVital.ViewModels;

namespace EcoVital.Views
{
    public partial class ProgressStatus : ContentPage
    {
        private ProgressStatusViewModel _viewModel;

        public ProgressStatus()
        {
            InitializeComponent();
            InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            // Se obtiene la instancia de ActivityService a través de DependencyService
            // para poder inyectarla en el ViewModel
            // primero pantalla de carga

            ActivityService activityService = DependencyService.Get<ActivityService>();
            if (activityService == null)
            {
                // inicializar el servicio si no se ha registrado
                activityService = new ActivityService(new HttpClient());
            }

            _viewModel = new ProgressStatusViewModel(activityService);
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            
            await LoadingService.ShowLoading();

            await _viewModel.LoadRegisteredActivities(App.UserInfo.UserId);
            
            await Task.Delay(500);
            
            await LoadingService.HideLoading();
        }
    }
}