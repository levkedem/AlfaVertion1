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
using SQLite;

namespace AlfaVertion1
{
    [Activity(Label = "RecentWorkoutsActivity1")/*, Theme = "@style/AppTheme", MainLauncher = true)*/]
    public class RecentWorkoutsActivity1 : Activity
    {
        public string path;
        

        ListView listView;
        ExerciseAdapter1 adapter1;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Recent_workouts_layout);

            path = HelperClass.Path();
            
            //db.CreateTable<Exercise>();            

            this.listView = (ListView)FindViewById(Resource.Id.lvRecent);

            adapter1 = new ExerciseAdapter1(this, MainActivity.allExerci);
            listView.Adapter = adapter1;

            this.listView.ItemClick += ListView_ItemClick;
            this.listView.ItemLongClick += ListView_ItemLongClick;

            // Create your application here
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            if (e.Position != null)
            {
                MainActivity.theOneInUse = MainActivity.allExerci[e.Position];
            }
            Intent i1 = new Intent(this, typeof(RunningExOnGoing));
            StartActivity(i1);
        }

        private void ListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            if (e.Position != null)
            {
                var db = new SQLiteConnection(path);
                db.Delete(MainActivity.allExerci[e.Position]);
                MainActivity.allExerci.RemoveAt(e.Position);
                adapter1.NotifyDataSetChanged();
            }
        }

        

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            adapter1.NotifyDataSetChanged();

        }
    }
}