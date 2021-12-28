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

namespace AlfaVertion1
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Button btMakeRunning, btMakeExercise, btRecentWorkouts;
        public static bool musicState;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            this.btMakeRunning = (Button)FindViewById(Resource.Id.btrunning);
            this.btMakeExercise = (Button)FindViewById(Resource.Id.btexe);
            this.btRecentWorkouts = (Button)FindViewById(Resource.Id.btoldStaff);

            btMakeRunning.Click += BtMakeRunning_Click; 
            btMakeExercise.Click += BtMakeExercise_Click;
            btRecentWorkouts.Click += BtRecentWorkouts_Click;

            Intent intent = new Intent(this, typeof(MyService));
            StartService(intent);
            MainActivity.musicState = true;

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
            throw new NotImplementedException();
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
        protected override void OnResume()
        {
            base.OnResume();
            if (musicState)
                ResumeMusic();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}