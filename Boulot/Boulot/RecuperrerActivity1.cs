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
    [Activity(Theme = "@android:style/Theme.Black.NoTitleBar")]
    public class RecuperrerActivity1 : Activity
    {
        EditText RecupTele;
        Button b1;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Recuperer);
            RecupTele = FindViewById<EditText>(Resource.Id.RecupTele);
            b1 = FindViewById<Button>(Resource.Id.b1);
            b1.Click += B1_Click;
        }

        private void B1_Click(object sender, EventArgs e)
        {
            webService.h21WebService1 ws = new webService.h21WebService1();
            ws.getQuestionCompleted += Ws_getQuestionCompleted;
            ws.getQuestionAsync(RecupTele.Text);
        }

        private void Ws_getQuestionCompleted(object sender, webService.getQuestionCompletedEventArgs e)
        {
            if (e.Result.Equals("aucun"))
            {
                var callDialog = new AlertDialog.Builder(this);
                callDialog.SetTitle("Information");
                callDialog.SetMessage("Aucune compte associe a cet numero de telefone");
                callDialog.SetNeutralButton("Ok", delegate
                {

                });
                callDialog.Show();
            }
            else
            {
                Intent intent = new Intent(this, typeof(RecuperrerActivity2));
                intent.PutExtra("question", e.Result);
                intent.PutExtra("tele", RecupTele.Text);
                StartActivity(intent);
            }


        }
    }
}