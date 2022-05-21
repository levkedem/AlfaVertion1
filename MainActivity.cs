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
using Xamarin.Android;
using Android.Content.PM;
using System.Net.NetworkInformation;
//using Android.Gms;
//using Android.Gms.Maps;

namespace AlfaVertion1
{   
    [Activity(Label = "Run For Your Life", Theme = "@style/AppTheme", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, Icon = "@drawable/runningicon")]
    public class MainActivity : AppCompatActivity
    {
        Button btMakeRunning, btMakeExercise, btRecentWorkouts;
        public static bool musicState = true;
        public static bool ShowEndingDialog;
        Dialog d;

        public static List<Exercise> allExerci;
        string path;

        public static Exercise theOneInUse { get; set; }

        public static ISharedPreferences userName;
        public static ISharedPreferences distanceInThisDvice;//in meters

        public bool isJustStarted;

        bool shouldBeDestroyed;
        public static bool isServiceAlive;//for music

        public static bool IsInternetGood = true;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            /*
            path = HelperClass.Path();
            var db = new SQLiteConnection(path);
            db.CreateTable<Exercise>();
            db.DeleteAll<Exercise>();

            //db.DeleteAll<Exercise>();
            */
            try
            {
                MainActivity.allExerci = await FirebaseHelper.GetAll();
            }
            catch
            {
                IsInternetGood = false;
            }

            userName= this.GetSharedPreferences("details", FileCreationMode.Private);
            distanceInThisDvice = this.GetSharedPreferences("details2", FileCreationMode.Private);

            this.btMakeRunning = (Button)FindViewById(Resource.Id.btrunning);
            //this.btMakeExercise = (Button)FindViewById(Resource.Id.btexe);
            this.btRecentWorkouts = (Button)FindViewById(Resource.Id.btoldStaff);

            btMakeRunning.Click += BtMakeRunning_Click; 
            //btMakeExercise.Click += BtMakeExercise_Click;
            btRecentWorkouts.Click += BtRecentWorkouts_Click;

            
            Intent intent = new Intent(this, typeof(MusicService));
            StartService(intent);
            
            MainActivity.isServiceAlive = true;
            
            
            MainActivity.ShowEndingDialog = false;

            theOneInUse = null;

            //var mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            //mapFragment.GetMapAsync(this);
            string uName = userName.GetString("UserName", "0");
            //Toast.MakeText(this, "" + userName.GetString("UserName", "0"), ToastLength.Long).Show();
            if (uName.Equals("0"))
            {
                uName = GetDeviceMacAddress();
                var editor = userName.Edit();
                editor.PutString("UserName", uName);
                editor.Commit();
            }

            isJustStarted = true;
            this.shouldBeDestroyed = false;
        }
        public static string GetDeviceMacAddress()
        {
            foreach (var netInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (netInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                    netInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    var address = netInterface.GetPhysicalAddress();
                    return BitConverter.ToString(address.GetAddressBytes());

                }
            }

            return "NoMac";
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu1, menu);

            IMenuItem item = menu.GetItem(0);
            //item.SetTitle("mute");
            
            if (musicState || isJustStarted)
            {
                item.SetTitle("mute");
                isJustStarted = false;
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
            if (IsInternetGood)
            {
                Intent i1 = new Intent(this, typeof(RecentWorkoutsActivity1));
                StartActivity(i1);
            }
            else
            {
                Toast.MakeText(this, "this feature is not active", ToastLength.Short).Show();
            }
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
        public void ResumeMusic() 
        {
            Intent i = new Intent("music");
            i.PutExtra("action", 1); // 1 is on
            SendBroadcast(i);
        }

        public void PauseMusic() 
        {
            Intent i = new Intent("music");
            i.PutExtra("action", 0); // 0 is off
            SendBroadcast(i);
        }
        public override void OnBackPressed()
        {
            
            if (this.shouldBeDestroyed)
            {
                //base.OnBackPressed();
                //base.OnPause();
                //base.OnDestroy();
                //Finish();
                //FinishAffinity();
                //System.Exit();
                //FinishAndRemoveTask();

            }
            else
            {
                //Toast.MakeText(this, "press again to exit", ToastLength.Short).Show();
                //this.shouldBeDestroyed = true;
            }
        }
        protected override void OnPause()
        {
            base.OnPause();
            PauseMusic();
        }
        public void ShowEndDialogFunc()
        {
            /*
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
            */            
        }
        
        protected override void OnResume()
        {
            base.OnResume();
            this.shouldBeDestroyed = false;
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