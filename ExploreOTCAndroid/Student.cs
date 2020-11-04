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
    public class Student
    {
        public string FirstName { get; set; }

        public string Email { get; set; }
        public string LastName { get; set; }

        public string School { get; set; }

        public string Phone { get; set; }

        public float Perception { get; set; }

        public bool HasSurvey { get; set; }
    }
}