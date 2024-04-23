using EcoVital.Services;
using EcoVital.ViewModels;
using EcoVital.Views;

namespace EcoVital;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        BindingContext = new AppShellViewModel();
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
        Routing.RegisterRoute(nameof(SecurityAnswerPage), typeof(SecurityAnswerPage));
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(SecurityQuestionPage), typeof(SecurityQuestionPage));
        Routing.RegisterRoute(nameof(ForgotPasswordPage), typeof(ForgotPasswordPage));
        Routing.RegisterRoute(nameof(ChangePasswordPage), typeof(ChangePasswordPage));
        Routing.RegisterRoute(nameof(ChatBotPage), typeof(ChatBotPage));
        Routing.RegisterRoute(nameof(ActivityRecord), typeof(ActivityRecord));
        Routing.RegisterRoute(nameof(ProgressStatus), typeof(ProgressStatus));
        
                

    }

    void OnNavigating(object? sender, ShellNavigatedEventArgs e)
    {
        // Comprueba si el usuario está navegando a la página de inicio de sesión
        if (e.Current.Location.OriginalString == "///LoginPage")
        {
            // Desactiva el Flyout
            this.FlyoutBehavior = FlyoutBehavior.Disabled;
        }
        else
        {
            // Si el usuario está navegando a cualquier otra página y está autenticado, habilita el Flyout
            this.FlyoutBehavior = FlyoutBehavior.Flyout;
        }
    }


    protected override void OnNavigated(ShellNavigatedEventArgs args)
    {
        base.OnNavigated(args);
        // Ajusta el FlyoutBehavior basándote en la página actual
        if (args.Current.Location.OriginalString.Contains("LoginPage"))
        {
            this.FlyoutBehavior = FlyoutBehavior.Disabled;
        }
        else
        {
            // Activa el Flyout para todas las demás páginas
            this.FlyoutBehavior = FlyoutBehavior.Flyout;
        }
    }
}