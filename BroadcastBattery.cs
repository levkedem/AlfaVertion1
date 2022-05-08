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
        int battery;// battery %
        AlertDialog d;// dialog from activity
        public BroadcastBattery()
        {
        }
        public BroadcastBattery(AlertDialog a)
        {
            this.d = a;
        }
        
        public override void OnReceive(Context context, Intent intent)
        {
            this.battery = intent.GetIntExtra("level", 0);
            if (battery<20)
            {
                d.Show();
            }
        }
        public int GetBattery()
        {
            return this.battery;
        }
    }
}