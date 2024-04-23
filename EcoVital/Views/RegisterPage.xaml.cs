using EcoVital.ViewModels;

namespace EcoVital.Views
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
            BindingContext = new RegisterPageViewModel();
        }

        void OnPasswordTextChanged(object sender, TextChangedEventArgs e)
        {
            var isPasswordSecure = ((RegisterPageViewModel)BindingContext).IsPasswordSecure(e.NewTextValue);
            if (isPasswordSecure)
            {
                PasswordValidationLabel.Text = "Contraseña segura";
                PasswordValidationLabel.TextColor = Colors.Green;
            }
            else
            {
                PasswordValidationLabel.Text = "Contraseña insegura";
                PasswordValidationLabel.TextColor = Colors.Red;
            }
        }
    }
}