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
    [Activity(Label = "Review", Theme = "@android:style/Theme.Black.NoTitleBar")]
    public class ReviewActivity : Activity
    {
        Button addScore;
        RatingBar score;
        EditText email;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Review);

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Review);
            score = FindViewById<RatingBar>(Resource.Id.review);
            email = FindViewById<EditText>(Resource.Id.mailR);
            addScore = FindViewById<Button>(Resource.Id.add_scor);

            addScore.Click += AddScore_Click;
            // Create your application here
        }
        private void AddScore_Click(object sender, EventArgs e)
        {
            Intent i = this.Intent;

            int id = Int32.Parse(i.Extras.GetString("id_emp_review"));
            webService.h21WebService1 ws = new webService.h21WebService1();
            ws.reviewCompleted += Ws_reviewCompleted;
            ws.reviewAsync(id, email.Text, score.Rating);
        }

        private void Ws_reviewCompleted(object sender, webService.reviewCompletedEventArgs e)
        {
            if ("sucess".Equals(e.Result))
            {

                var callDialog = new AlertDialog.Builder(this);
                callDialog.SetTitle("revue");
                callDialog.SetMessage("merci chers client pour votre avis");
                callDialog.SetNeutralButton("Ok", delegate
                {

                });
                callDialog.Show();

            }
            else
            {
                var callDialog = new AlertDialog.Builder(this);
                callDialog.SetTitle("erreur");
                callDialog.SetMessage("vous etez deja donnee votre avis pour ce bricoleur");
                callDialog.SetNeutralButton("Ok", delegate
                {

                });
                callDialog.Show();

            }

        }
    }
}