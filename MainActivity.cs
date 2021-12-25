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

namespace AlfaVertion1
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Button btMakeRunning, btMakeExercise, btRecentWorkouts;
        
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

            Intent intent = new Intent(this, typeof(MusicService));
            StartService(intent);

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

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}