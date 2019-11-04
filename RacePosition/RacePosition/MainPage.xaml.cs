using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace RacePosition
{
    public partial class MainPage : ContentPage
    {
        private EventMetadataModel[] events;


        public MainPage()
        {
            InitializeComponent();
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            carNumber.Text = Preferences.Get("CarNum", "336");
            eventUrl.Text = Preferences.Get("EvtUrl", "");
            eventKeywords.Text = Preferences.Get("EvtKeywords", "World Racing League, WRL");

            RefreshEvents_Clicked(null, null);
        }

        private async void KeywordConnect_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(carNumber.Text) || string.IsNullOrWhiteSpace(eventKeywords.Text))
            {
                await DisplayAlert("Error", "Enter car number and keywords", "OK");
            }
            else
            {
                Preferences.Set("CarNum", carNumber.Text);
                Preferences.Set("EvtKeywords", eventKeywords.Text);

                var evt = RaceHeroDataProvider.FindLiveEvent(events, eventKeywords.Text);
                if (evt != null)
                {
                    var url = CreateUrl(evt.Url);
                    await Navigation.PushAsync(new PositionStatus { CarNumber = carNumber.Text, EventUrl = url });
                }
                else
                {
                    // **** Add status indicator...
                    var ts = TimeSpan.FromSeconds(10);
                    Device.StartTimer(ts, () =>
                    {
                        RefreshEvents_Clicked(null, null);
                        evt = RaceHeroDataProvider.FindLiveEvent(events, eventKeywords.Text);
                        if (evt != null)
                        {
                            var url = CreateUrl(evt.Url);
                            Navigation.PushAsync(new PositionStatus { CarNumber = carNumber.Text, EventUrl = url });
                            return false;
                        }

                        return true;
                    });
                }
            }
        }

        private async void PickerConnect_Clicked(object sender, EventArgs e)
        {
            var eventName = EventPicker.SelectedItem;
            if (string.IsNullOrWhiteSpace(carNumber.Text) || eventName == null)
            {
                await DisplayAlert("Error", "Enter car number and select an event", "OK");
            }
            else
            {
                Preferences.Set("CarNum", carNumber.Text);
                if (events != null)
                {
                    var evt = events.FirstOrDefault(s => s.Name == EventPicker.SelectedItem.ToString());

                    var url = CreateUrl(evt.Url);
                    await Navigation.PushAsync(new PositionStatus { CarNumber = carNumber.Text, EventUrl = url });
                }

            }
        }

        private async void UrlConnect_Clicked(object sender, EventArgs e)
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

        private void RefreshEvents_Clicked(object sender, EventArgs e)
        {
            try
            {
                events = RaceHeroDataProvider.RequestEvents();
                EventPicker.ItemsSource = events.Select(s => s.Name).ToArray();
            }
            catch
            {
                EventPicker.ItemsSource = null;
            }
        }

        private static string CreateUrl(string evtUrl)
        {
            return RaceHeroDataProvider.SITE + "/events/" + evtUrl;
        }
    }
}
