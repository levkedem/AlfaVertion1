using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System;
using System.Collections.Generic;
using Android.Content;
using Android.Views;
using SQLite;

namespace AlfaVertion1
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Button btMakeRunning, btMakeExercise, btRecentWorkouts;
        public static bool musicState;
        public static bool ShowEndingDialog;
        Dialog d;

        public static List<Exercise> allExerci { get; set; }
        string path;

        public static Exercise theOneInUse { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            path = HelperClass.Path();
            var db = new SQLiteConnection(path);
            db.CreateTable<Exercise>();
            //db.DeleteAll<Exercise>();
            MainActivity.allExerci = HelperClass.getAll();

            this.btMakeRunning = (Button)FindViewById(Resource.Id.btrunning);
            this.btMakeExercise = (Button)FindViewById(Resource.Id.btexe);
            this.btRecentWorkouts = (Button)FindViewById(Resource.Id.btoldStaff);

            btMakeRunning.Click += BtMakeRunning_Click; 
            btMakeExercise.Click += BtMakeExercise_Click;
            btRecentWorkouts.Click += BtRecentWorkouts_Click;

            Intent intent = new Intent(this, typeof(MyService));
            StartService(intent);
            MainActivity.musicState = true;
            MainActivity.ShowEndingDialog = false;

            theOneInUse = null;

        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu1, menu);

            IMenuItem item=menu.GetItem(0);
            if (musicState)
            {
                item.SetTitle("mute");
            }
            else
            {
                item.SetTitle("unmute");
            }
            return true;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            base.OnOptionsItemSelected(item);
            if (item.ItemId == Resource.Id.MuteOrUnMute)
            {
                
                musicState = !musicState;
                if (musicState)
                {
                    ResumeMusic();
                    item.SetTitle("mute");
                }
                else
                {
                    PauseMusic();
                    item.SetTitle("unmute");
                }
                return true;
            }
            
            return true;
        }

        private void BtRecentWorkouts_Click(object sender, EventArgs e)
        {
            Intent i1 = new Intent(this, typeof(RecentWorkoutsActivity1));
            StartActivity(i1);
        }

        private void BtMakeExercise_Click(object sender, EventArgs e)
        {
            Intent i1 = new Intent(this, typeof(RunningExOnGoing));
            StartActivity(i1);
        }

        private void BtMakeRunning_Click(object sender, EventArgs e)
        {
            Intent i1 = new Intent(this, typeof(Construct_running_Activity));
            StartActivity(i1);
        }
        public void ResumeMusic() // move to mainactivity
        {
            Intent i = new Intent("music");
            i.PutExtra("action", 1); // 1 to turn on
            SendBroadcast(i);
        }

        public void PauseMusic() // move to main
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
        public void ShowEndDialogFunc()
        {
            d = new Dialog(this);
            d.SetContentView(Resource.Layout.time_Dialog_Layout);
            d.SetTitle("exercise ended");
            d.SetCancelable(true);

            TextView tvExTime = (TextView)d.FindViewById(Resource.Id.tvTime);
            TextView tvExDis = (TextView)d.FindViewById(Resource.Id.tvDista);
            TextView tvExPace = (TextView)d.FindViewById(Resource.Id.tvPace);

            int timesec = Intent.GetIntExtra("time", 1);
            double idistance = Intent.GetDoubleExtra("distance", 1);
            tvExTime.Text = "time:" + timesec / 60 + ":" + timesec % 60;
            tvExDis.Text = "distance:" + (((int)idistance) / 100) / 10.0 + "km";
            tvExPace.Text = "pace:" + (idistance / 1000) / (timesec / 3600.0) + "km/h";
            d.Show();
        }
        protected override void OnResume()
        {
            base.OnResume();
            if (musicState)
                ResumeMusic();

            if (MainActivity.ShowEndingDialog)//if exercise ended
            {
                ShowEndDialogFunc();
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}