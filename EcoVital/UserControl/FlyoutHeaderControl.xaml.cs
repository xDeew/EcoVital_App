namespace EcoVital.UserControl;

public partial class FlyoutHeaderControl : ContentView
{
    public FlyoutHeaderControl()
    {
        InitializeComponent();
        lblUserName.Text = "Logueado como: " + App.UserInfo.UserName;
        lblUserEmail.Text = App.UserInfo.Email;
    }
}