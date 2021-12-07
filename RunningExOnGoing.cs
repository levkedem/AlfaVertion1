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

        int time;
        double currentDist;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.MasachOnGoing);

            tvTimer = (TextView)FindViewById(Resource.Id.tvTime);
            tvDistance = (TextView)FindViewById(Resource.Id.textViewDista);
            tvVelocity = (TextView)FindViewById(Resource.Id.textViewVelo);
            tvTimePerKm = (TextView)FindViewById(Resource.Id.textViewpace);

            loclist = new List<Location>();
            time = 0;            

            ThreadStart threadStart1 = new ThreadStart(GPSThreadManager);
            Thread thread1 = new Thread(threadStart1);
            thread1.Start();

            ThreadStart threadStart2 = new ThreadStart(DistanceThreadManager);
            Thread thread2 = new Thread(threadStart2);
            thread2.Start();

            ThreadStart threadStart3 = new ThreadStart(UpdateTime);
            Thread thread3 = new Thread(threadStart3);
            thread3.Start();
        }
        private void GPSThreadManager()//summon GetCurrentLocation every 15 sec
        {
            while (true)
            {
                GetCurrentLocation();
                Thread.Sleep(TimeSpan.FromSeconds(15));
            }
        }
        async Task GetCurrentLocation()//asks for Geolocation
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(30));
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
            while (true)
            {
                calcDis();
                calcVelocity();
                //calcPace();
                Thread.Sleep(TimeSpan.FromSeconds(6));
            }
        }
        public void calcDis()//calculate distance
        {
            double dist = 0;
            if (loclist.Count>3)
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
            DateTime time0 = loclist[0].Timestamp.UtcDateTime;
            DateTime timeI = loclist[loclist.Count-1].Timestamp.UtcDateTime;
            TimeSpan subTime = timeI.Subtract(time0);
            double hours = subTime.TotalHours;

            double velocity = this.currentDist / hours;
            string strVel = String.Format("{0:0.00}", velocity/1000);
            RunOnUiThread(() =>
            {
                tvVelocity.Text = strVel;
            });

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
            DateTime time0 = loclist[i2].Timestamp.UtcDateTime;
            DateTime timeI = loclist[loclist.Count - 1].Timestamp.UtcDateTime;
            TimeSpan subTime = timeI.Subtract(time0);
            double min = subTime.TotalMinutes;

            double partsToKm = 1000.0 / pacedis;
            double timeForKm = min * partsToKm;

            string strPace = String.Format("{0:0.00}", timeForKm);

            RunOnUiThread(() =>
            {
                tvTimePerKm.Text = strPace;
            });

        }
        public void UpdateTime()//do timer
        {
            while (true)
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
                this.time++;
                RunOnUiThread(() =>
                {
                    tvTimer.Text = m + ":" + s;
                });
                
                Thread.Sleep(1000);
            }
        }
        
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}