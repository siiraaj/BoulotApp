using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Boulot
{
    [Activity(Label = "AproposActivity", Theme = "@android:style/Theme.Black.NoTitleBar")]
    public class AproposActivity : Activity
    {
        ImageView img;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.aboutUs);

            img = FindViewById<ImageView>(Resource.Id.imageView1);

            // Create your application here
        }
    }
}