using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Text;
using Android.Text.Method;

namespace Boulot
{
    [Activity(Label = "main", Theme = "@android:style/Theme.Black.NoTitleBar", Icon = "@drawable/ic_launcher", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            Button connection = FindViewById<Button>(Resource.Id.btnConnection);
            Button service = FindViewById<Button>(Resource.Id.btnService);

            connection.Click += Connection_Click;
            service.Click += Service_Click;


            TextView CreerCompte = FindViewById<TextView>(Resource.Id.textView);
            CreerCompte.Click += CreerCompte_Click;
            TextView map = FindViewById<TextView>(Resource.Id.map);
            map.Click += Map_Click;

        }

        private void Map_Click(object sender, System.EventArgs e)
        {
            StartActivity(typeof(Map));
        }

        private void CreerCompte_Click(object sender, System.EventArgs e)
        {
            StartActivity(typeof(AproposActivity));

        }

        private void Service_Click(object sender, System.EventArgs e)
        {
            StartActivity(typeof(SearchActivity));

        }

        private void Connection_Click(object sender, System.EventArgs e)
        {
            StartActivity(typeof(LoginActivity));
        }
    }
}

