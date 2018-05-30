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
    [Activity(Label = "ProfilActivity", Theme = "@android:style/Theme.Black.NoTitleBar")]
    public class ProfilActivity : Activity
    {
        EditText nom, prenom, service, ville, dispo, phone, psudo, adresse;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.profil);
            nom = FindViewById<EditText>(Resource.Id.txtNom);
            prenom = FindViewById<EditText>(Resource.Id.txtPrenom);
            service = FindViewById<EditText>(Resource.Id.txtService);
            ville = FindViewById<EditText>(Resource.Id.txtVillee);
            // dispo = FindViewById<EditText>(Resource.Id.txtDispo);
            phone = FindViewById<EditText>(Resource.Id.txtPhoone);
            //psudo = FindViewById<EditText>(Resource.Id.txtPsudo);
            adresse = FindViewById<EditText>(Resource.Id.etAdresse);

            nom.Text = Intent.GetStringExtra("nom");
            prenom.Text = Intent.GetStringExtra("prenom");
            //psudo.Text = Intent.GetStringExtra("pseudo");
            ville.Text = Intent.GetStringExtra("ville");
            //dispo.Text = Intent.GetStringExtra("dispo");
            phone.Text = Intent.GetStringExtra("tele");
            service.Text = Intent.GetStringExtra("service");
            adresse.Text = Intent.GetStringExtra("adresse");

            // Create your application here
        }
    }
}