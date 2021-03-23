using System;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BigMission.RacePosition
{
    public partial class MainPage : ContentPage
    {
        private EventMetadataModel[] events;
        private bool keywordCancel = false;


        public MainPage()
        {
            InitializeComponent();
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            ResetKeyword();
            carNumber.Text = Preferences.Get("CarNum", "336");
            eventUrl.Text = Preferences.Get("EvtUrl", "");
            eventKeywords.Text = Preferences.Get("EvtKeywords", "World Racing League, WRL");
        }

        private async void KeywordConnect_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(carNumber.Text) || string.IsNullOrWhiteSpace(eventKeywords.Text))
            {
                await DisplayAlert("Error", "Enter car number and keywords", "OK");
            }
            else
            {
                if (kwConnect.Text == "Connect")
                {
                    kwConnect.Text = "Cancel";
                }
                else
                {
                    ResetKeyword();
                    return;
                }

                Preferences.Set("CarNum", carNumber.Text);
                Preferences.Set("EvtKeywords", eventKeywords.Text);

                keywordCancel = false;

                var evt = RaceHeroDataProvider.FindLiveEvent(events, eventKeywords.Text);
                if (evt != null)
                {
                    var url = CreateUrl(evt.Url);
                    await Navigation.PushAsync(new PositionStatus { CarNumber = carNumber.Text, EventUrl = url, Event = evt.Name });
                }
                else
                {
                    activitySl.IsVisible = true;
                    var ts = TimeSpan.FromSeconds(5);
                    Device.StartTimer(ts, () =>
                    {
                        try
                        {
                            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                            {
                                activityLabel.Text = "Waiting on Internet access";
                            }

                            if (keywordCancel)
                            {
                                return false;
                            }

                            RefreshEvents_Clicked(null, null);
                            evt = RaceHeroDataProvider.FindLiveEvent(events, eventKeywords.Text);
                            if (evt != null)
                            {
                                var url = CreateUrl(evt.Url);
                                Navigation.PushAsync(new PositionStatus { CarNumber = carNumber.Text, EventUrl = url, Event = evt.Name });
                                return false;
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Failed to connect, retrying");
                            activityLabel.Text = "Failed to connect, retrying";
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
                    await Navigation.PushAsync(new PositionStatus { CarNumber = carNumber.Text, EventUrl = url, Event = evt.Name });
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
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    events = RaceHeroDataProvider.RequestEvents();
                    EventPicker.ItemsSource = events.Select(s => s.Name).ToArray();
                }
                else
                {
                    DisplayAlert("Error", "Internet not available.", "OK").Wait();
                }
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

        private void ResetKeyword()
        {
            keywordCancel = true;
            kwConnect.Text = "Connect";
            activitySl.IsVisible = false;
        }
    }
}
