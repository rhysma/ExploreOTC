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
    [Activity(Label = "EventsActivity", Theme = "@style/CustomTheme")]
    public class EventsActivity : ListActivity
    {

        //a list to hold the items for the list
        public List<string> Items { get; set; }

        //This adapter is used to connect data to the listview
        ArrayAdapter<string> adapter;

        //setup the shared preferences file where todo items will be stored
        ISharedPreferences prefs = Application.Context.GetSharedPreferences("EVENT_DATA", FileCreationMode.Private);

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Events);

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
                Toast.MakeText(this, "Getting saved items...", Android.Widget.ToastLength.Short).Show();

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
            tempList.Add("9:00 - Event Start");
            tempList.Add("12:00-1:00 - Lunch, Main Hall");
            tempList.Add("3:00 - Event End");

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
    }
}