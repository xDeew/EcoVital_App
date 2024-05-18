using EcoVital.ViewModels;

namespace EcoVital.Views;

public partial class ForgotPasswordPage : ContentPage
{
    public ForgotPasswordPage()
    {
        InitializeComponent();
        BindingContext = new ForgotPasswordPageViewModel();
    }
}