using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AlfaVertion1
{
    [Activity(Label = "Make_running_Interval_Activity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Make_running_Interval_Activity : Activity
    {

        RadioGroup rgPace, rgType;
        RadioButton rbFast, rbMed, rbSlow, rbDist, rbTime;
        TextView typeCounter;
        double dis;
        TimeSpan time;
        Button save;

        Dialog d;

        NumberPicker kmPicker, meterPicker;
        NumberPicker hoursPicker, minPicker,secPicker;
        string[] metersArr;

        Button saveDialog;

        int witchDialog;
        string speed;
        Interval interval0;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.make_running_interval_layout);

            time = TimeSpan.Zero;
            dis = 0;
            interval0 = null;

            typeCounter = (TextView)FindViewById(Resource.Id.tvTypeShow);
            save = (Button)FindViewById(Resource.Id.btSaveInterval);
            rgPace = (RadioGroup)FindViewById(Resource.Id.rgPace1);
            rbFast = (RadioButton)FindViewById(Resource.Id.rbFast);
            rbMed = (RadioButton)FindViewById(Resource.Id.rbMed1);
            rbSlow = (RadioButton)FindViewById(Resource.Id.rbSlow1);

            rgType = (RadioGroup)FindViewById(Resource.Id.rgType);
            rbDist = (RadioButton)FindViewById(Resource.Id.rbDis1);
            rbTime = (RadioButton)FindViewById(Resource.Id.rbTime1);

            metersArr =new string[19];
            for (int i = 0; i < metersArr.Length; i++)
            {
                int num= i * 50 + 50;
                metersArr[i] = Convert.ToString(num); 
            }


            rbDist.Click += RbDist_Click;
            rbTime.Click += RbTime_Click;
            save.Click += Save_Click;


            // Create your application here
        }

        private void Save_Click(object sender, EventArgs e)
        {
            Bitmap bitmap=null;
            if (rbFast.Checked == true)
            {
                speed = "fast";
                bitmap= BitmapFactory.DecodeResource(Resources, Resource.Drawable.fast);
            }
            else if (rbMed.Checked == true)
            {
                speed = "med";
                bitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.steady);

            }
            else if(rbSlow.Checked == true)
            {
                speed = "slow";
                bitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.slow);
            }

            if (this.dis!=0)
            {
                Interval_Distance interval = new Interval_Distance((int)(this.dis * 1000), speed);
                this.interval0 = interval;               
            }
            else if (this.time!=TimeSpan.Zero)
            {
                Interval_Time interval = new Interval_Time(time, speed);
                this.interval0 = interval;
            }
            if (this.interval0!=null)
            {
                int intentnum = Intent.GetIntExtra("listNum", 0);
                if (intentnum == 1)
                {
                    Construct_running_Activity.interval_List1.Add(interval0);
                    
                }
                else if (intentnum == 2)
                {
                    Construct_running_Activity.interval_List2.Add(interval0);
                }
                else if (intentnum == 3)
                {
                    Construct_running_Activity.interval_List3.Add(interval0);
                }
            }
            Finish();
        }

        private void RbTime_Click(object sender, EventArgs e)
        {
            witchDialog = 1;
            d = new Dialog(this);
            d.SetContentView(Resource.Layout.time_Dialog_Layout);
            d.SetTitle("set Time for interval");
            d.SetCancelable(true);

            

            hoursPicker= (NumberPicker)d.FindViewById(Resource.Id.npHours);
            this.hoursPicker.MaxValue = 24;
            this.hoursPicker.MinValue = 0;

            this.minPicker= (NumberPicker)d.FindViewById(Resource.Id.npMin);
            this.minPicker.MaxValue = 60;
            this.minPicker.MinValue = 0;

            this.secPicker = (NumberPicker)d.FindViewById(Resource.Id.npSec);
            this.secPicker.MaxValue = 60;
            this.secPicker.MinValue = 0;
            saveDialog = (Button)d.FindViewById(Resource.Id.btSaveDialog);
            d.Show();
            saveDialog.Click += saveDialog_Click;
        }

        private void RbDist_Click(object sender, EventArgs e)
        {
            witchDialog = 2;
            d = new Dialog(this);
            d.SetContentView(Resource.Layout.Distance_Dialog_layout);
            d.SetTitle("set Distance for the interval");
            d.SetCancelable(true);

            saveDialog = (Button)d.FindViewById(Resource.Id.btSaveDialog);
            this.kmPicker = (NumberPicker)d.FindViewById(Resource.Id.npKm);
            this.kmPicker.MaxValue=42;
            this.kmPicker.MinValue = 0;

            this.meterPicker = (NumberPicker)d.FindViewById(Resource.Id.npM);
            this.meterPicker.MaxValue = metersArr.Length-1;
            this.meterPicker.MinValue = 0;
            this.meterPicker.SetDisplayedValues(metersArr);

            d.Show();
            saveDialog.Click += saveDialog_Click;
        }

        private void saveDialog_Click(object sender, EventArgs e)
        {
            if (witchDialog == 2)
            {
                this.typeCounter.Text = "" + (kmPicker.Value*1000.0 +Convert.ToInt32( metersArr[meterPicker.Value])) /1000.0 + " km";
                
                dis = Convert.ToDouble((kmPicker.Value * 1000.0 + Convert.ToInt32(metersArr[meterPicker.Value])) / 1000.0);
                time = TimeSpan.Zero;
                d.Dismiss();
            }
            else if (witchDialog == 1)
            {
                string h=Convert.ToString( hoursPicker.Value), m = Convert.ToString(minPicker.Value), s = Convert.ToString(secPicker.Value);
                if (m.Length==1)
                {
                    m = "0" + m;
                }
                if (s.Length == 1)
                {
                    s = "0" + s;
                }

                this.typeCounter.Text = h + ":" + m + ":" + s;
                dis = 0;
                int sec = hoursPicker.Value * 3600 + minPicker.Value * 60 + secPicker.Value;
                time = TimeSpan.FromSeconds(sec);
                d.Dismiss();
            }
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
            if (MainActivity.musicState)
                ResumeMusic();
        }
    }
}