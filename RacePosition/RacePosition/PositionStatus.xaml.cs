using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RacePosition
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PositionStatus : ContentPage
    {
        public string CarNumber { get; set; }
        public string EventUrl { get; set; }
        private const int INTERVALSEC = 3;

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
                dataProvider = new RaceHeroDataProvider(CarNumber, EventUrl);
                var ts = TimeSpan.FromSeconds(3);
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

                    SetErrorMessage(dataProvider.LastError);
                    return true;
                });
            }

            loaded = true;
        }

        private void SetErrorMessage(string error)
        {
            var timestamp = DateTime.Now;
            this.errorLabel.Text = timestamp + " :: " + error;
        }
    }
}