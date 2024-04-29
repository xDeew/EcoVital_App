namespace EcoVital.Services
{
    public static class LoadingService
    {
        static bool IsLoadingShown;

        public static async Task ShowLoading()
        {
            if (IsLoadingShown) return;

            IsLoadingShown = true;
            await Application.Current.MainPage.Navigation.PushModalAsync(new LoadingPage(), true);
        }

        public static async Task HideLoading()
        {
            if (!IsLoadingShown) return;

            var mainPage = Application.Current.MainPage;
            if (mainPage != null && mainPage.Navigation.ModalStack.Count > 0)
            {
                await mainPage.Navigation.PopModalAsync(true);
                IsLoadingShown = false;
            }
        }
    }
}