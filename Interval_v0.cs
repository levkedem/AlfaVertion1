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
    public class Interval_v0
    {
        public int atrtribute;
        public string speed, type;
        public Interval_v0(int a, string speed, string type)
        {
            this.atrtribute = a;
            this.speed = speed;
            this.type = type;
        }
        public Interval_v0()
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