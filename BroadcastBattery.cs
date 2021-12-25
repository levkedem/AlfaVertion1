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
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] { Intent.ActionBatteryChanged })]
    public class BroadcastBattery : BroadcastReceiver
    {
        int battery;
        Paint p1;
        public BroadcastBattery()
        {
        }
        
        public override void OnReceive(Context context, Intent intent)
        {
            this.battery = intent.GetIntExtra("level", 0);            
        }
        public int GetBattery()
        {
            return this.battery;
        }
    }
}