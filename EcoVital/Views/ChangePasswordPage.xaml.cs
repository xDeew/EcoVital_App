using EcoVital.Services;
using EcoVital.ViewModels;

namespace EcoVital.Views;

public partial class ChangePasswordPage : ContentPage
{
    public ChangePasswordPage()
    {
        InitializeComponent();
        BindingContext = new ChangePasswordViewModel(new LoginService());
    }
}