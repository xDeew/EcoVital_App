using EcoVital.ViewModels;
using EcoVital.Views;

namespace EcoVital;

/// <summary>
/// Clase que define la estructura de navegación de la aplicación.
/// </summary>
public partial class AppShell : Shell
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="AppShell"/>.
    /// </summary>
    public AppShell()
    {
        InitializeComponent();
        BindingContext = new AppShellViewModel();
        RegisterRoutes();
        Navigated += (sender, args) => BackgroundColor = Color.FromArgb("#76C893");
    }

    /// <summary>
    /// Registra las rutas de navegación de la aplicación.
    /// </summary>
    void RegisterRoutes()
    {
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
        Routing.RegisterRoute(nameof(HealthRemindersPage), typeof(HealthRemindersPage));
        Routing.RegisterRoute(nameof(FeedbackPage), typeof(FeedbackPage));
    }

    /// <summary>
    /// Maneja el evento de navegación.
    /// </summary>
    /// <param name="sender">El origen del evento.</param>
    /// <param name="e">Los datos del evento.</param>
    void OnNavigating(object? sender, ShellNavigatedEventArgs e)
    {
        FlyoutBehavior = e.Current.Location.OriginalString.Contains("LoginPage")
            ? FlyoutBehavior.Disabled
            : FlyoutBehavior.Flyout;
    }

    /// <summary>
    /// Método llamado cuando la navegación ha finalizado.
    /// </summary>
    /// <param name="args">Los datos del evento.</param>
    protected override void OnNavigated(ShellNavigatedEventArgs args)
    {
        base.OnNavigated(args);

        FlyoutBehavior = args.Current.Location.OriginalString.Contains("LoginPage")
            ? FlyoutBehavior.Disabled
            : FlyoutBehavior.Flyout;
    }
}