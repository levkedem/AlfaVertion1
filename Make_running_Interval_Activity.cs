using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AlfaVertion1
{
    [Activity(Label = "Make_running_Interval_Activity")]
    public class Make_running_Interval_Activity : Activity
    {

        RadioGroup rgPace, rgType;
        RadioButton rbFast, rbMed, rbSlow, rbDist, rbTime;
        TextView typeCounter;
        double dis;
        TimeSpan time;
        Button save;

        Dialog d;
        EditText etDis, etmin, etsec;
        Button saveDialog;

        int witchDialog;
        string speed;
        Interval_v0 interval0;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Construct_WorkOut_Running);

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


            rbDist.Click += RbDist_Click;
            rbTime.Click += RbTime_Click;
            save.Click += Save_Click;


            // Create your application here
        }

        private void Save_Click(object sender, EventArgs e)
        {
            Bitmap bitmap;
            if (rbFast.Checked)
            {
                speed = "fast";
                bitmap= BitmapFactory.DecodeResource(Resources, Resource.Drawable.fast);
            }
            else if (rbMed.Checked)
            {
                speed = "med";
                bitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.steady);

            }
            else
            {
                speed = "slow";
                bitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.slow);
            }

            if (this.dis!=0)
            {
                Interval_v2 interval = new Interval_v2((int)this.dis * 1000, speed,bitmap);
                this.interval0 = interval;               
            }
            else if (this.time!=TimeSpan.Zero)
            {
                Interval_V1 interval = new Interval_V1(time, speed,bitmap);
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

            etmin = (EditText)d.FindViewById(Resource.Id.etMin);
            etsec = (EditText)d.FindViewById(Resource.Id.etSec);
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

            etDis = (EditText)d.FindViewById(Resource.Id.etDistance1);
            saveDialog = (Button)d.FindViewById(Resource.Id.btSaveDialog);
            d.Show();
            saveDialog.Click += saveDialog_Click;
        }

        private void saveDialog_Click(object sender, EventArgs e)
        {
            if (witchDialog == 2)
            {
                this.typeCounter.Text = "" + etDis.Text + " km";
                dis = Convert.ToDouble(etDis.Text);
                time = TimeSpan.Zero;
                d.Dismiss();
            }
            else if (witchDialog == 1)
            {
                this.typeCounter.Text = "" + (Convert.ToInt32(etmin.Text) + Convert.ToInt32(etsec.Text) / 60) + ":" + Convert.ToInt32(etsec.Text) % 60;
                dis = 0;
                time = TimeSpan.FromMinutes(Convert.ToInt32(etmin.Text) + Convert.ToDouble(etsec.Text) / 60.0);
                d.Dismiss();
            }
        }
    }
}