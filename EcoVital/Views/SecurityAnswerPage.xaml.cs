using EcoVital.ViewModels;

namespace EcoVital.Views;

public partial class SecurityAnswerPage
{
    public SecurityAnswerPage()
    {
        InitializeComponent();
    }

    public SecurityAnswerPage(int userId, string securityQuestion)
    {
        InitializeComponent();
        BindingContext = new SecurityAnswerPageViewModel(userId, securityQuestion);
    }

    void OnBackButtonClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("LoginPage");
    }
}