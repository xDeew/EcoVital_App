using EcoVital.ViewModels;

namespace EcoVital.Views;

public partial class SecurityQuestionPage
{
    public SecurityQuestionPage()
    {
        InitializeComponent();
        BindingContext = new SecurityQuestionPageViewModel();
    }

    protected override bool OnBackButtonPressed() =>
        // True = Cancela la acción de volver atrás
        // False = Permite la acción normal de volver atrás
        true;
}