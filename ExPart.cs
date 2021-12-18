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
    class ExPart
    {
        List<Interval_v0> intervals;
        int repeats, currentRep, intervalCount;
        public ExPart(int rep, List<Interval_v0> inList)
        {
            this.repeats = rep;
            this.intervals = inList;
            this.currentRep = 0;
            this.intervalCount = 0;
        }
        /*public void RunPart()
        {
            for (int i = 0; i < this.repeats; i++)
            {
                this.intervalCount = 0;
                for (int j = 0; j < this.intervals.Count-1; j++)
                {
                    this.intervalCount = j;
                }
                this.repCount++;
            }
        }*/
        public bool MoveToNext()
        {
            if (this.intervalCount < this.intervals.Count - 1)
            {
                this.intervalCount++;
                return false;
            }
            else if (this.currentRep < this.repeats)
            {
                this.currentRep++;
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}