using System;
using EcoVital.Services;
using EcoVital.ViewModels;

namespace EcoVital.Views;

public partial class ActivityRecord
{
    private ActivityRecordViewModel _viewModel;

    public ActivityRecord()
    {
        InitializeComponent();
        HttpClient client = new HttpClient();
        _viewModel = new ActivityRecordViewModel(new ActivityService(client), new UserGoalService(client));
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadActivitiesCommand.ExecuteAsync(null);
        
    }

    private async void OnInfoButtonClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Información", "Para seleccionar una actividad, toca la imagen de la actividad correspondiente.", "OK");
    }
}