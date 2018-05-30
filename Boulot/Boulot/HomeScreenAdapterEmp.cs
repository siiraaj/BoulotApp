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
using Square.Picasso;

namespace Boulot
{
    class HomeScreenAdapterEmp : BaseAdapter<employer>
    {
        List<employer> items;
        Activity context;
        int a;

        public HomeScreenAdapterEmp(Activity context, List<employer> items)
            : base()
        {
            this.context = context;
            this.items = items;
            //this.A = c;
        }

        public override long GetItemId(int position)
        {
            return position;
        }
        public override employer this[int position]
        {
            get { return items[position]; }
        }
        public override int Count
        {
            get { return items.Count; }
        }

        public int A
        {
            get
            {
                return a;
            }

            set
            {
                a = value;
            }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;
            if (view == null) // no view to re-use, create new
                view = context.LayoutInflater.Inflate(Resource.Layout.listEmp, null);
            view.FindViewById<TextView>(Resource.Id.NomEmp).Text = item.Nom;
            view.FindViewById<TextView>(Resource.Id.PreEmp).Text = item.Prenom;
            view.FindViewById<RatingBar>(Resource.Id.ratingEmp).Rating = item.Rating;
            Picasso.With(context)
             .Load(item.ImageEmp)
             .Into(view.FindViewById<ImageView>(Resource.Id.imgEmp));
            return view;
        }
    }

}
