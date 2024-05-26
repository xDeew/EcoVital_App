namespace EcoVital.UserControl;

/// <summary>
/// Control de cabecera del Flyout que muestra la información del usuario logueado.
/// </summary>
public partial class FlyoutHeaderControl : ContentView
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="FlyoutHeaderControl"/>.
    /// </summary>
    public FlyoutHeaderControl()
    {
        InitializeComponent();
        lblUserName.Text = "Logueado como: " + App.UserInfo.UserName;
        lblUserEmail.Text = App.UserInfo.Email;
    }
}