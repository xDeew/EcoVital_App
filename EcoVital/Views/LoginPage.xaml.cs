using EcoVital.ViewModels;
using EcoVital.Views;

namespace EcoVital;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        BindingContext = new LoginPageViewModel();
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
        // Replace LoginPageViewModel with the actual type of _viewModel

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
    
    protected override bool OnBackButtonPressed()
    {
        // True = Cancela la acción de volver atrás
        // False = Permite la acción normal de volver atrás
        return true;
    }
}