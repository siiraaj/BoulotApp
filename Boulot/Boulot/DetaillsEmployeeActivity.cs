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
using Square.Picasso;

namespace Boulot
{
    [Activity(Label = "DetaillsEmployeeActivity", Theme = "@android:style/Theme.Black.NoTitleBar")]
    public class DetaillsEmployeeActivity : Activity
    {

        private TextView nameTxt, prenomTxt, villeText, ServiceTxt, PhoneTxt;
        private Button mbtnPhone;
        private ImageView img;
        private RatingBar r;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DetaillsLayout);

            var gallery = FindViewById<Gallery>(Resource.Id.gallery);


            Button appel = FindViewById<Button>(Resource.Id.phone);

            appel.Click += Appel_Click;

            r = FindViewById<RatingBar>(Resource.Id.review);
            gallery.Adapter = new ImageAdapter(this);

            gallery.ItemClick += (s, e) => {

                Toast.MakeText(this, e.Position.ToString(), ToastLength.Short).Show();

            };
            nameTxt = FindViewById<TextView>(Resource.Id.txtNom);
            prenomTxt = FindViewById<TextView>(Resource.Id.txtPrenom);

            villeText = FindViewById<TextView>(Resource.Id.txtVillee);
            ServiceTxt = FindViewById<TextView>(Resource.Id.txtService);
            PhoneTxt = FindViewById<TextView>(Resource.Id.txtPhoone);

            // recieve data 

            Intent i = this.Intent;
            string namee = i.Extras.GetString("nameKey");
            string prenomm = i.Extras.GetString("prenomKey");
            string villeee = i.Extras.GetString("villeKey");
            string servicee = i.Extras.GetString("ServiceKey");
            string phonee = i.Extras.GetString("phoneKey");
            string imageUrl = i.Extras.GetString("imageKey");
            float review = i.Extras.GetFloat("reviewKey");

            //show Data

            nameTxt.Text = namee;
            prenomTxt.Text = prenomm;
            villeText.Text = villeee;
            ServiceTxt.Text = servicee;
            PhoneTxt.Text = phonee;
            r.Rating = review;
            Picasso.With(this)
             .Load(imageUrl)
             .Into(FindViewById<ImageView>(Resource.Id.user_profile_photo));




        }

        private void Appel_Click(object sender, EventArgs e)
        {

            Intent i = this.Intent;
            string tele = i.Extras.GetString("phoneKey");
            var uri = Android.Net.Uri.Parse("tel:" + tele);
            var intent = new Intent(Intent.ActionDial, uri);
            StartActivity(intent);
        }
    }
}