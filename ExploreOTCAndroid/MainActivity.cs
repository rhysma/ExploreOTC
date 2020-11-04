using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Support.V4.App;
using TaskStackBuilder = Android.Support.V4.App.TaskStackBuilder;
using System;

namespace ExploreOTCAndroid
{
    [Activity(
Label = "Explore OTC",
MainLauncher = true,
Theme = "@style/CustomTheme")]
    public class MainActivity : Activity
    {
       
        //setup the shared preferences file where main app data items will be stored
        ISharedPreferences prefs = Application.Context.GetSharedPreferences("APP_DATA", FileCreationMode.Private);

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            //only un-comment this method if you want to clear out all of the data in shared preferences (testing purposes!)
            //ClearSP();

            //check to see if this device has already registered.  If not, send them to the registration dialog
            //if they are registered, grab their info from shared preferences
            ISharedPreferencesEditor editor = prefs.Edit();
            string email = prefs.GetString("email", "");
            if(email == "")
            {
                //they are not registered
                var openReg = new Intent(this, typeof(RegistrationActivity));
                StartActivity(openReg);
            }
            else
            {
                //they are registered - proceed
                Student currentStudent = new Student();
                currentStudent.FirstName = prefs.GetString("firstname", "");
                currentStudent.LastName = prefs.GetString("lastname", "");
                currentStudent.Email = prefs.GetString("email", "");
                currentStudent.School = prefs.GetString("school", "");
                currentStudent.Phone = prefs.GetString("phone", "");
            }

            //get the map button and setup the click event
            ImageButton mapButton = FindViewById<ImageButton>(Resource.Id.mapButton);
            mapButton.Click += MapButton_Click;

            //get the events button and setup the click event
            ImageButton eventsButton = FindViewById<ImageButton>(Resource.Id.eventsButton);
            eventsButton.Click += EventsButton_Click;

            //get the programs button and setup the click event
            ImageButton programsButton = FindViewById<ImageButton>(Resource.Id.programsButton);
            programsButton.Click += ProgramsButton_Click;

            //get the survey button and setup the click event
            ImageButton surveyButton = FindViewById<ImageButton>(Resource.Id.surveyButton);
            surveyButton.Click += SurveyButton_Click;

            //get the QR scanner button and setup the click event
            ImageButton qrScanButton = FindViewById<ImageButton>(Resource.Id.qrButton);
            qrScanButton.Click += QRScanButton_Click;

            // get the QR scanner history button and setup the click event
             ImageButton historyScanButton = FindViewById<ImageButton>(Resource.Id.scanHistoryButton);
            historyScanButton.Click += HistoryScanButton_Click;

            //check to see if we should be doing a survey
            bool hasSurvey = prefs.GetBoolean("survey", false);
            if(hasSurvey == false)
            {
                SurveyReminder();
            }

        }

        /// <summary>
        /// Click event for the scan history button on the main screen.  When the user clicks this button the scan history activity will open and display the results
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HistoryScanButton_Click(object sender, EventArgs e)
        {
            var openHistory = new Intent(this, typeof(ScanHistoryActivity));
            StartActivity(openHistory);

        }

        /// <summary>
        /// Click event for the Map button on the main screen.  When the user clicks this button the map activity will open and display the map image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapButton_Click(object sender, System.EventArgs e)
        {
            var openMap = new Intent(this, typeof(MapActivity));
            StartActivity(openMap);
        }

        /// <summary>
        /// Click event for the events button on the main screen.  When the user clicks this button the events activity will open and display the list of events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventsButton_Click(object sender, System.EventArgs e)
        {
            var openEvents = new Intent(this, typeof(EventsActivity));
            StartActivity(openEvents);
        }

        /// <summary>
        /// Click event for the programs button on the main screen. When the user clicks this button the programs activity will open and display a clickable list of programs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProgramsButton_Click(object sender, System.EventArgs e)
        {
            var openPrograms = new Intent(this, typeof(ProgramsActivity));
            StartActivity(openPrograms);
        }


        /// <summary>
        /// Click event for the survey button on the main screen
        /// </summary>
        private void SurveyButton_Click(object sender, System.EventArgs e)
        {
            var openSurvey = new Intent(this, typeof(SurveyActivity));
            StartActivity(openSurvey);

        }

       /// <summary>
       /// Click event for the QR Scanner button on the main screen.  When the user clicks this button the QR scanner activity will open and allow the user to scan a code
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void QRScanButton_Click(object sender, System.EventArgs e)
        {
            var openScanner = new Intent(this, typeof(QRScannerActivity));
            StartActivity(openScanner);
        }



        //a testing method that clears out files in shared preferences
        public void ClearSP()
        {
            //setup the shared preferences file where event items will be stored
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.Clear();
            editor.Commit();
        }


        private static readonly int ButtonClickNotificationId = 1000;

        /// <summary>
        /// This method runs to see if the user has completed the survey yet and sets up a reminder for them to do so
        /// </summary>
        public void SurveyReminder()
        {
            //get the current time
            DateTime current = DateTime.Now;

            ISharedPreferencesEditor editor = prefs.Edit();
            //check to see when they registered
            string timeEnrolled = prefs.GetString("timestamp", "08:00:00 AM");
            bool survey = prefs.GetBoolean("survey", false);

            DateTime timestamp = Convert.ToDateTime(timeEnrolled);

            //we want to send the survey notification three hours after they sign up
            if (current >= timestamp.AddHours(3) && survey == false)
            {
                // When the user clicks the notification, SurveyActivity will start up.
                Intent resultIntent = new Intent(this, typeof(SurveyActivity));

                // Construct a back stack for cross-task navigation:
                TaskStackBuilder stackBuilder = TaskStackBuilder.Create(this);
                stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(SurveyActivity)));
                stackBuilder.AddNextIntent(resultIntent);

                // Create the PendingIntent with the back stack:            
                PendingIntent resultPendingIntent =
                    stackBuilder.GetPendingIntent(0, (int)PendingIntentFlags.UpdateCurrent);

                // Build the notification:
                NotificationCompat.Builder builder = new NotificationCompat.Builder(this)
                    .SetAutoCancel(true)                    // Dismiss from the notif. area when clicked
                    .SetContentIntent(resultPendingIntent)  // Start 2nd activity when the intent is clicked.
                    .SetContentTitle("Event Survey")      // Set its title
                    .SetSmallIcon(Resource.Drawable.Icon)  // Display this icon
                    .SetContentText(String.Format(
                        "Complete the ExploreOTC Event Survey")); // The message to display.

                // Finally, publish the notification:
                NotificationManager notificationManager =
                    (NotificationManager)GetSystemService(Context.NotificationService);
                notificationManager.Notify(ButtonClickNotificationId, builder.Build());

            }
        }

    }
}

