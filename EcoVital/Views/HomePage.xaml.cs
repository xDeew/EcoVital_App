﻿namespace EcoVital.Views
{
    public partial class HomePage : ContentPage
    {
        
        public HomePage()
        {
            InitializeComponent();
            BindingContext = App.HomePageViewModel; 
        }
        
    }
}