using EcoVital.ViewModels;

namespace EcoVital.Views;

/// <summary>
/// Página para el registro de nuevos usuarios.
/// </summary>
public partial class RegisterPage : ContentPage
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="RegisterPage"/>.
    /// </summary>
    public RegisterPage()
    {
        InitializeComponent();
        BindingContext = new RegisterPageViewModel();
    }

    /// <summary>
    /// Maneja el evento cuando el texto de la contraseña cambia.
    /// </summary>
    /// <param name="sender">El origen del evento.</param>
    /// <param name="e">Los datos del evento.</param>
    void OnPasswordTextChanged(object sender, TextChangedEventArgs e)
    {
        var isPasswordSecure = RegisterPageViewModel.IsPasswordSecure(e.NewTextValue);
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