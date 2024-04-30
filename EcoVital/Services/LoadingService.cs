namespace EcoVital.Services
{
    public class LoadingService : ILoadingService
    {
        private bool isLoadingShown = false;

        public async Task ShowLoading()
        {
            if (isLoadingShown) return;
            isLoadingShown = true;
            await Application.Current.MainPage.Navigation.PushModalAsync(new LoadingPage(), true);
        }

        public async Task HideLoading()
        {
            if (!isLoadingShown) return;
            isLoadingShown = false;
            await Application.Current.MainPage.Navigation.PopModalAsync(true);
        }
    }
}