using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace RacePosition
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            carNumber.Text = Preferences.Get("CarNum", "336");
            eventUrl.Text = Preferences.Get("EvtUrl", "");
        }

        private async void Connect_Clicked(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(carNumber.Text) || string.IsNullOrWhiteSpace(eventUrl.Text))
            {
                await DisplayAlert("Error", "Enter car number and the Race Hero event URL", "OK");
            }
            else
            {
                Preferences.Set("CarNum", carNumber.Text);
                Preferences.Set("EvtUrl", eventUrl.Text);

                await Navigation.PushAsync(new PositionStatus { CarNumber = carNumber.Text, EventUrl = eventUrl.Text });
            }
        }
    }
}
