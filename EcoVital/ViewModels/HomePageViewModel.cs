using System.Windows.Input;
using EcoVital.Services;
using EcoVital.Views;
using Microsoft.Toolkit.Mvvm.Input;

namespace EcoVital.ViewModels
{
    public partial class HomePageViewModel : BaseViewModel
    {
        readonly ILoadingService _loadingService = new LoadingService();
        string _userName = "usuario predeterminado";

        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }


        public string DailyAdvice { get; set; }

        public string HealthGoals { get; set; }

        public ICommand GoToChatbotCommand { get; set; }

        public ICommand RegisterActivityCommand { get; set; }


        public HomePageViewModel()
        {
            GoToChatbotCommand = new RelayCommand(GoToChatbot);
            RegisterActivityCommand = new RelayCommand(RegisterActivity);
        }

        void GoToChatbot()
        {
            Shell.Current.GoToAsync(nameof(ChatBotPage));
        }

        async void RegisterActivity()
        {
            await Shell.Current.GoToAsync(nameof(ActivityRecord));
        }
    }
}