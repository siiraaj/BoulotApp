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
    public class RecuperrerActivity2 : Activity
    {
        Button reset;
        TextView question;
        EditText answar;
        String tel;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Recuperer2);

            reset = FindViewById<Button>(Resource.Id.reset);
            question = FindViewById<TextView>(Resource.Id.questionReset);
            answar = FindViewById<EditText>(Resource.Id.answerReset);

            question.Text = Intent.GetStringExtra("question");
            tel = Intent.GetStringExtra("tele");
            reset.Click += Reset_Click;

        }

        private void Reset_Click(object sender, EventArgs e)
        {
            webService.h21WebService1 ws = new webService.h21WebService1();
            ws.Valide_answerCompleted += Ws_Valide_answerCompleted;
            ws.Valide_answerAsync(tel, question.Text, answar.Text);
        }

        private void Ws_Valide_answerCompleted(object sender, webService.Valide_answerCompletedEventArgs e)
        {
            if (e.Result.Equals("echec"))
            {
                var callDialog = new AlertDialog.Builder(this);
                callDialog.SetTitle("Information");
                callDialog.SetMessage("votre reponse et incorrect");
                callDialog.SetNeutralButton("Ok", delegate
                {

                });
                callDialog.Show();
            }
            else
            {
                Intent intent = new Intent(this, typeof(RecuperrerActivity3));
                intent.PutExtra("telef", tel);
                StartActivity(intent);
            }
        }
    }
}