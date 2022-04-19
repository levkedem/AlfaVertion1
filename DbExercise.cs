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
    public class DbExercise
    {
        //public string  user { get; set; }

        public List<ExPartDB> dBParts { get; set; }
        public DateTime date { get; set; }
        public string name { get; set; }
        public int timeForThisEx { get; set; }

        public double distanceForThisExKM { get; set; }

        public DbExercise()
        {

        }
        public DbExercise(string u, List<ExPartDB> p, DateTime d, string name, int time, double dis)
        {
            //this.user = u;
            this.dBParts = p;
            this.date = d;
            this.name = name;
            this.timeForThisEx = time;
            this.distanceForThisExKM = dis;
        }
        public DbExercise(Exercise ex)
        {
            this.date = ex.date;
            this.name = ex.name;
            this.timeForThisEx = ex.timeForThisEx;
            this.distanceForThisExKM = ex.distanceForThisExKM;

            dBParts = new List<ExPartDB>();
            for (int i = 0; i < ex.parts.Count; i++)
            {
                this.dBParts.Add(new ExPartDB(ex.parts[i]));
            }

        }
        
        public Exercise ConvertToNormal()
        {
            List<ExPart> partsForNew = new List<ExPart>();

            for (int i = 0; i < this.dBParts.Count; i++)
            {
                partsForNew.Add(this.dBParts[i].GetNprmalPart());
            }

            Exercise e = new Exercise(partsForNew, this.name, this.timeForThisEx, this.distanceForThisExKM);
            return e;
        }
    }
}