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
using Newtonsoft.Json;
using Amazon.Kinesis;
using Amazon.Kinesis.Model;
using System.IO;

namespace ExploreOTCAndroid
{
    [Activity(Label = "SurveyActivity")]
    public class SurveyActivity : Activity
    {
        //setup the shared preferences file where main app data items will be stored
        ISharedPreferences prefs = Application.Context.GetSharedPreferences("APP_DATA", FileCreationMode.Private);
        Student currentStudent = new Student();
        Survey currentSurvey = new Survey();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Survey);

            Button submitButton = FindViewById<Button>(Resource.Id.surveySubmit);
            //button click event
            submitButton.Click += SubmitButton_Click;

            //this is the "star" rating bar
            RatingBar eventRating = FindViewById<RatingBar>(Resource.Id.eventRating);

            eventRating.RatingBarChange += (o, es) =>
            {
                currentSurvey.EventRating = eventRating.Rating;
            };

            #region Checkboxes
            //get the boxes that are checked
            CheckBox goalsMilitary = FindViewById<CheckBox>(Resource.Id.goalsMilitary);
            goalsMilitary.Click += (o, e) =>
            {
                if (goalsMilitary.Checked)
                {
                    currentSurvey.CareerGoalMilitary = 1;
                }
            };
            CheckBox goalsAAS = FindViewById<CheckBox>(Resource.Id.goalsAAS);
            goalsAAS.Click += (o, e) =>
            {
                if (goalsAAS.Checked)
                {
                    currentSurvey.CareerGoalAAS = 1;
                }
            };
            CheckBox goalsBA = FindViewById<CheckBox>(Resource.Id.goalsBA);
            goalsBA.Click += (o, e) =>
            {
                if (goalsBA.Checked)
                {
                    currentSurvey.CareerGoalBA = 1;
                }
            };
            CheckBox goalsNoPlan = FindViewById<CheckBox>(Resource.Id.goalsNoPlan);
            goalsNoPlan.Click += (o, e) =>
            {
                if (goalsNoPlan.Checked)
                {
                    currentSurvey.CareerGoalNoPlan = 1;
                }
            };
            CheckBox goalsDont = FindViewById<CheckBox>(Resource.Id.goalsDont);
            goalsDont.Click += (o, e) =>
            {
                if (goalsDont.Checked)
                {
                    currentSurvey.CareerGoalDont = 1;
                }
            };

            #endregion

            #region RadioButtons
            RadioButton interestYes = FindViewById<RadioButton>(Resource.Id.interestYes);
            interestYes.Click += interestYes_ButtonClick;
            RadioButton interestNo = FindViewById<RadioButton>(Resource.Id.interestNo);
            interestNo.Click += interestNo_ButtonClick;
            RadioButton fulldayYes = FindViewById<RadioButton>(Resource.Id.fulldayYes);
            fulldayYes.Click += fulldayYes_ButtonClick;
            RadioButton fulldayNo = FindViewById<RadioButton>(Resource.Id.fulldayNo);
            fulldayNo.Click += fulldayNo_ButtonClick;
            RadioButton halfdayYes = FindViewById<RadioButton>(Resource.Id.halfdayYes);
            halfdayYes.Click += halfdayYes_ButtonClick;
            RadioButton halfdayNo = FindViewById<RadioButton>(Resource.Id.halfdayNo);
            halfdayNo.Click += halfdayNo_ButtonClick;
            RadioButton afterYes = FindViewById<RadioButton>(Resource.Id.afterYes);
            afterYes.Click += afterYes_ButtonClick;
            RadioButton afterNo = FindViewById<RadioButton>(Resource.Id.afterNo);
            afterNo.Click += afterNo_ButtonClick;
            #endregion

            //get the current student info from SP
            currentStudent.FirstName = prefs.GetString("firstname", "");
            currentStudent.LastName = prefs.GetString("lastname", "");
            currentStudent.Email = prefs.GetString("email", "");
            currentStudent.School = prefs.GetString("school", "");
            currentStudent.Phone = prefs.GetString("phone", "");
        }

        #region Checkbox Click Events
        private void halfdayYes_ButtonClick(object sender, EventArgs e)
        {
            currentSurvey.HSHalfDay = 1;
        }

        private void afterNo_ButtonClick(object sender, EventArgs e)
        {
            currentSurvey.AfterHS = 0;
        }

        private void afterYes_ButtonClick(object sender, EventArgs e)
        {
            currentSurvey.AfterHS = 1;
        }

        private void halfdayNo_ButtonClick(object sender, EventArgs e)
        {
            currentSurvey.HSHalfDay = 0;
        }

        private void fulldayNo_ButtonClick(object sender, EventArgs e)
        {
            currentSurvey.HSFullDay = 0;
        }

        private void fulldayYes_ButtonClick(object sender, EventArgs e)
        {
            currentSurvey.HSFullDay = 1;
        }

        private void interestNo_ButtonClick(object sender, EventArgs e)
        {
            currentSurvey.IncreaseInterest = 0;
        }

        private void interestYes_ButtonClick(object sender, EventArgs e)
        {
            currentSurvey.IncreaseInterest = 1;
        }

        #endregion
        
        
        /// <summary>
        /// Click event for the submit button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SubmitButton_Click(object sender, System.EventArgs e)
        {
            //survey has been completed and submitted.

            //gather up the control information
            //setup a survey object
            currentSurvey.StudentEmail = currentStudent.Email;

            EditText comments = FindViewById<EditText>(Resource.Id.commentsBox);
            currentSurvey.Comments = comments.Text;

            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials();


            try
            {

                string dataAsJson = JsonConvert.SerializeObject(currentSurvey);
                byte[] dataAsBytes = Encoding.UTF8.GetBytes(dataAsJson);
                using (MemoryStream memoryStream = new MemoryStream(dataAsBytes))
                {
                    //create config that points to AWS region
                    AmazonKinesisConfig config = new AmazonKinesisConfig();
                    config.RegionEndpoint = Amazon.RegionEndpoint.USEast1;

                    //create client that pulls creds from config
                    AmazonKinesisClient kinesisClient = new AmazonKinesisClient(awsCredentials, Amazon.RegionEndpoint.USEast1);

                    //create put request
                    PutRecordRequest requestRecord = new PutRecordRequest();
                    requestRecord.StreamName = "exploreOTCSurvey";

                    //give partition key that is used to place record in particular shard
                    requestRecord.PartitionKey = "survey";

                    //add record as memory stream
                    requestRecord.Data = memoryStream;

                    //PUT the record to Kinesis
                    Toast.MakeText(this, "Processing registration...", ToastLength.Long);
                    PutRecordResponse response = await kinesisClient.PutRecordAsync(requestRecord);
                    Console.WriteLine(response);
                }


            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            //add data to shared preferences
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutBoolean("survey", true);

            //close this activity
            Finish();
        }
    
    }
}