using System.Windows.Input;
using EcoVital.Views;
using Microsoft.Toolkit.Mvvm.Input;

namespace EcoVital.ViewModels;

/// <summary>
/// ViewModel para la página de inicio.
/// </summary>
public class HomePageViewModel : BaseViewModel
{
    readonly string[] _dailyAdvices =
    {
        "Mantén una dieta equilibrada y bebe suficiente agua.",
        "Realiza al menos 30 minutos de actividad física hoy.",
        "Tómate un tiempo para meditar y despejar tu mente.",
        "Recuerda estirar antes y después de hacer ejercicio.",
        "Duerme al menos 7 horas para un descanso óptimo.",
        "Prueba algo nuevo hoy, ¡sal de tu zona de confort!",
        "Dedica tiempo a conectarte con amigos o familia."
    };

    string _userName = "usuario predeterminado";

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="HomePageViewModel"/>.
    /// </summary>
    public HomePageViewModel()
    {
        GoToChatbotCommand = new RelayCommand(GoToChatbot);
        RegisterActivityCommand = new RelayCommand(RegisterActivity);
        GoToHealthRemindersPageCommand = new RelayCommand(() => GoToPage(nameof(HealthRemindersPage)));
        GoToProgressPageCommand = new RelayCommand(() => GoToPage("//ProgressStatus"));
        DailyAdvice = GetDailyAdvice();
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="HomePageViewModel"/> para un día específico.
    /// </summary>
    /// <param name="specificDay">El día específico para el cual se desea obtener el consejo diario.</param>
    public HomePageViewModel(DateTime? specificDay = null)
    {
        var dayIndex = (int)(specificDay ?? DateTime.Now).DayOfWeek;
        DailyAdvice = GetDailyAdvice(dayIndex);
    }

    /// <summary>
    /// Obtiene o establece el nombre del usuario.
    /// </summary>
    public string UserName
    {
        get => _userName;
        set => SetProperty(ref _userName, value);
    }

    /// <summary>
    /// Obtiene el consejo diario.
    /// </summary>
    public string DailyAdvice { get; private set; }

    /// <summary>
    /// Comando para ir al chatbot.
    /// </summary>
    public ICommand GoToChatbotCommand { get; private set; }

    /// <summary>
    /// Comando para registrar una actividad.
    /// </summary>
    public ICommand RegisterActivityCommand { get; private set; }

    /// <summary>
    /// Comando para ir a la página de recordatorios de salud.
    /// </summary>
    public ICommand GoToHealthRemindersPageCommand { get; private set; }

    /// <summary>
    /// Comando para ir a la página de progreso.
    /// </summary>
    public ICommand GoToProgressPageCommand { get; private set; }

    string GetDailyAdvice(int dayIndex) => _dailyAdvices[dayIndex];

    string GetDailyAdvice()
    {
        var dayIndex = (int)DateTime.Now.DayOfWeek;
        return _dailyAdvices[dayIndex];
    }

    async void GoToChatbot()
    {
        var chatBotPage = new ChatBotPage();
        await Shell.Current.Navigation.PushAsync(chatBotPage);
    }

    async void RegisterActivity()
    {
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await Application.Current.MainPage.DisplayAlert("Error",
                "Se requiere conexión a Internet para poder registrar una actividad.", "OK");
            return;
        }

        await Shell.Current.GoToAsync(nameof(ActivityRecord));
    }

    async void GoToPage(string route)
    {
        await Shell.Current.GoToAsync(route, true);
    }
}