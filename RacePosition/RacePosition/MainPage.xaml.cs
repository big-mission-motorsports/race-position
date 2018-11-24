using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RacePosition
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Connect_Clicked(object sender, EventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(carNumber.Text) || string.IsNullOrWhiteSpace(eventUrl.Text))
            {
                await DisplayAlert("Error", "Enter car number and the Race Hero event URL", "OK");
            }
            else
            {
                await Navigation.PushAsync(new PositionStatus { CarNumber = carNumber.Text, EventUrl = eventUrl.Text });
            }
        }
    }
}
