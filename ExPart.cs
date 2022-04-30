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
    public class ExPart
    {
        public List<Interval> intervals;
        public int repeats;
        public int currentRep;
        public int currentIntrval;
        public ExPart(int rep, List<Interval> inList)
        {
            this.repeats = rep;
            this.intervals = inList;
            this.currentRep = 1;
            this.currentIntrval = 0;
        }
        public ExPart()
        { }

        public void StartPart()
        {
            this.currentRep = 1;
            this.currentIntrval = 0;
        }
        public Interval getCurrent()
        {
            return this.intervals[this.currentIntrval];
        }
        public bool MoveToNext()
        {
            if (this.currentIntrval < this.intervals.Count - 1)
            {
                this.currentIntrval++;
                return false;
            }
            else if (this.currentRep < this.repeats)
            {
                this.currentRep++;
                this.currentIntrval = 0;
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}