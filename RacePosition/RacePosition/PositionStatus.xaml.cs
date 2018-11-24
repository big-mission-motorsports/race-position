using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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

        private RaceHeroDataProvider dataProvider;

        public PositionStatus()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            dataProvider = new RaceHeroDataProvider(CarNumber, EventUrl);

            Device.StartTimer(TimeSpan.FromSeconds(3), () =>
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

        private void SetErrorMessage(string error)
        {
            this.errorLabel.Text = error;
        }
    }
}