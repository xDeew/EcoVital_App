using System.Collections.ObjectModel;
using System.Windows.Input;
using EcoVital.Models;
using Plugin.LocalNotification;

namespace EcoVital.ViewModels;

public class HealthRemindersViewModel : BaseViewModel
{
    public ICommand AddReminderCommand { get; }
    public ObservableCollection<HealthReminder> Reminders { get; }
    public TimeSpan SelectedTime { get; set; } = TimeSpan.Zero;

    public HealthRemindersViewModel()
    {
        AddReminderCommand = new Command(AddReminder);
        Reminders = new ObservableCollection<HealthReminder>
        {
            new()
            {
                ReminderType = "Beber agua", ReminderMessage = "Es hora de beber un vaso de agua.",
                ImageSource = "water.png", ReminderTime = ""
            },
            new()
            {
                ReminderType = "Tomar medicamento", ReminderMessage = "No olvides tomar tu medicamento.",
                ImageSource = "medication.png", ReminderTime = ""
            },
            new()
            {
                ReminderType = "Revisión médica", ReminderMessage = "Recuerda tu cita médica.",
                ImageSource = "medical_checkup.png", ReminderTime = ""
            },
            new()
            {
                ReminderType = "Ejercicio diario", ReminderMessage = "Tiempo para tu rutina de ejercicio.",
                ImageSource = "exercise.png", ReminderTime = ""
            },
            new()
            {
                ReminderType = "Meditar", ReminderMessage = "Un momento de meditación para empezar el día.",
                ImageSource = "meditations.png", ReminderTime = ""
            },
            new()
            {
                ReminderType = "Llamada de seguimiento",
                ReminderMessage = "Llamada de seguimiento con el especialista.", ImageSource = "followup_call.png",
                ReminderTime = ""
            },
            new()
            {
                ReminderType = "Comida saludable", ReminderMessage = "Recuerda comer algo saludable.",
                ImageSource = "healthy_food.png", ReminderTime = ""
            },
            new()
            {
                ReminderType = "Dormir", ReminderMessage = "Prepárate para dormir y descansar.",
                ImageSource = "sleep.png", ReminderTime = ""
            }
        };
    }


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
        if (scheduledNotificationTime < now)
        {
            scheduledNotificationTime = scheduledNotificationTime.AddDays(1);
        }

        var request = new NotificationRequest
        {
            NotificationId = new Random().Next(),
            Title = "Recordatorio de salud - " + reminder.ReminderType,
            Description = notificationMessage,
            Schedule = new NotificationRequestSchedule
            {
                // NotifyTime = DateTime.Today.Add(SelectedTime),
                // NotifyTime = DateTime.Now.AddSeconds(5), // Para pruebas
                NotifyTime = scheduledNotificationTime,
                //  NotifyRepeatInterval = TimeSpan.FromSeconds(10) // Repetir cada 10 segundos para pruebas
                NotifyRepeatInterval = TimeSpan.FromDays(1) // Repetir cada 24 horas
            },
        };

        await LocalNotificationCenter.Current.Show(request);

        await Application.Current.MainPage.DisplayAlert("Recordatorio añadido",
            "El recordatorio se ha añadido correctamente.", "OK");
    }

    string GetNotificationMessage(HealthReminder reminder)
    {
        switch (reminder.ReminderType)
        {
            case "Beber agua":
                return "Es hora de beber un vaso de agua.";
            case "Tomar medicamento":
                return "No olvides tomar tu medicamento.";
            case "Revisión médica":
                return "Recuerda tu cita médica.";
            case "Ejercicio diario":
                return "Tiempo para tu rutina de ejercicio.";
            case "Meditar":
                return "Un momento de meditación para empezar el día.";
            case "Llamada de seguimiento":
                return "Llamada de seguimiento con el especialista.";
            case "Comida saludable":
                return "Recuerda comer algo saludable.";
            case "Dormir":
                return "Prepárate para dormir y descansar.";
            default:
                return "Este es un recordatorio general.";
        }
    }
}