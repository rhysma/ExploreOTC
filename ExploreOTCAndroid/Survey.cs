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

namespace ExploreOTCAndroid
{
    class Survey
    {
        public string StudentEmail { get; set; }

        public int CareerGoalMilitary { get; set; }
        public int CareerGoalAAS { get; set; }
        public int CareerGoalBA { get; set; }
        public int CareerGoalNoPlan { get; set; }
        public int CareerGoalDont { get; set; }

        public int IncreaseInterest { get; set; }

        public int HSFullDay { get; set; }

        public int HSHalfDay { get; set; }

        public int AfterHS { get; set; }

        public float EventRating { get; set; }

        public string Comments { get; set; }
    }
}