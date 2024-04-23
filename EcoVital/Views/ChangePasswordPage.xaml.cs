using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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