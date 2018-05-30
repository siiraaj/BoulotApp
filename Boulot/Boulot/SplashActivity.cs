using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Boulot
{

    [Activity(Label = "SplashScreen")]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.SplashScrenn);

            //Display Splash Screen for 4 Sec  
            Thread.Sleep(4000);
            //Start Activity1 Activity  
            StartActivity(typeof(MainActivity));
        }
    }
}