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
    [Activity(Label = "ProgramsActivity", Theme = "@style/CustomTheme")]
    public class ProgramsActivity : ListActivity
    {

        //a list to hold the items for the list
        public List<string> Items { get; set; }

        //This adapter is used to connect data to the listview
        ArrayAdapter<string> adapter;

        //setup the shared preferences file where event items will be stored
        ISharedPreferences prefs = Application.Context.GetSharedPreferences("EVENT_DATA", FileCreationMode.Private);

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Programs);

            // initialize the list
            Items = new List<string>();

            PopulateEvents();

            //load in any existing list items from Shared Preferences
            LoadList();

            //Add the list of items to the listview
            adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, Items);
            ListAdapter = adapter;

        }

        //this method loads in the items that are in shared preferences
        //and populates the list
        public void LoadList()
        {
            //first we need to find out how many items we have in shared preferences
            //use the itemCount key to find out
            int count = prefs.GetInt("itemCount", 0);

            //loop through the number of items we should have
            //as we get each key/value pair in SP add them to the Items List
            if (count > 0)
            {
                Toast.MakeText(this, "Click a program for more info", Android.Widget.ToastLength.Long).Show();

                for (int i = 0; i <= count; i++)
                {
                    string item = prefs.GetString(i.ToString(), null);
                    if (item != null)
                    {
                        //put the item in the list
                        Items.Add(item);
                    }
                }
            }

        }//end of LoadList

        //this method loads in data to the shared preferences file
        public void PopulateEvents()
        {
            //remove the current items in shared preferences
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.Clear();
            editor.Commit();

            //add all of the items in the list to the shared preferences 
            //so if the app is closed we can re-open the list
            editor = prefs.Edit();

            int counter = 0;

            //temp list to hold some test event items
            List<string> tempList = new List<string>();
            tempList.Add("Auto Collision Repair");
            tempList.Add("Agriculture");
            tempList.Add("Automotive Technology");
            tempList.Add("Computer Information Science");
            tempList.Add("Construction Technology");
            tempList.Add("Culinary Arts");
            tempList.Add("Diesel Technology");
            tempList.Add("Drafting and Design Technology");
            tempList.Add("Early Childhood Development");
            tempList.Add("Electrical Trades");
            tempList.Add("Electronic Media Production");
            tempList.Add("Fire Science Technology");
            tempList.Add("Graphic Design Technology");
            tempList.Add("Health Sciences");
            tempList.Add("Heating, Refrigeration and Air Conditioning");
            tempList.Add("Industrial Engineering Technology");
            tempList.Add("Industrial Systems Technology");
            tempList.Add("Machine Tool Technology");
            tempList.Add("Networking Technology");
            tempList.Add("Welding Technology");


            //key that keeps track of how many items we have stored in SP
            editor.PutInt("itemCount", tempList.Count);

            //loop through each item in the list and add it to the shared preferences
            //list to be written
            foreach (string item in tempList)
            {
                editor.PutString(counter.ToString(), item);
                counter++;
            }

            //write to SP
            editor.Apply();
        }

        //this is the method that is fired when an item in the list is checked
        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            base.OnListItemClick(l, v, position, id);

            //when the user clicks on the item we want to load the program's web address in the browser
            RunOnUiThread(() =>
            {
                var item = Items[position];
                var uri = Android.Net.Uri.Parse("http://www.exploreotc.net/"); ;

                switch (item)
                {
                    case "Auto Collision Repair":
                        uri = Android.Net.Uri.Parse("https://academics.otc.edu/transportation/auto-collision-repair/");
                        break;
                    case "Agriculture":
                        uri = Android.Net.Uri.Parse("https://academics.otc.edu/construction-trades/agriculture-turf-landscape-management/");
                        break;
                    case "Automotive Technology":
                        uri = Android.Net.Uri.Parse("https://academics.otc.edu/transportation/automotive-technology/");
                        break;
                    case "Computer Information Science":
                        uri = Android.Net.Uri.Parse("https://academics.otc.edu/cis/");
                        break;
                    case "Construction Technology":
                        uri = Android.Net.Uri.Parse("https://academics.otc.edu/construction-trades/construction-technology/");
                        break;
                    case "Culinary Arts":
                        uri = Android.Net.Uri.Parse("https://academics.otc.edu/cul-hsm/culinary-arts-hospitality-management/");
                        break;
                    case "Diesel Technology":
                        uri = Android.Net.Uri.Parse("https://academics.otc.edu/transportation/diesel-technology/");
                        break;
                    case "Drafting and Design Technology":
                        uri = Android.Net.Uri.Parse("https://academics.otc.edu/industrial-manufacturing/drafting-and-design/");
                        break;
                    case "Early Childhood Development":
                        uri = Android.Net.Uri.Parse("https://academics.otc.edu/ecd/");
                        break;
                    case "Electrical Trades":
                        uri = Android.Net.Uri.Parse("https://academics.otc.edu/construction-trades/electrical/");
                        break;
                    case "Electronic Media Production":
                        uri = Android.Net.Uri.Parse("https://academics.otc.edu/emp/");
                        break;
                    case "Fire Science Technology":
                        uri = Android.Net.Uri.Parse("https://academics.otc.edu/fst/");
                        break;
                    case "Graphic Design Technology":
                        uri = Android.Net.Uri.Parse("https://academics.otc.edu/gdt/");
                        break;
                    case "Health Sciences":
                        uri = Android.Net.Uri.Parse("https://academics.otc.edu/alliedhealth/high-school-health-sciences/");
                        break;
                    case "Heating, Refrigeration and Air Conditioning":
                        uri = Android.Net.Uri.Parse("https://academics.otc.edu/construction-trades/heating-refrigeration-ac-hra/");
                        break;
                    case "Industrial Engineering Technology":
                        uri = Android.Net.Uri.Parse("http://www.google.com");
                        break;
                    case "Industrial Systems Technology":
                        uri = Android.Net.Uri.Parse("https://academics.otc.edu/industrial-manufacturing/industrial-systems-technology/");
                        break;
                    case "Machine Tool Technology":
                        uri = Android.Net.Uri.Parse("https://academics.otc.edu/industrial-manufacturing/manufacturing-technology/");
                        break;
                    case "Networking Technology":
                        uri = Android.Net.Uri.Parse("https://academics.otc.edu/net/");
                        break;
                    case "Welding Technology":
                        uri = Android.Net.Uri.Parse("https://academics.otc.edu/industrial-manufacturing/welding-technology/");
                        break;

                }

                var intent = new Intent(Intent.ActionView, uri);
                StartActivity(intent);


            });

        }
    }
}