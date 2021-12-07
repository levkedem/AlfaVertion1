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
        CancellationTokenSource cts;
        TextView lat, lon, alt, tvdis;
        Button btn1;

        List<Location> loclist;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            this.loclist = new List<Location>();

            lat = (TextView)FindViewById(Resource.Id.textViewlat);
            lon = (TextView)FindViewById(Resource.Id.textViewlon);
            alt = (TextView)FindViewById(Resource.Id.textViewalt);
            tvdis = (TextView)FindViewById(Resource.Id.textViewDis);
            btn1 = (Button)FindViewById(Resource.Id.buttonGoo);


            btn1.Click += Btn1_Click;
        }

        private void Btn1_Click(object sender, EventArgs e)
        {
            /*
            ThreadStart threadStart1 = new ThreadStart(GPSThreadManager);
            Thread thread1 = new Thread(threadStart1);
            thread1.Start();

            ThreadStart threadStart2 = new ThreadStart(DistanceThreadManager);
            Thread thread2 = new Thread(threadStart2);
            thread2.Start();

            Toast.MakeText(this, "btn", ToastLength.Short).Show();
            */
            Intent intent1 = new Intent(this, typeof(RunningExOnGoing));
            StartActivity(intent1);
        }

        private void GPSThreadManager()
        {
            while (true)
            {                
                GetCurrentLocation();
                Thread.Sleep(TimeSpan.FromSeconds(15));
            }
        }
        private void DistanceThreadManager()
        {
            while (true)
            {
                calcDis();
                Thread.Sleep(TimeSpan.FromSeconds(6));
            }
        }
        public void calcDis()
        {
            double dist = 0;
            for (int i = 0; i < loclist.Count-1; i++)
            {
                dist = dist + 1000*Location.CalculateDistance(loclist[i], loclist[i + 1], DistanceUnits.Kilometers);
            }
            RunOnUiThread(() =>
            {
                tvdis.Text = "distance: " + dist;
            });
        }

        async Task GetCurrentLocation()//asks for Geolocation
        {
            try
            {                                
                var request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(30));
                cts = new CancellationTokenSource();
                var location1 = await Geolocation.GetLocationAsync(request, cts.Token);
                

                if (location1 != null)
                {
                    loclist.Add(location1);
                    RunOnUiThread(() =>
                    {
                        //Toast.MakeText(this, "have loc", ToastLength.Short).Show();
                    });
                    RunOnUiThread(() =>
                    {
                        lat.Text = "Latitude" + location1.Latitude;
                        lon.Text = "Longitude" + location1.Longitude;
                        alt.Text = "Altitude" + location1.Altitude;                        
                    });                    
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
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}