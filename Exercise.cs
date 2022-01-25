﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace AlfaVertion1
{
    [Table("Archive")]
    public class Exercise
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int id { get; set; }

        public List<ExPart> parts;
        public DateTime date;
        public string name { get; set; }
        public int timeForThisEx { get; set; }

        public double distanceForThisExKM { get; set; }

        int count;
        int currentPart;
        public EventHandler<int> theEnd;

        public Exercise()
        { 
        
        }
        public Exercise(List<ExPart> parts,string n)
        {
            this.parts = parts;
            this.date = DateTime.Now;
            this.name = n;
            if (n=="")
            {
                this.name = "Exercise";
            }
            this.currentPart = 0;           
        }
        public void StartEx()
        {
            this.currentPart = 0;
            for (int i = 0; i < parts.Count-1; i++)
            {
                parts[i].StartPart();
            }
        }
        public void setExercise(int id, List<ExPart> parts, DateTime date, string name)
        {
            this.id = id;
            this.parts = parts;
            this.date = date;
            this.name = name;
        }

        public Interval_v0 GetCurrentInterval()
        {
            return this.parts[this.currentPart].getCurrent();
        }
        public void Next()
        {
            if (parts[currentPart].MoveToNext())
            {
                if (this.currentPart+1==this.parts.Count)
                {
                    this.theEnd.Invoke(this,1);
                }
                else
                    this.currentPart++;
            }
        }
        public void MoveToNext()
        {
            if (parts[currentPart].MoveToNext())
            {
                currentPart++;
            }
        }

    }
}