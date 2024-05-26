using EcoVital.Services;
using EcoVital.ViewModels;
using EcoVital.Views;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;

namespace EcoVital;

/// <summary>
/// Clase que configura la aplicación Maui.
/// </summary>
public static class MauiProgram
{
    /// <summary>
    /// Crea y configura la aplicación Maui.
    /// </summary>
    /// <returns>Una instancia configurada de <see cref="MauiApp"/>.</returns>
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseLocalNotification()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("FontAwesome.ttf", "FontAwesome");
            });

        // Registro de páginas para inyección de dependencias
        builder.Services.AddSingleton<HomePage>();
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<LoginRegister>();
        builder.Services.AddSingleton<AboutPage>();
        builder.Services.AddSingleton<ContactPage>();
        builder.Services.AddSingleton<ChatBotPage>();

        // Registro de servicios para inyección de dependencias
        builder.Services.AddSingleton<ActivityService>();
        builder.Services.AddSingleton<UserGoalService>();
        builder.Services.AddSingleton<FeedbackService>();

        // Registro de ViewModels para inyección de dependencias
        builder.Services.AddTransient<ActivityRecordViewModel>();
        builder.Services.AddTransient<ProgressStatusViewModel>();
        builder.Services.AddTransient<FeedbackViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}