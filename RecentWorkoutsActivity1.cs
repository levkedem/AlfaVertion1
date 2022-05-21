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
    [Activity(Label = "RecentWorkoutsActivity1")/*, Theme = "@style/AppTheme", MainLauncher = true)*/]
    public class RecentWorkoutsActivity1 : Activity
    {
        public string path;

        List<Exercise> privateEx, PublicEx;

        ListView privateExrciseLV, publicEcerciseLV;
        ExerciseAdapter1 adapter1, adapter2;


        protected override  void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Recent_workouts_layout);

            string mac = MainActivity.userName.GetString("UserName", "0");
            this.privateEx = GetExercisesOfUserGiven(MainActivity.allExerci, mac);
            this.PublicEx = GetPublicExercisesOfUserGiven(MainActivity.allExerci, mac);

            this.privateExrciseLV = (ListView)FindViewById(Resource.Id.lvRecent);
            
            adapter1 = new ExerciseAdapter1(this, privateEx);
            privateExrciseLV.Adapter = adapter1;

            this.privateExrciseLV.ItemClick += ListView_ItemClick;
            this.privateExrciseLV.ItemLongClick += ListView_ItemLongClick;


            this.publicEcerciseLV = (ListView)FindViewById(Resource.Id.lvPublicWorkouts);

            adapter2 = new ExerciseAdapter1(this, PublicEx);
            publicEcerciseLV.Adapter = adapter2;

            this.publicEcerciseLV.ItemClick += PublicEcerciseLV_ItemClick;
            // Create your application here
        }

        

        public List<Exercise> GetExercisesOfUserGiven(List<Exercise> lst,string mac)
        {
            List<Exercise> nlst = new List<Exercise>();

            foreach (Exercise ex in lst)
            {
                if (ex.user.Equals(mac))
                {
                    nlst.Add(ex);
                }
            }
            return nlst;
        }
        public List<Exercise> GetPublicExercisesOfUserGiven(List<Exercise> lst, string mac)
        {
            List<Exercise> nlst = new List<Exercise>();

            foreach (Exercise ex in lst)
            {
                if (!(ex.user.Equals(mac))&& ex.isPublic)
                {
                    nlst.Add(ex);
                }
            }
            return nlst;
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            if (e.Position != null)
            {
                MainActivity.theOneInUse = this.privateEx[e.Position];
            }
            Intent i1 = new Intent(this, typeof(RunningExOnGoing));
            StartActivity(i1);
        }

        private void ListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            if (e.Position != null)
            {
                FirebaseHelper.Delete2(this.privateEx[e.Position].date);//add awaut


                DeleteExFromList(this.privateEx[e.Position]);
                this.privateEx.RemoveAt(e.Position);
                //MainActivity.allExerci.RemoveAt(e.Position);
                adapter1.NotifyDataSetChanged();
            }
        }
        public void DeleteExFromList(Exercise ex)
        {
            for (int i = 0; i < MainActivity.allExerci.Count; i++)
            {
                if (MainActivity.allExerci[i]==ex)
                {
                    MainActivity.allExerci.RemoveAt(i);
                }
            }
        }
        
        private void PublicEcerciseLV_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            if (e.Position != null)
            {
                MainActivity.theOneInUse = this.PublicEx[e.Position];
            }
            Intent i1 = new Intent(this, typeof(RunningExOnGoing));
            StartActivity(i1);
        }
        public void ResumeMusic()
        {
            Intent i = new Intent("music");
            i.PutExtra("action", 1); // 1 to turn on
            SendBroadcast(i);
        }

        public void PauseMusic()
        {
            Intent i = new Intent("music");
            i.PutExtra("action", 0); // 0 to turn on
            SendBroadcast(i);
        }

        protected override void OnPause()
        {
            base.OnPause();
            PauseMusic();
        }
        protected override void OnResume()
        {
            base.OnResume();
            if (MainActivity.musicState)
                ResumeMusic();

           
        }
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            //Intent i1 = new Intent(this, typeof(MainActivity));
            //StartActivity(i1);

        }
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            adapter1.NotifyDataSetChanged();

        }
    }
}