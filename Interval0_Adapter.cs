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

namespace AlfaVertion1
{
    public class Interval0_Adapter: BaseAdapter<Interval_v0>
    {
        Context context;
        List<Interval_v0> intervals;
        public Interval0_Adapter(Context c,List<Interval_v0> lst)
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
        public override Interval_v0 this[int position]
        {
            get { return this.intervals[position]; }
        }
        public override int Count
        {
            get { return this.intervals.Count; }
        }

        public List<Interval_v0> getList()
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

                        
            Interval_v0 temp = intervals[position];
            if (temp != null)
            {
                imageView1.SetImageBitmap(temp.GetBitmap());
                tvPace1.Text = temp.GetSpeed();
                string s = ""+temp.GetAtrtribute() % 60;
                if (temp.GetAtrtribute() % 60<10)
                {
                    s = "0" + temp.GetAtrtribute() % 60;
                }
                if (temp.GetType().Equals("time"))
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