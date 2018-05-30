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
    [Activity(Label = "SearchActivity", Theme = "@android:style/Theme.Black.NoTitleBar")]
    public class SearchActivity : Activity
    {
        List<TableItem> tableItems;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            SetContentView(Resource.Layout.ServiceGridBackGround);

            //Definir le GridView
            GridView GridService = FindViewById<GridView>(Resource.Id.GridViewService);

            //Remplire le table de Item

            tableItems = new List<TableItem>();
            tableItems.Add(new TableItem(1, "Menuisier", Resource.Drawable.njar));
            tableItems.Add(new TableItem(2, "Macon", Resource.Drawable.bnay));
            tableItems.Add(new TableItem(3, "jardinier", Resource.Drawable.gardener));
            tableItems.Add(new TableItem(4, "Aluminium", Resource.Drawable.hadad));
            tableItems.Add(new TableItem(5, "Plombier", Resource.Drawable.Plumber));
            tableItems.Add(new TableItem(6, "Electricien", Resource.Drawable.tricien));
            tableItems.Add(new TableItem(7, "Teinturier", Resource.Drawable.sebagh));
            tableItems.Add(new TableItem(8, "Coiffeur", Resource.Drawable.halak));
            tableItems.Add(new TableItem(9, "Courtier", Resource.Drawable.semsar));
            tableItems.Add(new TableItem(10, "electro auto", Resource.Drawable.mecanic));
            tableItems.Add(new TableItem(11, "Mecanicien", Resource.Drawable.Icon));
            tableItems.Add(new TableItem(12, "gypse", Resource.Drawable.Icon));
            GridService.Adapter = new HomeScreenAdapter(this, tableItems);
            GridService.ItemClick += OnListItemClick;  // to be defined

        }

        // get item slected
        void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var listView = sender as ListView;
            var t = tableItems[e.Position];
            //Android.Widget.Toast.MakeText(this, t.Heading, Android.Widget.ToastLength.Short).Show();
            var NomServ = t.NomService;
            var IdServ = t.idService.ToString();
            Intent In = new Intent(this, typeof(searchEmplAcitivity));
            In.PutExtra("NomService", NomServ);
            In.PutExtra("idService", IdServ);
            StartActivity(In);
        }

    }

    //Classe Item
    public class TableItem
    {
        public int idService;
        public string NomService;

        public int ImgService;
        public TableItem(int idService, string NomService, int ImgService)
        {
            this.idService = idService;
            this.NomService = NomService;
            this.ImgService = ImgService;
        }
    }

    // adpater manage

}