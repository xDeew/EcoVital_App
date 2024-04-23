using System.ComponentModel;
using Microsoft.Maui.Controls;
using System.Windows.Input;
using EcoVital.Views;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace EcoVital.ViewModels
{
    public partial class HomePageViewModel : BaseViewModel
    {
        private string _userName = "usuario predeterminado";

        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }


        public string DailyAdvice { get; set; } // Deberías implementar la lógica para obtener el consejo del día

        public string
            HealthGoals { get; set; } // Deberías implementar la lógica para obtener las metas de salud del usuario

        public ICommand GoToChatbotCommand { get; set; } // Deberías implementar la lógica para navegar al chatbot

        public ICommand
            RegisterActivityCommand { get; set; } // Deberías implementar la lógica para registrar una actividad


        public HomePageViewModel()
        {
            GoToChatbotCommand = new RelayCommand(GoToChatbot);
            RegisterActivityCommand = new RelayCommand(RegisterActivity);
            // Este valor se sobrescribirá cuando el usuario inicie sesión.
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