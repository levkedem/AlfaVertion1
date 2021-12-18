using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AlfaVertion1
{
    [Activity(Label = "Construct_running_Activity")]
    public class Construct_running_Activity : Activity
    {
        public static List<Interval_v0> interval_List1 { get; set; }
        public static List<Interval_v0> interval_List2 { get; set; }
        public static List<Interval_v0> interval_List3 { get; set; }

        Interval0_Adapter adapter1, adapter2, adapter3;

        EditText etWorkOutName;
        CheckBox cbWarm, cbCool;
        ListView listView1, listView2, listView3;
        Button makeInterval1, makeInterval2, makeInterval3;
        SeekBar sk1, sk2, sk3;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Construct_WorkOut_Running);

            interval_List1 = new List<Interval_v0>();
            adapter1 = new Interval0_Adapter(this, interval_List1);
            listView1 = (ListView)FindViewById(Resource.Id.listViewInterval1);
            listView1.Adapter = adapter1;

            interval_List2 = new List<Interval_v0>();
            adapter2 = new Interval0_Adapter(this, interval_List2);
            listView2 = (ListView)FindViewById(Resource.Id.listViewInterval2);
            listView2.Adapter = adapter2;


            interval_List3 = new List<Interval_v0>();
            adapter3 = new Interval0_Adapter(this, interval_List3);
            listView3 = (ListView)FindViewById(Resource.Id.listViewInterval3);
            listView3.Adapter = adapter3;



            // Create your application here
        }
    }
}