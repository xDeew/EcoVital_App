﻿using EcoVital.Services;
using EcoVital.ViewModels;

namespace EcoVital.Views;

public partial class LoginPage : ContentPage
{
    readonly ILoadingService _loadingService = new LoadingService();
    readonly ILoginRepository _loginRepository = new LoginService();

    public LoginPage()
    {
        InitializeComponent();
        BindingContext = new LoginRegister(_loginRepository, _loadingService);
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
    }

    protected void OnAppearing(object? sender, EventArgs eventArgs)
    {
        base.OnAppearing();

        // Desactivar el Flyout
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Desactivar el Flyout
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
    }

    protected override bool OnBackButtonPressed() =>
        // True = Cancela la acción de volver atrás
        // False = Permite la acción normal de volver atrás
        true;
}