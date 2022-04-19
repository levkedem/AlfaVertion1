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
    public class ExPartDB
    {
        public List<Interval_VDB> intervals;
        public int repeats;
        public int currentRep;
        public int currentIntrval;

        public ExPartDB(ExPart part)
        {
            this.repeats = part.repeats;
            this.currentRep = 1;
            currentIntrval = 0;
            this.intervals = new List<Interval_VDB>();
            for (int i = 0; i < part.intervals.Count; i++)
            {
                this.intervals.Add(new Interval_VDB(part.intervals[i]));
                Console.WriteLine(i);
            }
        }
        public ExPart GetNprmalPart()
        {
            List<Interval_v0> intervalsForPart = new List<Interval_v0>();
            for (int i = 0; i < intervals.Count; i++)
            {
                intervalsForPart.Add(intervals[i].GetConvertedInterval());
            }
            ExPart p = new ExPart(this.repeats,intervalsForPart);
            return p;
        }

    }
}