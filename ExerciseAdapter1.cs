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
    class ExerciseAdapter1: BaseAdapter<Exercise>
    {
        Context context;
        List<Exercise> objects;

        public ExerciseAdapter1(Context con, List<Exercise> lst)
        {
            this.context = con;
            this.objects = lst;
        }
        public override int Count
        {
            get { return this.objects.Count; }
        }

        public override Exercise this[int position]
        {
            get { return this.objects[position - 1]; }
        }
        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater layoutInflater = ((RecentWorkoutsActivity1)context).LayoutInflater;
            View view = layoutInflater.Inflate(Resource.Layout.Workout_listview_layout, parent, false);
            TextView name = view.FindViewById<TextView>(Resource.Id.tvName);
            TextView date = view.FindViewById<TextView>(Resource.Id.tvDate);            
            ImageView picture = view.FindViewById<ImageView>(Resource.Id.imageType);

            Exercise s = objects[position];
            if (s != null)
            {               
                name.Text = s.name;
                date.Text = s.date.ToString().Substring(0,9);               
            }
            return view;
        }
    }
}