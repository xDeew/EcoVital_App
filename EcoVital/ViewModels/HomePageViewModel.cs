using System.Windows.Input;
using EcoVital.Views;
using Microsoft.Toolkit.Mvvm.Input;

namespace EcoVital.ViewModels
{
    public partial class HomePageViewModel : BaseViewModel
    {
        string _userName = "usuario predeterminado";

        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }


        public string DailyAdvice { get; set; } // Deberías implementar la lógica para obtener el consejo del día

        public string
            HealthGoals { get; set; } // Deberías implementar la lógica para obtener las metas de salud del usuario

        public ICommand GoToChatbotCommand { get; set; }

        public ICommand
            RegisterActivityCommand { get; set; }


        public HomePageViewModel()
        {
            GoToChatbotCommand = new RelayCommand(GoToChatbot);
            RegisterActivityCommand = new RelayCommand(RegisterActivity);
        }

        private void GoToChatbot()
        {
            Shell.Current.GoToAsync(nameof(ChatBotPage));
        }

        private void RegisterActivity()
        {
            Shell.Current.GoToAsync(nameof(ActivityRecord));
        }

        private void GoBack()
        {
            Shell.Current.GoToAsync("..");
        }
    }
}