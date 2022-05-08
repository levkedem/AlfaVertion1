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
    public class Interval
    {
        public int atrtribute;//atrtribute
        public string speed, type;//interval speed(fast,med,slow),type
        public Interval(int a, string speed, string type)
        {
            this.atrtribute = a;
            this.speed = speed;
            this.type = type;
        }
        public Interval()
        { }

        public virtual int GetAtrtribute()
        {
            return this.atrtribute;
        }

        public virtual string GetSpeed()
        {
            return this.speed;
        }

        public virtual string GetType1()
        {
            return this.type;
        }
        
    }
}