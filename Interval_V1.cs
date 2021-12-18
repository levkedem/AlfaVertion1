using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AlfaVertion1
{
    class Interval_V1:Interval_v0
    {
        TimeSpan time;
        string speed, type;
        public Interval_V1(TimeSpan timeSpan, string t)
        {
            this.time = timeSpan;
            this.speed = t;
            this.type = "time";
        }
        public override int GetAtrtribute()
        {
            return this.time.Seconds;
        }
        public override string GetSpeed()
        {
            return this.speed;
        }
        public override string GetType()
        {
            return this.type;
        }
    }
}