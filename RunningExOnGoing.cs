using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Hardware;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Essentials;


namespace AlfaVertion1
{
    [Activity(Label = "RunningExOnGoing", ScreenOrientation = ScreenOrientation.Portrait)]
    public class RunningExOnGoing : Activity,  ISensorEventListener
    {
        TextView tvTimer, tvDistance, tvVelocity, tvTimePerKm, tvIntervalTime;
        ImageView icon;
        Bitmap bitPouse, bitPlay;

        CancellationTokenSource cts;
        List<Location> loclist;

        int time;//
        double currentDist;

        Exercise exerciseInUse;
        int intervalTime;//time sec
        int intervalDis;// dis m
        bool needsNewInterval;

        int timeWhenIntervalStarted;
        int indexOfLocationInListWhenIntervalStarted;

        BroadcastBattery broadCastBattery;
        AlertDialog.Builder builder;


        bool ExerciseState;
        bool isPoused;

        Thread thread1, thread2, thread3;

        SensorManager sensorManager;
        Sensor temprSensor;
        bool checkLight;
        AlertDialog.Builder builder2;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.MasachOnGoing);

            Intent intent = new Intent(this, typeof(MyService));
            StopService(intent);

            

            tvTimer = (TextView)FindViewById(Resource.Id.tvTime);
            tvDistance = (TextView)FindViewById(Resource.Id.textViewDista);
            tvVelocity = (TextView)FindViewById(Resource.Id.textViewVelo);
            //tvTimePerKm = (TextView)FindViewById(Resource.Id.textViewpace);
            tvIntervalTime = (TextView)FindViewById(Resource.Id.tvTimeForInterval);
            icon = (ImageView)FindViewById(Resource.Id.ivIconImage);

            bitPouse= BitmapFactory.DecodeResource(Resources, Resource.Drawable.redpouse);
            bitPlay = BitmapFactory.DecodeResource(Resources, Resource.Drawable.blueplay2);

            icon.SetImageBitmap(bitPouse);

            loclist = new List<Location>();
            time = 0;
            needsNewInterval = true;
            this.intervalTime = 0;
            this.intervalDis = 0;

            if (MainActivity.theOneInUse!=null)
            {
                this.exerciseInUse = MainActivity.theOneInUse;
            }
            else
            {
                int num = MainActivity.allExerci.Count;
                this.exerciseInUse = MainActivity.allExerci[num-1];
            }
            //this.exerciseInUse.
            this.exerciseInUse.StartEx();
                                 

            
            builder = new AlertDialog.Builder(this);
            builder.SetTitle("your battery is very low");
            builder.SetMessage("charge your phone to start training");
            builder.SetCancelable(false);
            builder.SetPositiveButton("back to menu", OkAction);                
            AlertDialog d2 = builder.Create();
            broadCastBattery = new BroadcastBattery(d2);

            this.exerciseInUse.theEnd += EndExercise;
            this.ExerciseState = true;
            this.isPoused = false;

            icon.Click += Icon_Click;

            ThreadStart threadStart1 = new ThreadStart(GPSThreadManager);
            thread1 = new Thread(threadStart1);
            thread1.Start();

            ThreadStart threadStart2 = new ThreadStart(DistanceThreadManager);
            thread2 = new Thread(threadStart2);
            thread2.Start();

            ThreadStart threadStart3 = new ThreadStart(UpdateTime);
            thread3 = new Thread(threadStart3);
            thread3.Start();

            sensorManager = (SensorManager)GetSystemService(Context.SensorService);

