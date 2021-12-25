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
    class Interval_v2 : Interval_v0
    {
        int distanceM;
        string speed, type;
        Bitmap photo;

        public Interval_v2(int dis, string t, Bitmap photo)
        {
            this.distanceM = dis;
            this.speed = t;
            this.type = "dis";
            this.photo = photo;
        }
        public override int GetAtrtribute()
        {
            return this.distanceM;
        }

        public override Bitmap GetBitmap()
        {
            return this.photo;
        }

        public override string GetSpeed()
        {
            return this.speed;
        }
        public override string GetType()
        {
            return this.type;
        }
    }
}