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

        public Interval_VDB(Interval_v0 interval)
        {
            if (interval.GetType()=="time")
            {

            }
        }
        

    }
}