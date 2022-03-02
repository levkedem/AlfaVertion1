﻿using System;
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
    public class Interval_VDB
    {
        public TimeSpan time;
        public int distanceM;
        public string speed, type;

        public Interval_VDB(Interval_v0 interval)
        {
            if (interval.GetType().Equals("time"))
            {
                this.time = TimeSpan.FromSeconds(interval.GetAtrtribute());
                this.speed = interval.GetSpeed();
                this.type = interval.GetType();
                this.distanceM = 0;
            }
            else if (interval.GetType().Equals("dis"))
            {
                this.distanceM = interval.GetAtrtribute();
                this.speed = interval.GetSpeed();
                this.type = interval.GetType();
                this.time = TimeSpan.FromSeconds(0);
            }
        }
        /*
        public Interval_v0 GetConvertedInterval()
        {
            if (this.type.Equals("time"))
            {
                Interval_V1 v1Interval=new Interval_V1(this.time,this.type)
            }
            else if (interval.GetType().Equals("dis"))
            {
                this.distanceM = interval.GetAtrtribute();
                this.speed = interval.GetSpeed();
                this.type = interval.GetType();
                this.time = TimeSpan.FromSeconds(0);
            }
        }*/
        

    }
}