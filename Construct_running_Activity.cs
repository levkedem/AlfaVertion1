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
    [Activity(Label = "Construct_running_Activity")]
    public class Construct_running_Activity : Activity
    {
        public static List<Interval_v0> interval_List1 { get; set; }
        public static List<Interval_v0> interval_List2 { get; set; }
        public static List<Interval_v0> interval_List3 { get; set; }

        Interval0_Adapter adapter1, adapter2, adapter3;

        EditText etWorkOutName;
        CheckBox cbWarm, cbCool;
        ListView listView1, listView2, listView3;
        Button makeInterval1, makeInterval2, makeInterval3, finish;
        SeekBar sk1, sk2, sk3;
        TextView rep1, rep2, rep3;

        public static Exercise exercise { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Construct_WorkOut_Running);

            interval_List1 = new List<Interval_v0>();
            adapter1 = new Interval0_Adapter(this, interval_List1);
            listView1 = (ListView)FindViewById(Resource.Id.listViewInterval1);
            listView1.Adapter = adapter1;

            interval_List2 = new List<Interval_v0>();
            adapter2 = new Interval0_Adapter(this, interval_List2);
            listView2 = (ListView)FindViewById(Resource.Id.listViewInterval2);
            listView2.Adapter = adapter2;


            interval_List3 = new List<Interval_v0>();
            adapter3 = new Interval0_Adapter(this, interval_List3);
            listView3 = (ListView)FindViewById(Resource.Id.listViewInterval3);
            listView3.Adapter = adapter3;

            this.cbWarm = (CheckBox)FindViewById(Resource.Id.CHwarmUp);
            this.cbCool = (CheckBox)FindViewById(Resource.Id.CHcoolDown);


            this.makeInterval1 = (Button)FindViewById(Resource.Id.BtAddInterval1);
            this.makeInterval2 = (Button)FindViewById(Resource.Id.BtAddInterval2);
            this.makeInterval3 = (Button)FindViewById(Resource.Id.BtAddInterval3);
            this.finish = (Button)FindViewById(Resource.Id.BtFinish);

            this.etWorkOutName = (EditText)FindViewById(Resource.Id.etWorkName);

            this.sk1 = (SeekBar)FindViewById(Resource.Id.sb1);
            this.rep1 = (TextView)FindViewById(Resource.Id.tvRep1);
            this.rep1.Text = "repeats: " + 5;
            this.sk1.Progress = 50;
            this.sk1.ProgressChanged += Sk1_ProgressChanged;

            this.sk2 = (SeekBar)FindViewById(Resource.Id.sb2);
            this.rep2 = (TextView)FindViewById(Resource.Id.tvRep2);
            this.rep2.Text = "repeats: " + 5;
            this.sk2.Progress = 50;
            this.sk2.ProgressChanged += Sk2_ProgressChanged;

            this.sk3 = (SeekBar)FindViewById(Resource.Id.sb3);
            this.rep3 = (TextView)FindViewById(Resource.Id.tvRep3);
            this.rep3.Text = "repeats: " + 5;
            this.sk3.Progress = 50;
            this.sk3.ProgressChanged += Sk3_ProgressChanged;


            makeInterval1.Click += MakeInterval1_Click;
            makeInterval2.Click += MakeInterval2_Click;
            makeInterval3.Click += MakeInterval3_Click;
            this.finish.Click += Finish_Click;
        }

        private void Sk1_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)// control sb1
        {
            rep1.Text = "repeats:" + ((e.Progress / 10)+1);
        }
        private void Sk2_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)// control sb2
        {
            rep2.Text = "repeats:" + ((e.Progress / 10) + 1);
        }
        private void Sk3_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)// control sb3
        {
            rep3.Text = "repeats:" + ((e.Progress / 10) + 1);
        }

        public void makeExerciseObject()// make exercise
        {
            
            List<ExPart> parts = new List<ExPart>();
            if (this.cbWarm.Checked)
            {
                Bitmap warmBit= BitmapFactory.DecodeResource(Resources, Resource.Drawable.slow);
                Interval_V1 tempInterval = new Interval_V1(TimeSpan.FromMinutes(5), "warm up", warmBit);
                List<Interval_v0> tempList = new List<Interval_v0>();
                tempList.Add(tempInterval);

                parts.Add(new ExPart(1, tempList));
            }
            if (interval_List1.Count>0)
            {
                int repit;
                string repStr = this.rep1.Text;
                int tempint = repStr.ElementAt(9);
                if (tempint!=1)
                {
                    repit = tempint;
                }
                else 
                {
                    if (repStr.Length==11)
                    {
                        repit = 10;
                    }
                    else
                    {
                        repit = 1;
                    }
                }
                ExPart p1 = new ExPart(repit, interval_List1);
                parts.Add(p1);
            }
            if (interval_List2.Count > 0)
            {
                int repit2;
                string repStr2 = this.rep2.Text;
                if (repStr2.ElementAt(10) != 1)
                {
                    repit2 = Convert.ToInt32(repStr2.ElementAt(10));
                }
                else
                {
                    if (repStr2.Length == 11)
                    {
                        repit2 = 10;
                    }
                    else
                    {
                        repit2 = 1;
                    }
                }
                ExPart p2 = new ExPart(repit2, interval_List1);
                parts.Add(p2);
            }
            if (interval_List3.Count > 0)
            {
                int repit3;
                string repStr3 = this.rep2.Text;
                if (repStr3.ElementAt(10) != 1)
                {
                    repit3 = Convert.ToInt32(repStr3.ElementAt(10));
                }
                else
                {
                    if (repStr3.Length == 11)
                    {
                        repit3 = 10;
                    }
                    else
                    {
                        repit3 = 1;
                    }
                }
                ExPart p3 = new ExPart(repit3, interval_List1);
                parts.Add(p3);
            }
            if (this.cbCool.Checked)
            {
                Bitmap coolBit = BitmapFactory.DecodeResource(Resources, Resource.Drawable.slow);
                Interval_V1 tempInterval2 = new Interval_V1(TimeSpan.FromMinutes(5), "warm up", coolBit);
                List<Interval_v0> tempList2 = new List<Interval_v0>();
                tempList2.Add(tempInterval2);

                parts.Add(new ExPart(1, tempList2));
            }
            
            Construct_running_Activity.exercise = new Exercise(parts, this.etWorkOutName.Text);//

        }
        private void Finish_Click(object sender, EventArgs e)
        {
            makeExerciseObject();
            Intent i1 = new Intent(this, typeof(RunningExOnGoing));
            StartActivity(i1);
        }

        private void MakeInterval3_Click(object sender, EventArgs e)//make interval 3
        {
            Intent intent1 = new Intent(this, typeof(Make_running_Interval_Activity));
            intent1.PutExtra("listNum", 3);
            StartActivity(intent1);
        }

        private void MakeInterval2_Click(object sender, EventArgs e)// make interval 2
        {
            Intent intent1 = new Intent(this, typeof(Make_running_Interval_Activity));
            intent1.PutExtra("listNum", 2);
            StartActivity(intent1);
        }

        private void MakeInterval1_Click(object sender, EventArgs e)// make interval 1
        {
            Intent intent1 = new Intent(this, typeof(Make_running_Interval_Activity));
            intent1.PutExtra("listNum",1);
            StartActivity(intent1);
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

            //updating listView
            adapter1.NotifyDataSetChanged();
            adapter2.NotifyDataSetChanged();
            adapter3.NotifyDataSetChanged();
        }

    }


}