            temprSensor = sensorManager.GetDefaultSensor(SensorType.Light);
            checkLight = false;
            

        }
        private void Icon_Click(object sender, EventArgs e)
        {
            if (!isPoused)
            {
                isPoused = true;
                icon.SetImageBitmap(bitPlay);
            }
            else
            {
                isPoused = false;
                icon.SetImageBitmap(bitPouse);
            }
        }
        public void EndExercise(object s,int n)
        {
            Intent.PutExtra("time", this.time);
            Intent.PutExtra("distance", this.currentDist);
            MainActivity.ShowEndingDialog = true;

            this.exerciseInUse.timeForThisEx = this.time;
            this.exerciseInUse.distanceForThisExKM = ((int)this.currentDist / 10) / 100.0;

            ExerciseState = false;            
            Intent i1 = new Intent(this, typeof(MainActivity));
            StartActivity(i1);

        }
        private void OkAction(object sender, DialogClickEventArgs e)
        {
            Intent i1 = new Intent(this, typeof(MainActivity));
            StartActivity(i1);
        }
        private void GPSThreadManager()//summon GetCurrentLocation every 15 sec
        {
            while (ExerciseState)
            {
                if (!isPoused)
                {
                    GetCurrentLocation();
                }
                
                Thread.Sleep(TimeSpan.FromSeconds(15));
            }
        }
        async Task GetCurrentLocation()//asks for Geolocation
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(30));
                cts = new CancellationTokenSource();
                Location location1 = await Geolocation.GetLocationAsync(request, cts.Token);


                if (location1 != null)
                {
                    loclist.Add(location1);
                }
                else
                {
                    Toast.MakeText(this, "no loc", ToastLength.Short).Show();
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                Toast.MakeText(this, "please enable your device GPS", ToastLength.Long).Show();
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }
        private void DistanceThreadManager()//summon calcDis and calcVelocity every 6 sec
        {
            Thread.Sleep(TimeSpan.FromSeconds(15));
            while (ExerciseState)
            {
                if (!isPoused)
                {
                    calcDis();
                    calcVelocity();
                    //calcPace();
                }
                Thread.Sleep(TimeSpan.FromSeconds(6));
            }
        }
        public void calcDis()//calculate distance
        {
            double dist = 0;
            if (loclist.Count>1)
            {
                for (int i = 0; i < loclist.Count - 1; i++)
                {
                    double tempDis = 1000 * Location.CalculateDistance(loclist[i], loclist[i + 1], DistanceUnits.Kilometers);
                    if (tempDis > 8)
                    {
                        dist = dist + tempDis;
                    }
                }
                this.currentDist = dist;
                string strdistance = String.Format("{0:0.00}", dist);
                RunOnUiThread(() =>
                {
                    tvDistance.Text = strdistance;
                });
            }
        }
        public void calcVelocity()//calculate velocity
        {
            if (loclist.Count > 1)
            {
                DateTime time0 = loclist[0].Timestamp.UtcDateTime;
                DateTime timeI = loclist[loclist.Count - 1].Timestamp.UtcDateTime;
                TimeSpan subTime = timeI.Subtract(time0);
                double hours = subTime.TotalHours;

                double velocity = this.currentDist / hours;
                string strVel = String.Format("{0:0.00}", velocity / 1000);
                RunOnUiThread(() =>
                {
                    tvVelocity.Text = strVel;
                });
            }

        }
        /* public void calcPace()//calculate current pace
        {
            int i2=0;
            double pacedis=0;
            if (loclist.Count>=3)
            {
                i2 = loclist.Count - 3;
            }
            else if (loclist.Count==2)
            {
                i2 = loclist.Count - 2;
            }
            for (; i2 < loclist.Count-1; i2++)
            {
                double tempDis = 1000 * Location.CalculateDistance(loclist[i2], loclist[i2 + 1], DistanceUnits.Kilometers);
                if (tempDis > 8)
                {
                    pacedis = pacedis + tempDis;
                }
            }
            if (loclist.Count > 1)
            {
                DateTime time0 = loclist[i2].Timestamp.UtcDateTime;
                DateTime timeI = loclist[loclist.Count - 1].Timestamp.UtcDateTime;
                TimeSpan subTime = timeI.Subtract(time0);
                double min = subTime.TotalMinutes;

                double partsToKm = 1000.0 / pacedis;
                double timeForKm = min * partsToKm;

                string strPace = "" + timeForKm / 1 + ":" + ((timeForKm % 1) * 60) / 1;

                RunOnUiThread(() =>
                {
                    tvTimePerKm.Text = strPace;
                });
            }

        }*/
        public void UpdateTime()//do timer
        {
            while (ExerciseState)
            {
                if (!isPoused)
                {


                    int min = this.time / 60;
                    int sec = this.time - min * 60;
                    string s, m;
                    if (min < 10)
                    {
                        m = "0" + min;
                    }
                    else
                    {
                        m = "" + min;
                    }
                    if (sec < 10)
                    {
                        s = "0" + sec;
                    }
                    else
                    {
                        s = "" + sec;
                    }

                    Interval_v0 curInterval = exerciseInUse.GetCurrentInterval();
                    string whatatoShow = "eeeeeerrrr";
                    if (needsNewInterval)
                    {
                        whatatoShow = SetNewInterval(curInterval);
                    }
                    else
                    {
                        if (curInterval.GetType().Equals("time"))
                        {

                            int numOfSec = (this.timeWhenIntervalStarted + this.intervalTime) - this.time;
                            if (numOfSec > 0)
                            {
                                whatatoShow = TimeSpan.FromSeconds(numOfSec).ToString();
                            }
                            else
                            {
                                //Toast.MakeText(this, "time ended", ToastLength.Short).Show();
                                this.exerciseInUse.Next();
                                needsNewInterval = true;
                                //whatatoShow = SetNewInterval(curInterval);
                            }
                        }
                        else if (curInterval.GetType().Equals("dis"))
                        {
                            int disFormIntervalStart = CalcSpecificDist(this.indexOfLocationInListWhenIntervalStarted);
                            if (this.intervalDis - disFormIntervalStart > 0)
                            {
                                whatatoShow = "" + (this.intervalDis - disFormIntervalStart);
                            }
                            else
                            {
                                //Toast.MakeText(this, "dis ended", ToastLength.Short).Show();
                                this.exerciseInUse.Next();
                                needsNewInterval = true;
                                //whatatoShow = SetNewInterval(curInterval);
                            }
                        }
                    }
                    this.time++;

                    RunOnUiThread(() =>
                    {
                        tvTimer.Text = m + ":" + s;
                        this.tvIntervalTime.Text = whatatoShow;
                    });
                }
                Thread.Sleep(1000);
            }
        }
        public int CalcSpecificDist(int startPoint)//calc distance from specific time
        {
            double dist = 0;
            if (loclist.Count > 1)
            {
                for (int i = startPoint; i < loclist.Count - 1; i++)
                {
                    if (i + 1 < this.loclist.Count - 1)
                    {
                        double tempDis = 1000 * Location.CalculateDistance(loclist[i], loclist[i + 1], DistanceUnits.Kilometers);
                        if (tempDis > 8)
                        {
                            dist = dist + tempDis;
                        }
                    }
                }

            }
            return (int)dist;
        }
        public string SetNewInterval(Interval_v0 curInterval)
        {
            string str="";
            //Vibration.Vibrate(TimeSpan.FromMilliseconds(700));
            if (curInterval.GetType().Equals("time"))
            {
                this.intervalTime = curInterval.GetAtrtribute();
                this.intervalDis = 0;
                this.timeWhenIntervalStarted = this.time;
                str = TimeSpan.FromSeconds(intervalTime).ToString();
                needsNewInterval = false;
            }
            else if (curInterval.GetType().Equals("dis"))
            {
                this.intervalTime = 0;
                this.intervalDis = curInterval.GetAtrtribute();
                int ind = this.loclist.Count - 1;
                if (ind<0)
                {
                    ind = 0;
                }
                this.indexOfLocationInListWhenIntervalStarted = ind;
                str = "" + this.intervalDis;
                needsNewInterval = false;
            }
            return str;

        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {

        }

        public void OnSensorChanged(SensorEvent e)
        {
            if (!checkLight)
            {
                //Toast.MakeText(this, "" + e.Values[0], ToastLength.Short).Show();
                if (e.Values[0]<10)
                {
                    isPoused = true;

                    builder = new AlertDialog.Builder(this);
                    builder.SetTitle("It is dark outside");
                    builder.SetMessage("make sure you are visible");
                    builder.SetCancelable(false);
                    builder.SetPositiveButton("back to menu", OkAction2);
                    builder.SetNegativeButton("keep running", NoAction);
                    AlertDialog d3 = builder.Create();
                    d3.Show();
                    checkLight = true;
                }
            }
            

        }
        private void OkAction2(object sender, DialogClickEventArgs e)
        {
            Intent i1 = new Intent(this, typeof(MainActivity));
            StartActivity(i1);
        }
        private void NoAction(object sender, DialogClickEventArgs e)
        {
            isPoused = false;
        }
        protected override void OnResume()
        {
            base.OnResume();
            RegisterReceiver(broadCastBattery, new IntentFilter(Intent.ActionBatteryChanged));
            if (!checkLight)
            {
                sensorManager.RegisterListener(this, sensorManager.GetDefaultSensor(SensorType.Light), SensorDelay.Ui);
            }
        }
        protected override void OnPause()
        {
            base.OnPause();
            UnregisterReceiver(broadCastBattery);
            sensorManager.UnregisterListener(this);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}