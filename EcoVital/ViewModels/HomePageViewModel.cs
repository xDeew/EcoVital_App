using System.Windows.Input;
using EcoVital.Views;
using Microsoft.Toolkit.Mvvm.Input;

namespace EcoVital.ViewModels
{
    public partial class HomePageViewModel : BaseViewModel
    {
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        string _userName = "usuario predeterminado";

        private readonly string[] _dailyAdvices =
        {
            "Mantén una dieta equilibrada y bebe suficiente agua.",
            "Realiza al menos 30 minutos de actividad física hoy.",
            "Tómate un tiempo para meditar y despejar tu mente.",
            "Recuerda estirar antes y después de hacer ejercicio.",
            "Duerme al menos 7 horas para un descanso óptimo.",
            "Prueba algo nuevo hoy, ¡sal de tu zona de confort!",
            "Dedica tiempo a conectarte con amigos o familia."
        };

        public string DailyAdvice { get; private set; }

        public ICommand GoToChatbotCommand { get; private set; }
        public ICommand RegisterActivityCommand { get; private set; }
        public ICommand GoToHealthRemindersPageCommand { get; private set; }
        public ICommand GoToProgressPageCommand { get; private set; }

        public HomePageViewModel()
        {
            GoToChatbotCommand = new RelayCommand(GoToChatbot);
            RegisterActivityCommand = new RelayCommand(RegisterActivity);
            GoToHealthRemindersPageCommand = new RelayCommand(() => GoToPage(nameof(HealthRemindersPage)));
            GoToProgressPageCommand = new RelayCommand(() => GoToPage("//ProgressStatus"));
            DailyAdvice = GetDailyAdvice();
        }
        public HomePageViewModel(DateTime? specificDay = null)
        {
            int dayIndex = (int)(specificDay ?? DateTime.Now).DayOfWeek;
            DailyAdvice = GetDailyAdvice(dayIndex);
        }

        private string GetDailyAdvice(int dayIndex)
        {
            return _dailyAdvices[dayIndex];
        }

        private string GetDailyAdvice()
        {
            int dayIndex = (int)DateTime.Now.DayOfWeek;

            return _dailyAdvices[dayIndex];
        }

        private async void GoToChatbot()
        {
            await Shell.Current.GoToAsync(nameof(ChatBotPage));
           
        }

        private async void RegisterActivity()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Application.Current.MainPage.DisplayAlert("Error",
                    "Se requiere conexión a Internet para poder registrar una actividad.", "OK");

                return;
            }
            
            await Shell.Current.GoToAsync(nameof(ActivityRecord));
        }

        private async void GoToPage(string route)
        {
            await Shell.Current.GoToAsync(route, true);
        }
    }
}