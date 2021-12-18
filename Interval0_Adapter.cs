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
            throw new NotImplementedException();
        }
    }
}