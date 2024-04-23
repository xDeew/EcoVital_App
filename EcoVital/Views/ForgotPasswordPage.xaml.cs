using EcoVital.ViewModels;
using Microsoft.Maui.Controls;

namespace EcoVital.Views
{
    public partial class ForgotPasswordPage : ContentPage
    {
        public ForgotPasswordPage()
        {
            InitializeComponent();
            BindingContext = new ForgotPasswordPageViewModel();
        }
    }
}