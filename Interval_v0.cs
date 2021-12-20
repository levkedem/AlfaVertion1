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
    public abstract class Interval_v0
    {
        

        public abstract int GetAtrtribute();
        public abstract string GetSpeed();
        public abstract string GetType();
        public abstract Bitmap GetBitmap(); 
    }
}