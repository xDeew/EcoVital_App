namespace EcoVital.Views;

public partial class AboutPage
{
    public AboutPage()
    {
        InitializeComponent();
    }

    async void OnFeedbackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(FeedbackPage));
    }
}