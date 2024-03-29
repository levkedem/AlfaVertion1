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

namespace AlfaVertion1
{
    
    public class Exercise
    {
        
        public int id { get; set; }

        public string user;//device mac
        public bool isPublic;

        public List<ExPart> parts;// all the part for the exercise
        public DateTime date;// date of cration
        public string name { get; set; }//ex name
        public int timeForThisEx { get; set; }

        public double distanceForThisExKM { get; set; }

        int currentPart;
        public EventHandler<int> theEnd;

        public Exercise()
        { 
        
        }
        
        public Exercise(List<ExPart> parts,string n, bool ispub)
        {
            this.parts = parts;
            this.date = DateTime.Now;
            this.name = n;
            this.timeForThisEx = 0;
            this.distanceForThisExKM = 0;
            if (n=="")
            {
                this.name = "Exercise";
            }
            this.currentPart = 0;

            this.user = MainActivity.userName.GetString("UserName", "0");
            this.isPublic = ispub;
        }
        public Exercise(List<ExPart> parts, string n, int t, double km)
        {
            this.parts = parts;
            this.date = DateTime.Now;
            this.name = n;
            this.timeForThisEx = t;
            this.distanceForThisExKM = km;
            if (n == "")
            {
                this.name = "Exercise";
            }
        }

        public void StartEx()
        {
            this.currentPart = 0;
            for (int i = 0; i < this.parts.Count-1; i++)
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

        public Interval GetCurrentInterval()
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