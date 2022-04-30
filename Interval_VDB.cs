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
    public class Interval_VDB
    {
        public TimeSpan time;
        public int distanceM;
        public string speed, type;

        public Interval_VDB()
        { }
        public Interval_VDB(Interval interval)
        {
            if (interval.GetType1().Equals("time"))
            {
                this.time = TimeSpan.FromSeconds(interval.GetAtrtribute());
                this.speed = interval.GetSpeed();
                this.type = interval.GetType1();
                this.distanceM = 0;
            }
            else if (interval.GetType1().Equals("dis"))
            {
                this.distanceM = interval.GetAtrtribute();
                this.speed = interval.GetSpeed();
                this.type = interval.GetType1();
                this.time = TimeSpan.FromSeconds(0);
            }
        }
        
        public Interval GetConvertedInterval()
        {
            if (this.type.Equals("time")|| this.type.Equals("warm up")||this.type.Equals("cool down"))
            {
                Interval_Time v1Interval = new Interval_Time(this.time, this.type);
                return (Interval)v1Interval;
            }
            else if (this.GetType().Equals("dis"))
            {
                Interval_Distance v2Interval = new Interval_Distance(this.distanceM, this.speed);
                return (Interval)v2Interval;
            }
            return null;
        }
        

    }
}