namespace EcoVital.Services
{
    public class LoadingService : ILoadingService
    {
        bool _isLoadingShown;

        public async Task ShowLoading()
        {
            if (_isLoadingShown) return;
            _isLoadingShown = true;
            await Application.Current.MainPage.Navigation.PushModalAsync(new LoadingPage(), true);
        }

        public async Task HideLoading()
        {
            if (!_isLoadingShown) return;
            _isLoadingShown = false;
            await Application.Current.MainPage.Navigation.PopModalAsync(true);
        }
    }
}