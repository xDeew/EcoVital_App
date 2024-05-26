using System.Collections.ObjectModel;
using System.Windows.Input;
using EcoVital.Models;
using Plugin.LocalNotification;

namespace EcoVital.ViewModels;

/// <summary>
/// ViewModel para gestionar los recordatorios de salud.
/// </summary>
public class HealthRemindersViewModel : BaseViewModel
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="HealthRemindersViewModel"/>.
    /// </summary>
    public HealthRemindersViewModel()
    {
        AddReminderCommand = new Command(AddReminder);
        Reminders = new ObservableCollection<HealthReminder>
        {
            new()
            {
                ReminderType = "Beber agua",
                ReminderMessage = "Es hora de beber un vaso de agua.",
                ImageSource = "water.png",
                ReminderTime = ""
            },
            new()
            {
                ReminderType = "Tomar medicamento",
                ReminderMessage = "No olvides tomar tu medicamento.",
                ImageSource = "medication.png",
                ReminderTime = ""
            },
            new()
            {
                ReminderType = "Revisión médica",
                ReminderMessage = "Recuerda tu cita médica.",
                ImageSource = "medical_checkup.png",
                ReminderTime = ""
            },
            new()
            {
                ReminderType = "Ejercicio diario",
                ReminderMessage = "Tiempo para tu rutina de ejercicio.",
                ImageSource = "exercise.png",
                ReminderTime = ""
            },
            new()
            {
                ReminderType = "Meditar",
                ReminderMessage = "Un momento de meditación para empezar el día.",
                ImageSource = "meditations.png",
                ReminderTime = ""
            },
            new()
            {
                ReminderType = "Llamada de seguimiento",
                ReminderMessage = "Llamada de seguimiento con el especialista.",
                ImageSource = "followup_call.png",
                ReminderTime = ""
            },
            new()
            {
                ReminderType = "Comida saludable",
                ReminderMessage = "Recuerda comer algo saludable.",
                ImageSource = "healthy_food.png",
                ReminderTime = ""
            },
            new()
            {
                ReminderType = "Dormir",
                ReminderMessage = "Prepárate para dormir y descansar.",
                ImageSource = "sleep.png",
                ReminderTime = ""
            }
        };
    }

    /// <summary>
    /// Comando para añadir un nuevo recordatorio.
    /// </summary>
    public ICommand AddReminderCommand { get; }

    /// <summary>
    /// Colección de recordatorios de salud.
    /// </summary>
    public ObservableCollection<HealthReminder> Reminders { get; }

    /// <summary>
    /// Obtiene o establece la hora seleccionada para el recordatorio.
    /// </summary>
    public TimeSpan SelectedTime { get; set; } = TimeSpan.Zero;

    /// <summary>
    /// Añade un nuevo recordatorio.
    /// </summary>
    /// <param name="obj">El objeto de recordatorio.</param>
    public async void AddReminder(object obj)
    {
        var reminder = obj as HealthReminder;
        if (reminder == null)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "No se pudo obtener el recordatorio.", "OK");
            return;
        }

        if (!await LocalNotificationCenter.Current.AreNotificationsEnabled())
        {
            var result = await LocalNotificationCenter.Current.RequestNotificationPermission();
            if (!result)
            {
                await Application.Current.MainPage.DisplayAlert("Permission Denied",
                    "Notifications permission was not granted. Please enable it in settings to receive reminders.",
                    "OK");
                return;
            }
        }

        var notificationMessage = GetNotificationMessage(reminder);
        var now = DateTime.Now;
        var scheduledNotificationTime = DateTime.Today.Add(SelectedTime);
        if (scheduledNotificationTime < now) scheduledNotificationTime = scheduledNotificationTime.AddDays(1);

        var request = new NotificationRequest
        {
            NotificationId = new Random().Next(),
            Title = "Recordatorio de salud - " + reminder.ReminderType,
            Description = notificationMessage,
            Schedule = new NotificationRequestSchedule
            {
                NotifyTime = scheduledNotificationTime,
                NotifyRepeatInterval = TimeSpan.FromDays(1) // Repetir cada 24 horas
            }
        };

        await LocalNotificationCenter.Current.Show(request);
        await Application.Current.MainPage.DisplayAlert("Recordatorio añadido", "El recordatorio se ha añadido correctamente.", "OK");
    }

    /// <summary>
    /// Obtiene el mensaje de notificación para un recordatorio específico.
    /// </summary>
    /// <param name="reminder">El recordatorio de salud.</param>
    /// <returns>El mensaje de notificación.</returns>
    string GetNotificationMessage(HealthReminder reminder)
    {
        return reminder.ReminderType switch
        {
            "Beber agua" => "Es hora de beber un vaso de agua.",
            "Tomar medicamento" => "No olvides tomar tu medicamento.",
            "Revisión médica" => "Recuerda tu cita médica.",
            "Ejercicio diario" => "Tiempo para tu rutina de ejercicio.",
            "Meditar" => "Un momento de meditación para empezar el día.",
            "Llamada de seguimiento" => "Llamada de seguimiento con el especialista.",
            "Comida saludable" => "Recuerda comer algo saludable.",
            "Dormir" => "Prepárate para dormir y descansar.",
            _ => "Este es un recordatorio general."
        };
    }
}