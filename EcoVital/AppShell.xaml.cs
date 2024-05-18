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
        Routing.RegisterRoute(nameof(HealthRemindersPage), typeof(HealthRemindersPage));
        Routing.RegisterRoute(nameof(FeedbackPage), typeof(FeedbackPage));
        Navigated += (sender, args) => BackgroundColor = Color.FromArgb("#76C893");
    }

    void OnNavigating(object? sender, ShellNavigatedEventArgs e)
    {
        FlyoutBehavior = e.Current.Location.OriginalString.Contains("LoginPage")
            ? FlyoutBehavior.Disabled
            : FlyoutBehavior.Flyout;
    }


    protected override void OnNavigated(ShellNavigatedEventArgs args)
    {
        base.OnNavigated(args);

        FlyoutBehavior = args.Current.Location.OriginalString.Contains("LoginPage")
            ? FlyoutBehavior.Disabled
            : FlyoutBehavior.Flyout;
    }
}