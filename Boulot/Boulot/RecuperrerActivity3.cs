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
    public class RecuperrerActivity3 : Activity
    {
        Button save;
        EditText Pass, repPass;
        String tel;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.recupperer3);
            save = FindViewById<Button>(Resource.Id.setPassword);
            Pass = FindViewById<EditText>(Resource.Id.password);
            repPass = FindViewById<EditText>(Resource.Id.repPassword);
            tel = Intent.GetStringExtra("telef");
            save.Click += Save_Click;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            webService.h21WebService1 ws = new webService.h21WebService1();
            ws.setPasswordCompleted += Ws_setPasswordCompleted;
            ws.setPasswordAsync(tel, Pass.Text, repPass.Text);
        }

        private void Ws_setPasswordCompleted(object sender, webService.setPasswordCompletedEventArgs e)
        {
            if (e.Result.Equals("succes"))
            {
                var callDialog = new AlertDialog.Builder(this);
                callDialog.SetTitle("Information");
                callDialog.SetMessage("votre mot de pass a ete modifier votre nouveau mot de pass et : " + Pass.Text);
                callDialog.SetNeutralButton("Ok", delegate
                {
                    Intent intent = new Intent(this, typeof(LoginActivity));
                    StartActivity(intent);
                });
                callDialog.Show();
            }
            else
            {
                var callDialog = new AlertDialog.Builder(this);
                callDialog.SetTitle("Information");
                callDialog.SetMessage(e.Result);
                callDialog.SetNeutralButton("Ok", delegate
                {

                });
                callDialog.Show();
            }
        }
    }
}