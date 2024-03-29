﻿using System;
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
    class Interval_Distance : Interval
    {
        public int distanceM;//dis in meters
        public string speed, type;
        

        public Interval_Distance(int dis, string t): base(dis,t,"dis")
        {
            this.distanceM = dis;
            this.speed = t;
            this.type = "dis";
        }
        public override int GetAtrtribute()
        {
            return this.distanceM;
        }        

        public override string GetSpeed()
        {
            return this.speed;
        }
        public override string GetType1()
        {
            return this.type;
        }
    }
}