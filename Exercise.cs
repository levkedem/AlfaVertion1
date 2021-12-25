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
    public class Exercise
    {
        List<ExPart> parts;
        DateTime date;
        string name;
        int count;
        int currentPart;
        public Exercise(List<ExPart> parts,string n)
        {
            this.parts = parts;
            this.date = DateTime.Now;
            this.name = n;
            if (n==null)
            {
                this.name = this.date.ToString();
            }
            this.currentPart = 0;
            
        }

        public Interval_v0 GetCurrentInterval()
        {
            return this.parts[this.currentPart].getCurrent();
        }
        public void Next()
        {
            if (parts[currentPart].MoveToNext())
            {
                this.currentPart++;
            }
        }
        public void MoveToNext()
        {
            if (parts[currentPart].MoveToNext())
            {
                currentPart++;
            }
        }

    }
}