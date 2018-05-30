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
    [Activity(Label = "GalerieActivity")]
    public class GalerieActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.DetaillsLayout);
            var gallery = FindViewById<Gallery>(Resource.Id.gallery);
            gallery.Adapter = new ImageAdapter(this);

            gallery.ItemClick += (s, e) => {

                Toast.MakeText(this, e.Position.ToString(), ToastLength.Short).Show();

            };
        }
    }
}