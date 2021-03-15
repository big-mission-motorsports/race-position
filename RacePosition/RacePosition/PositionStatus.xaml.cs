using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BigMission.RacePosition
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PositionStatus : ContentPage
    {
        public string CarNumber { get; set; }
        public string EventUrl { get; set; }
        public string Event { get; set; }
        private const int INTERVALSEC = 5;

        private RaceHeroDataProvider dataProvider;
        private bool loaded = false;


        public PositionStatus()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!loaded)
            {
                Title = CarNumber;
                if (!string.IsNullOrWhiteSpace(Event))
                {
                    Title += " - " + Event;
                }
                dataProvider = new RaceHeroDataProvider(CarNumber, EventUrl);
                var ts = TimeSpan.FromSeconds(INTERVALSEC);
                Device.StartTimer(ts, () =>
                {
                    if (!dataProvider.IsInialized)
                    {
                        dataProvider.InitilaizePath();
                    }

                    if (dataProvider.IsInialized)
                    {
                        racerList.BeginRefresh();
                        try
                        {
                            var data = dataProvider.RequestUpdate();
                            racerList.ItemsSource = data;
                        }
                        finally
                        {
                            racerList.EndRefresh();
                        }
                    }

                    posInClassLabel.Text = dataProvider.PositionInClass;
                    posOverallLabel.Text = dataProvider.PositionOverall;

                    SetErrorMessage(dataProvider.LastError);
                    return true;
                });
            }

            loaded = true;
        }

        private void SetErrorMessage(string error)
        {
            var timestamp = DateTime.Now;
            errorLabel.Text = timestamp + " :: " + error;
        }
    }
}