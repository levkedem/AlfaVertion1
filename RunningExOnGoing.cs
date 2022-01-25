using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Essentials;

namespace AlfaVertion1
{
    [Activity(Label = "RunningExOnGoing")]
    public class RunningExOnGoing : Activity
    {
        TextView tvTimer, tvDistance, tvVelocity, tvTimePerKm, tvIntervalTime;

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

        Thread thread1, thread2, thread3;

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
            tvTimePerKm = (TextView)FindViewById(Resource.Id.textViewpace);
            tvIntervalTime = (TextView)FindViewById(Resource.Id.tvTimeForInterval);

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
            

            ThreadStart threadStart1 = new ThreadStart(GPSThreadManager);
            thread1 = new Thread(threadStart1);
            thread1.Start();

            ThreadStart threadStart2 = new ThreadStart(DistanceThreadManager);
            thread2 = new Thread(threadStart2);
            thread2.Start();

            ThreadStart threadStart3 = new ThreadStart(UpdateTime);
            thread3 = new Thread(threadStart3);
            thread3.Start();


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
                GetCurrentLocation();
                Thread.Sleep(TimeSpan.FromSeconds(15));
            }
        }
        async Task GetCurrentLocation()//asks for Geolocation
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Low, TimeSpan.FromSeconds(30));
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
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }
        private void DistanceThreadManager()//summon calcDis and calcVelocity every 6 sec
        {
            Thread.Sleep(TimeSpan.FromSeconds(30));
            while (ExerciseState)
            {
                calcDis();
                calcVelocity();
                calcPace();
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
        public void calcPace()//calculate current pace
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

        }
        public void UpdateTime()//do timer
        {
            while (ExerciseState)
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
                string whatatoShow="eeeeeerrrr";
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
                            this.exerciseInUse.Next();
                            whatatoShow = SetNewInterval(curInterval);
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
                            this.exerciseInUse.Next();
                            whatatoShow = SetNewInterval(curInterval);
                        }
                    }
                }
                this.time++;

                RunOnUiThread(() =>
                {
                    tvTimer.Text = m + ":" + s;
                    this.tvIntervalTime.Text = whatatoShow;
                });

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

        protected override void OnResume()
        {
            base.OnResume();
            RegisterReceiver(broadCastBattery, new IntentFilter(Intent.ActionBatteryChanged));
        }
        protected override void OnPause()
        {
            base.OnPause();
            UnregisterReceiver(broadCastBattery);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}