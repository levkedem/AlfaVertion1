using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AlfaVertion1
{
    public class Interval0_Adapter: BaseAdapter<Interval>
    {
        Context context;
        List<Interval> intervals;
        public Interval0_Adapter(Context c,List<Interval> lst)
        {
            this.context = c;
            this.intervals = lst;
        }
        public override Java.Lang.Object GetItem(int position)
        {
            throw new NotImplementedException();
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override Interval this[int position]
        {
            get { return this.intervals[position]; }
        }
        public override int Count
        {
            get { return this.intervals.Count; }
        }

        public List<Interval> getList()
        {
            return this.intervals;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater layoutInflater1 = ((Construct_running_Activity)context).LayoutInflater;
            View view1 = layoutInflater1.Inflate(Resource.Layout.Running_Interval_listView_layout, parent, false);
            ImageView imageView1 = view1.FindViewById<ImageView>(Resource.Id.ivRunningSpeed);
            TextView tvLength1 = view1.FindViewById<TextView>(Resource.Id.tvLength);
            TextView tvPace1 = view1.FindViewById<TextView>(Resource.Id.tvPace);

                        
            Interval temp = intervals[position];
            if (temp != null)
            {

                Bitmap bitmap = null;
                if (temp.GetSpeed().Equals("fast"))
                {
                    bitmap = BitmapFactory.DecodeResource(this.context.Resources, Resource.Drawable.fast);
                }
                else if (temp.GetSpeed().Equals("med"))
                {
                    bitmap = BitmapFactory.DecodeResource(this.context.Resources, Resource.Drawable.steady);
                }
                else if (temp.GetSpeed().Equals("slow"))
                {
                    bitmap = BitmapFactory.DecodeResource(this.context.Resources, Resource.Drawable.slow);
                }
                else if (temp.GetSpeed().Equals("warm up"))
                {
                    bitmap = BitmapFactory.DecodeResource(this.context.Resources, Resource.Drawable.slow);
                }
                imageView1.SetImageBitmap(bitmap);
                tvPace1.Text = temp.GetSpeed();
                string s = ""+temp.GetAtrtribute() % 60;
                if (temp.GetAtrtribute() % 60<10)
                {
                    s = "0" + temp.GetAtrtribute() % 60;
                }
                if (temp.GetType1().Equals("time"))
                {
                    
                    tvLength1.Text = "" + temp.GetAtrtribute() / 60 + ":" + s;
                }
                else
                {
                    tvLength1.Text = "" + temp.GetAtrtribute()/1000.0;
                }
                

            }
            return view1;
        }
    }
}