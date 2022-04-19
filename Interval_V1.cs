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
    class Interval_V1:Interval_v0
    {
        public TimeSpan time;
        public string speed, type;
        public Interval_V1(TimeSpan timeSpan, string t):base((timeSpan.Seconds+timeSpan.Minutes*60),t,"time")
        {
            this.time = timeSpan;
            this.speed = t;
            this.type = "time";
        }
        
        public override int GetAtrtribute()
        {
            return this.time.Seconds + this.time.Minutes * 60;
        }
        public override string GetSpeed()
        {
            return this.speed;
        }
        public override string GetType1()
        {
            return this.type;
        }
    }
}