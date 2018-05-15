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


    [Activity(Label = "ProfilActivity", Theme = "@android:style/Theme.Black.NoTitleBar")]
    public class ProfilActivity : Activity
    {
        EditText nom, prenom, password, phone, psudo;
        Spinner spville, spservice;
        Button update;
        int serviceSelected, villeSelected, id_emp;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.profil);
            nom = FindViewById<EditText>(Resource.Id.txtNom);
            prenom = FindViewById<EditText>(Resource.Id.txtPrenom);
            spville = FindViewById<Spinner>(Resource.Id.spVille1);
            spservice = FindViewById<Spinner>(Resource.Id.spService1);
            psudo = FindViewById<EditText>(Resource.Id.txtPseeudo);
            password = FindViewById<EditText>(Resource.Id.txtPass);
            phone = FindViewById<EditText>(Resource.Id.txtPhoone);
            update = FindViewById<Button>(Resource.Id.envoyer);


            nom.Text = Intent.GetStringExtra("nom");
            prenom.Text = Intent.GetStringExtra("prenom");
            phone.Text = Intent.GetStringExtra("tele");
            psudo.Text = Intent.GetStringExtra("pseudo");
            id_emp = Intent.GetIntExtra("id_emp", 0);
            // Create your application here
            spinnerService();
            spinnerVille();
            update.Click += Update_Click;
        }

        private void Update_Click(object sender, EventArgs e)
        {
            webService.h21WebService1 ws = new webService.h21WebService1();
            ws.updateProfileCompleted += Ws_updateProfileCompleted;
            ws.updateProfileAsync(id_emp, villeSelected, nom.Text, psudo.Text, prenom.Text, serviceSelected, phone.Text, password.Text);
        }

        private void Ws_updateProfileCompleted(object sender, webService.updateProfileCompletedEventArgs e)
        {
            if ("succes".Equals(e.Result))
            {
                var callDialog = new AlertDialog.Builder(this);
                callDialog.SetTitle("Notification");
                callDialog.SetMessage("Votre modification est bien effectuer merci de se connecter avec des nouveaux donnees !! ");
                callDialog.SetNeutralButton("Ok", delegate
                {

                });
                callDialog.Show();

            }
            else
            {
                var callDialog = new AlertDialog.Builder(this);
                callDialog.SetTitle("erreur");
                callDialog.SetMessage(e.Result);
                callDialog.SetNeutralButton("Ok", delegate
                {

                });
                callDialog.Show();
            }
        }

        /************************************spinner services**************************************/
        void spinnerService()
        {
            webService.h21WebService1 ws = new webService.h21WebService1();
            ws.getAllServicesCompleted += Ws_getAllServicesCompleted;
            ws.getAllServicesAsync();

        }
        private void Ws_getAllServicesCompleted(object sender, webService.getAllServicesCompletedEventArgs e)
        {
            spservice.Prompt = "Choisissez Votre service";
            List<string> listServices = new List<string>();
            foreach (DataRow dt in e.Result.Rows)
            {
                listServices.Add(dt["NOM_SERVICE"].ToString());
            }
            var AdapterQuestion = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, listServices);
            AdapterQuestion.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spservice.Adapter = AdapterQuestion;
            spservice.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(ServiceSelected);
        }
        private void ServiceSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            serviceSelected = e.Position + 1;

        }
        /*******************************************************************************************/
        /************************************spinner villes**************************************/

        void spinnerVille()
        {
            webService.h21WebService1 ws = new webService.h21WebService1();
            ws.getAllVillesCompleted += Ws_getAllVillesCompleted;
            ws.getAllVillesAsync();

        }

        private void Ws_getAllVillesCompleted(object sender, webService.getAllVillesCompletedEventArgs e)
        {
            spville.Prompt = "Choisissez Votre ville";
            List<string> listVille = new List<string>();
            foreach (DataRow dt in e.Result.Rows)
            {
                listVille.Add(dt["NOM_VILLEE"].ToString());
            }
            var AdapterVille = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, listVille);
            AdapterVille.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spville.Adapter = AdapterVille;
            spville.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(VilleSelected);
        }
        private void VilleSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            villeSelected = e.Position + 1;
        }
        /**********************************************************************************************/
    }
}