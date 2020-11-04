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
    public class QRScan
    {

        public QRScan()
        {

        }

        public QRScan(string student, string program)
        {
            StudentEmail = student;
            ScannedProgram = program;
        }

        public QRScan(string student, string program, int call, int email, int shadow, int tour, int visit)
        {
            StudentEmail = student;
            ScannedProgram = program;
            Call = call;
            Email = email;
            Shadow = shadow;
            Tour = tour;
            Visit = visit;
        }

        public string StudentEmail { get; set; }

        public string ScannedProgram { get; set; }

        public int Call { get; set; }

        public int Email { get; set; }

        public int Shadow { get; set; }

        public int Tour { get; set; }

        public int Visit { get; set; }

        public override string ToString()
        {
            string output = "";
            output += "Scanned Program: " + ScannedProgram + "\n Requested: \n";

            if(Call == 1)
            {
                output += "Phone Call \n";
            }

            if (Email == 1)
            {
                output += "Email \n";
            }

            if (Shadow == 1)
            {
                output += "Job Shadow \n";
            }

            if (Tour == 1)
            {
                output += "Program Tour \n";
            }

            if (Visit == 1)
            {
                output += "Counselor Visit \n";
            }

            return output;
                
            
        }

    }
}