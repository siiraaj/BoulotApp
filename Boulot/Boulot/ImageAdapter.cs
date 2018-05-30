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
using System.Data;

namespace Boulot
{
    class ImageAdapter : BaseAdapter
    {
        Context context;
        int[] t = {
            Resource.Drawable.t1,Resource.Drawable.t2,Resource.Drawable.t3,Resource.Drawable.t4
        };
        public ImageAdapter(Context c)
        {
            context = c;
        }
        public override int Count
        {
            get
            {
                return t.Length;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return 0;

        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ImageView i = new ImageView(context);
            i.SetImageResource(t[position]);
            i.LayoutParameters = new Gallery.LayoutParams(350, 300);
            i.SetScaleType(ImageView.ScaleType.FitXy);
            return i;
        }

    }
}