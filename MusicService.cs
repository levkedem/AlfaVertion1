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
        MediaPlayer mp; // media player which plays the music
        MusicBroadcastReciever musicPlayerBroadcast; // broadcast reciever, is registered with the media player an plays the music according to the user

        public static bool musicStopped = false;
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


            // thread which stops the service if music is stopped for a long time, user left the app
            Thread t = new Thread(RunUntilMusicStopped);
            t.Start();

            return base.OnStartCommand(intent, flags, startId);
        }

        private void RunUntilMusicStopped()
        {
            while (!musicStopped) ;
            StopSelf();
        }
        public override void OnDestroy()
        {
            UnregisterReceiver(this.musicPlayerBroadcast);
            StopSelf();
            base.OnDestroy();
            
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
    }
}