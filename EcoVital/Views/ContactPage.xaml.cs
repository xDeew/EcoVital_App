

using EcoVital.ViewModels;

namespace EcoVital.Views;

public partial class ContactPage : ContentPage
{
    public ContactPage()
    {
        InitializeComponent();
        BindingContext = new ContactPageViewModel();
    }
}