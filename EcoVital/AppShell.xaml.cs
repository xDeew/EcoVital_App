﻿using EcoVital.Services;
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
    }

    void OnNavigating(object? sender, ShellNavigatedEventArgs e)
    {
        if (e.Current.Location.OriginalString == "///LoginPage")
        {
            // Desactiva el Flyout
            FlyoutBehavior = FlyoutBehavior.Disabled;
        }
        else
        {
            FlyoutBehavior = FlyoutBehavior.Flyout;
        }
    }


    protected override void OnNavigated(ShellNavigatedEventArgs args)
    {
        base.OnNavigated(args);

        if (args.Current.Location.OriginalString.Contains("LoginPage"))
        {
            FlyoutBehavior = FlyoutBehavior.Disabled;
        }
        else
        {
            FlyoutBehavior = FlyoutBehavior.Flyout;
        }
    }
}