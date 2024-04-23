using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using EcoVital.Views; 

namespace EcoVital.Services
{
    public static class LoadingService
    {
        private static bool IsLoadingShown;

        public static async Task ShowLoading()
        {
            if (IsLoadingShown) return;

            IsLoadingShown = true;
            await Application.Current.MainPage.Navigation.PushModalAsync(new LoadingPage(), true);
        }

        public static async Task HideLoading()
        {
            if (!IsLoadingShown) return;

            await Application.Current.MainPage.Navigation.PopModalAsync(true);
            IsLoadingShown = false;
        }
    }
}