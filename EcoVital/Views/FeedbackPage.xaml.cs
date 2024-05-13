using System;
using EcoVital.Models;
using EcoVital.Services;
using EcoVital.ViewModels;
using Microsoft.Maui.Controls;

namespace EcoVital.Views
{
    public partial class FeedbackPage : ContentPage
    {
        public FeedbackPage()
        {
            InitializeComponent();
            BindingContext = new FeedbackViewModel(new FeedbackService(new HttpClient()));
            
        }

        private async void OnSendFeedbackClicked(object sender, EventArgs e)
        {
            if (BindingContext is not FeedbackViewModel viewModel) return;
            await viewModel.PostFeedbackAsync(viewModel.CurrentFeedback);
            // Limpiar después de enviar
            viewModel.CurrentFeedback = new Feedback();

        }
    }
}