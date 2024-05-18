namespace EcoVital.Views;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
        BindingContext = App.HomePageViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Shell.Current.BackgroundColor = Color.FromArgb("#76C893");
    }
}