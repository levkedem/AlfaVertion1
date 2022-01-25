using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using SQLite;

namespace AlfaVertion1
{
    class HelperClass
    {
        public static string dbname = "Archive";
        public HelperClass()
        {
        }
        public static string Path()
        {
            string path = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), HelperClass.dbname);
            return path;
        }
        public static string BitmapToBase64(Bitmap bitmap)
        {
            string str = "";
            if (bitmap != null)
            {
                using (var stream = new MemoryStream())
                {
                    bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                    var bytes = stream.ToArray();
                    str = Convert.ToBase64String(bytes);
                }
            }
            return str;
        }

        public static List<Exercise> getAll()
        {
            List<Exercise> lst = new List<Exercise>();
            var db = new SQLiteConnection(Path());
            string query = string.Format("SELECT * FROM Archive");
            var Exercises1 = db.Query<Exercise>(query);
            Console.WriteLine(query);
            if (Exercises1.Count > 0)
            {
                foreach (var item in Exercises1)
                {
                    lst.Add(item);
                }
            }
            return lst;
        }

        public static Bitmap Base64ToBitmap(String base64String)
        {
            if (base64String != "")
            {
                byte[] imageAsBytes = Base64.Decode(base64String, Base64Flags.Default);
                return BitmapFactory.DecodeByteArray(imageAsBytes, 0, imageAsBytes.Length);
            }
            return null;

        }
    }
}