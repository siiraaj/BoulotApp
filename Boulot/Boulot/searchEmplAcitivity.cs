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
    [Activity(Label = "searchEmpl", Theme = "@android:style/Theme.Black.NoTitleBar")]
    public class searchEmplAcitivity : Activity
    {
        string nomServ;
        List<employer> TableEmp;
        ListView listView;
        Spinner SpinnerVille;
        EditText searchNom;
        EditText inputSearch;


        private SearchView sv;
        private ArrayAdapter _adapter;


        float rr;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ChercherEmp);
            sv = FindViewById<SearchView>(Resource.Id.sv);


            /**********Remplire la liste des employer ***************/
            listView = FindViewById<ListView>(Resource.Id.listEmp);
            listView.ItemLongClick += ListView_ItemLongClick;
            list_Emp();




            /*******Recuperer le nom de service **************/
            nomServ = Intent.GetStringExtra("NomService");
            /*****Afficher le nom de service ********/
            this.Title = "Service : " + nomServ;

            /**********Remplire le spinner de ville*************/
            spinnerVille();
            SpinnerVille.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            /********** Afficher l'Id du Travailleur *********/
            listView.ItemClick += OnListItemClick;


        }

        private void ListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            var menu = new PopupMenu(this, (View)sender);
            menu.Inflate(Resource.Menu.popupmenu);
            menu.MenuItemClick += (s, a) =>
            {
                switch (a.Item.ItemId)
                {
                    case Resource.Id.review:
                        // update stuff
                        break;
                    case Resource.Id.pm_repport:
                        // update stuff
                        break;

                }
            };
            menu.Show();
            menu.MenuItemClick += Menu_MenuItemClick;
            var t = TableEmp[e.Position];
            Intent = new Intent(this, typeof(ReviewActivity));
            Intent.PutExtra("id_emp_review", t.Id.ToString());
            Console.WriteLine("safiii hna review ");
        }

        private void Menu_MenuItemClick(object sender, PopupMenu.MenuItemClickEventArgs e)
        {
            // Intent = new Intent(this, typeof(ReviewActivity));
            StartActivity(Intent);
        }

        private void ListView_ItemClick1(object sender, AdapterView.ItemClickEventArgs e)
        {
            //Toast.MakeText(this, _adapter.GetItem(e.Position).ToString(), ToastLength.Short).Show();
        }

        private void Sv_QueryTextChange(object sender, SearchView.QueryTextChangeEventArgs e)
        {


            _adapter.Filter.InvokeFilter(e.NewText);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var inflater = MenuInflater;
            inflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Intent intent = null;
            int id = item.ItemId;
            if (id == Resource.Id.search)
            {
                Toast.MakeText(this, "Search clicked", ToastLength.Short).Show();
                return true;
            }
            else if (id == Resource.Id.share)
            {
                Toast.MakeText(this, "Share clicked", ToastLength.Short).Show();
                return true;
            }
            else if (id == Resource.Id.email)
            {
                intent = new Intent(this, typeof(AproposActivity));
                StartActivity(intent);

                return true;

            }
            return base.OnOptionsItemSelected(item);
        }





        /**********Methode pour Remplire le spinner de ville*************/
        void spinnerVille()
        {
            SpinnerVille = FindViewById<Spinner>(Resource.Id.chercheVille);
            SpinnerVille.Prompt = "Choisissez Votre ville";
            List<string> listVille = new List<string>();
            listVille.Add("Marseille");
            listVille.Add("Toulon");
            listVille.Add("Montpelier");
            listVille.Add("Lyon");
            listVille.Add("Avignon");
            var AdapterVille = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, listVille);
            AdapterVille.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            SpinnerVille.Adapter = AdapterVille;

        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;

            string toast = string.Format("Votre ville est {0}", spinner.GetItemAtPosition(e.Position));
            Toast.MakeText(this, toast, ToastLength.Long).Show();
        }

        /**********methode pour Remplire la liste des employer ***************/
        void list_Emp()
        {


            webService.h21WebService1 ws = new webService.h21WebService1();
            int id = Int32.Parse(Intent.GetStringExtra("idService"));
            ws.getAllEmployesCompleted += Ws_getAllEmployesCompleted;
            ws.getAllEmployesAsync(id);
            ws.SCOREAsync(id);
            ws.SCORECompleted += Ws_SCORECompleted;
        }

        private void Ws_SCORECompleted(object sender, webService.SCORECompletedEventArgs e)
        {
            rr = 0;
            rr = e.Result;
        }

        private void Ws_getAllEmployesCompleted(object sender, webService.getAllEmployesCompletedEventArgs e)
        {

            TableEmp = new List<employer>();
            foreach (DataRow data in e.Result.Rows)
            {
                TableEmp.Add(new employer(Int32.Parse(data["ID_EMPLOYER"].ToString()),
                    data["NOM_EMP"].ToString(),
                    data["PRENOM_EMP"].ToString(),
                    data["NOM_VILLEE"].ToString(),
                    data["NOM_SERVICE"].ToString(),
                    data["TEL_EMP"].ToString(),
                    data["adresse"].ToString(),
                    data["IMAGE"].ToString(),
                    rr
                    ));

            }

            _adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, TableEmp);//2
            listView.Adapter = _adapter;//2



            listView.Adapter = new HomeScreenAdapterEmp(this, TableEmp);// 1
            listView.ItemClick += ListView_ItemClick;//1

            sv.QueryTextChange += Sv_QueryTextChange;

            listView.ItemClick += ListView_ItemClick1;



            // var _adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, TableEmp);



        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            int itemposition = e.Position;
            openDetaillesActivity(itemposition);
        }

        private void openDetaillesActivity(int pos)
        {
            Intent i = new Intent(this, typeof(DetaillsEmployeeActivity));
            i.PutExtra("nameKey", TableEmp[pos].Nom);
            i.PutExtra("prenomKey", TableEmp[pos].Prenom);
            i.PutExtra("villeKey", TableEmp[pos].Ville);
            i.PutExtra("ServiceKey", TableEmp[pos].Service);
            i.PutExtra("phoneKey", TableEmp[pos].Tele);
            i.PutExtra("imageKey", TableEmp[pos].ImageEmp);
            i.PutExtra("reviewKey", TableEmp[pos].Rating);

            this.StartActivity(i);
        }






        /********Methode pour afficher l'Id du Travailleur************/
        void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //var listView = sender as ListView;
            //var t = TableEmp[e.Position];
            //FindViewById<TextView>(Resource.Id.textTest).Text = t.id_Emp.ToString();
            var listView = sender as ListView;
            var t = TableEmp[e.Position];
            //Android.Widget.Toast.MakeText(this, t.Id.ToString(), Android.Widget.ToastLength.Short).Show();
            // Toast.MakeText(this,adapterr.GetItem(e.Position).Nom,ToastLength.Short).Show();
        }

    }
    // adpater manage



}