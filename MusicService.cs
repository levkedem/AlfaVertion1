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
    [Service]
    public class MusicService : Service
    {
        MediaPlayer mp; // media player...
        MusicBroadcastReciever musicPlayerBroadcast; // broadcast reciever for music

        public static bool musicStopped = false;

        public bool didUnReg = false;
        public override void OnCreate()
        {
            base.OnCreate();

            mp = MediaPlayer.Create(this, Resource.Raw.musica);
            mp.SeekTo(17000);
            musicPlayerBroadcast = new MusicBroadcastReciever(mp);

            IntentFilter intentFilter = new IntentFilter("music");
            RegisterReceiver(musicPlayerBroadcast, intentFilter);
        }
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Intent i = new Intent("music");
            i.PutExtra("action", 1);
            SendBroadcast(i);


            return base.OnStartCommand(intent, flags, startId);
        }        
        public override void OnDestroy()
        {
            if (!didUnReg)
            {
                UnregisterReceiver(this.musicPlayerBroadcast);
                didUnReg = true;
            }            
            StopSelf();
            base.OnDestroy();
            
        }
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
    }
}