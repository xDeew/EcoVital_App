using EcoVital.Services;
using EcoVital.ViewModels;
using EcoVital.Views;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;

namespace EcoVital;

public static class MauiProgram
{
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


        builder.Services.AddSingleton<HomePage>();
        builder.Services.AddSingleton<LoginPage>(); 
        // esto se agrega para que se pueda inyectar la dependencia de
        // la página, es decir, para que se pueda inyectar la vista en el ViewModel

        builder.Services.AddSingleton<LoginRegister>();
        builder.Services.AddSingleton<AboutPage>();
        builder.Services.AddSingleton<ContactPage>();
        builder.Services.AddSingleton<ChatBotPage>();
        
        
        builder.Services.AddSingleton<ActivityService>();
        builder.Services.AddSingleton<UserGoalService>();
        // Se usa AddTransient para que se cree una nueva instancia cada vez que se solicite
        builder.Services.AddTransient<ActivityRecordViewModel>();
        builder.Services.AddTransient<ProgressStatusViewModel>();
      





#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}