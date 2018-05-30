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
    [Activity(Label = "Inscription1Activity", Theme = "@android:style/Theme.Black.NoTitleBar")]
    public class Inscription1Activity : Activity
    {

        EditText nom, prenom, telephone, pseudo, password, confirmation, reponse, adresse;
        Spinner ville, service, Question;
        Button insciption;
        string question = "";
        int serviceSelected, villeSelected;

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Inscription1);
            nom = FindViewById<EditText>(Resource.Id.etNom);
            prenom = FindViewById<EditText>(Resource.Id.etPrenom);
            telephone = FindViewById<EditText>(Resource.Id.etTelephone);
            pseudo = FindViewById<EditText>(Resource.Id.etPseudo);
            password = FindViewById<EditText>(Resource.Id.etPassword);
            confirmation = FindViewById<EditText>(Resource.Id.etConfirmation);
            reponse = FindViewById<EditText>(Resource.Id.etReponse);
            adresse = FindViewById<EditText>(Resource.Id.etAdresse);
            ville = FindViewById<Spinner>(Resource.Id.spVille);
            service = FindViewById<Spinner>(Resource.Id.spService);
            Question = FindViewById<Spinner>(Resource.Id.spQuestion);
            insciption = FindViewById<Button>(Resource.Id.btnInscription);
            insciption.Click += Insciption_Click;
            spinnerQuestion();
            spinnerService();
            spinnerVille();
        }
        /********************************spinner question******************************************/
        void spinnerQuestion()
        {
            Question.Prompt = "Choisissez Votre ville";
            List<string> listQuestion = new List<string>();
            listQuestion.Add("Quelle est votre nourriture préférée ?");
            listQuestion.Add("Quel est votre numéro de carte de bibliothèque?");
            listQuestion.Add("Quel est le nom de votre meilleur ami(e)?");
            listQuestion.Add("Quel est le lieu de naissance de votre pere?");
            var AdapterQuestion = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, listQuestion);
            AdapterQuestion.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            Question.Adapter = AdapterQuestion;
            Question.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(QuestionSelected);

        }
        private void QuestionSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            question = spinner.GetItemAtPosition(e.Position).ToString();

        }
        /**********************************************************************************************/

        /************************************spinner services**************************************/
        void spinnerService()
        {
            webService.h21WebService1 ws = new webService.h21WebService1();
            ws.getAllServicesCompleted += Ws_getAllServicesCompleted;
            ws.getAllServicesAsync();

        }
        private void Ws_getAllServicesCompleted(object sender, webService.getAllServicesCompletedEventArgs e)
        {
            service.Prompt = "Choisissez Votre service";
            List<string> listServices = new List<string>();
            foreach (DataRow dt in e.Result.Rows)
            {
                listServices.Add(dt["NOM_SERVICE"].ToString());
            }
            var AdapterQuestion = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, listServices);
            AdapterQuestion.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            service.Adapter = AdapterQuestion;
            service.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(ServiceSelected);
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
            ville.Prompt = "Choisissez Votre ville";
            List<string> listVille = new List<string>();
            foreach (DataRow dt in e.Result.Rows)
            {
                listVille.Add(dt["NOM_VILLEE"].ToString());
            }
            var AdapterVille = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, listVille);
            AdapterVille.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            ville.Adapter = AdapterVille;
            ville.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(VilleSelected);
        }
        private void VilleSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            villeSelected = e.Position + 1;
        }
        /**********************************************************************************************/
        private void Insciption_Click(object sender, EventArgs e)
        {
            webService.h21WebService1 ws = new webService.h21WebService1();
            ws.inscriptionCompleted += Ws_inscriptionCompleted;
            ws.inscriptionAsync(villeSelected, nom.Text, prenom.Text,
                serviceSelected, telephone.Text, pseudo.Text, password.Text, confirmation.Text, question
               , reponse.Text, adresse.Text);
        }

        private void Ws_inscriptionCompleted(object sender, webService.inscriptionCompletedEventArgs e)
        {

            var callDialog = new AlertDialog.Builder(this);
            callDialog.SetTitle("Information");
            callDialog.SetMessage(e.Result);
            callDialog.SetNeutralButton("Ok", delegate {

            });
            callDialog.Show();
        }
    }
}