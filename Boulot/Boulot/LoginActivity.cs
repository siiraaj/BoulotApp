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
using System.Data;

namespace Boulot
{
    [Activity(Label = "login", Theme = "@android:style/Theme.Black.NoTitleBar")]
    public class LoginActivity : Activity

    {
        TextView recup;
        Button login, register;
        EditText username, password;
        ImageView left;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.login);
            left = FindViewById<ImageView>(Resource.Id.left);
            username = FindViewById<EditText>(Resource.Id.etUsername);
            password = FindViewById<EditText>(Resource.Id.etPassword);
            recup = FindViewById<TextView>(Resource.Id.recup);

            login = FindViewById<Button>(Resource.Id.bulogin);
            register = FindViewById<Button>(Resource.Id.buRegister);

            register.Click += Register_Click;

            login.Click += Login_Click;

            left.Click += Left_Click;

            recup.Click += Recup_Click;

        }

        private void Recup_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(RecuperrerActivity1));
        }

        private void Register_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(Inscription1Activity));
        }

        private void Login_Click(object sender, EventArgs e)
        {
            webService.h21WebService1 ws = new webService.h21WebService1();
            ws.loginCompleted += Ws_loginCompleted;
            ws.loginAsync(username.Text, password.Text);

        }

        private void Ws_loginCompleted(object sender, webService.loginCompletedEventArgs e)
        {
            var msg = "données erronées!";
            if (e.Result.Rows.Count == 0)
            {

                var callDialog = new AlertDialog.Builder(this);
                callDialog.SetTitle("erreur");
                callDialog.SetMessage(msg);
                callDialog.SetNeutralButton("Ok", delegate {

                });
                callDialog.Show();
                //StartActivity(typeof(MainActivity));
            }
            else
            {
                Intent intent = new Intent(this, typeof(ProfilActivity));
                employer emp = new employer();
                foreach (DataRow data in e.Result.Rows)
                {
                    emp.Nom = data["NOM_EMP"].ToString();
                    emp.Prenom = data["PRENOM_EMP"].ToString();
                    emp.Pseudo = data["PSEUDO"].ToString();
                    emp.Tele = data["TEL_EMP"].ToString();
                    emp.Password = data["PASSWORD"].ToString();
                    emp.Ville = data["NOM_VILLEE"].ToString();
                    emp.Service = data["NOM_SERVICE"].ToString();
                    emp.Adresse = data["adresse"].ToString();


                    //emp.Ville = data["ville"].ToString();

                    //emp.Service = data["dispo"].ToString();

                }

                intent.PutExtra("nom", emp.Nom);
                intent.PutExtra("prenom", emp.Prenom);
                intent.PutExtra("pseudo", emp.Pseudo);
                intent.PutExtra("tele", emp.Tele);
                intent.PutExtra("password", emp.Password);
                //intent.PutExtra("dispo", emp.Dispo);
                intent.PutExtra("ville", emp.Ville);
                intent.PutExtra("service", emp.Service);
                intent.PutExtra("adresse", emp.Adresse);


                StartActivity(intent);
            }
        }

        private void Left_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(MainActivity));
        }
    }
}