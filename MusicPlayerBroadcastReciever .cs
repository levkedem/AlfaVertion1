using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AlfaVertion1
{
    [BroadcastReceiver]
    public class MusicBroadcastReciever : BroadcastReceiver
    {
        MediaPlayer mp;//plays the music
        public MusicBroadcastReciever()
        {
        }
        public MusicBroadcastReciever(MediaPlayer mp)
        {
            this.mp = mp;
            mp.Looping = true;



            mp.SetVolume(1, 1);

        }
        public override void OnReceive(Context context, Intent intent)
        {
            //Toast.MakeText(context, "Received intent!", ToastLength.Short).Show();
            int action = intent.GetIntExtra("action", 0);
            if (action == 1)
            {
                mp.Start();
                mp.SetVolume((float)0.3, (float)0.3);
                
            }
            else if (action == 0)
            {
                mp.Pause();                
            }
        }

    }
}