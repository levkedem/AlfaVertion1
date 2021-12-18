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
    class Exercise
    {
        List<ExPart> parts;
        DateTime date;
        int count;
        int currentPart;
        public Exercise(List<ExPart> parts)
        {
            this.parts = parts;
            this.date = DateTime.Now;
        }

        /*public void RunExercise()
        {
            for (int i = 0; i < parts.Count-1; i++)
            {
                parts[i]=null;
            }

        }*/
        public void MoveToNext()
        {
            if (parts[currentPart].MoveToNext())
            {
                currentPart++;
            }
        }

    }
}