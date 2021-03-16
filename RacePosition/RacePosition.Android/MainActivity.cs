using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace BigMission.RacePosition.Droid
{
    [Activity(Label = "Race Position", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            this.Window.SetFlags(WindowManagerFlags.KeepScreenOn, WindowManagerFlags.KeepScreenOn);
            LoadApplication(new App());

            AppCenter.Start("4ac4cd9d-8cbc-42ba-bf8b-f830857bdd0c", typeof(Analytics), typeof(Crashes));
            AppCenter.Start("4ac4cd9d-8cbc-42ba-bf8b-f830857bdd0c", typeof(Analytics), typeof(Crashes));
        }
    }
}