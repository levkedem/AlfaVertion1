using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AlfaVertion1
{
    [Service(Label = "FirstService")]//write service to menifest file 
    [IntentFilter(new String[] { "com.yourname.FirstService" })]
    class MusicService:Service
    {
        IBinder binder;//null not in bagrut 
        MediaPlayer mp;
        public override StartCommandResult OnStartCommand(Android.Content.Intent intent, StartCommandFlags flags, int startId)
        {
            
            mp = MediaPlayer.Create(this, Resource.Raw.Malagueña___Michael_Lucarelli___classical_guitar);
            mp.Start();
            return StartCommandResult.NotSticky;
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (mp != null)
            {
                mp.Stop();
                mp.Release();
                mp = null;
            }
        }
    }
}