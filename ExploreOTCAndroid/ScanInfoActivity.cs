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
    [Activity(
Label = "Explore OTC",
MainLauncher = true,
Theme = "@style/CustomTheme")]
    public class ScanInfoActivity : Activity
    {
        string programID;
        string studentEmail;
        int call = 0;
        int email = 0;
        int shadow = 0;
        int tour = 0;
        int visit = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.ScanInfo);

            programID = Intent.Extras.GetString("current_program_id", "");
            studentEmail = Intent.Extras.GetString("student_email", "");

            TextView programText = FindViewById<TextView>(Resource.Id.programInterest);
            programText.Text += programID;

            //gather all of the checkbox data
            CheckBox callBox = FindViewById<CheckBox>(Resource.Id.checkPhone);
            callBox.Click += (o, ex) => {
                if (callBox.Checked)
                    call = 1;
            };

            CheckBox emailBox = FindViewById<CheckBox>(Resource.Id.checkEmail);
            emailBox.Click += (o, ex) => {
                if (emailBox.Checked)
                    email = 1;
            };

            CheckBox shadowBox = FindViewById<CheckBox>(Resource.Id.checkShadow);
            shadowBox.Click += (o, ex) => {
                if (shadowBox.Checked)
                    shadow = 1;
            };

            CheckBox tourBox = FindViewById<CheckBox>(Resource.Id.checkTour);
            tourBox.Click += (o, ex) => {
                if (tourBox.Checked)
                    tour = 1;
            };

            CheckBox visitBox = FindViewById<CheckBox>(Resource.Id.checkVisit);
            visitBox.Click += (o, ex) => {
                if (visitBox.Checked)
                    visit = 1;
            };

            Button submitButton = FindViewById<Button>(Resource.Id.SubmitButton);
            submitButton.Click += SubmitButton_Click;
        }

        /// <summary>
        /// Click event for the submit button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitButton_Click(object sender, System.EventArgs e)
        {

            //create the scan object
            QRScan newScan = new QRScan(studentEmail, programID, call, email, shadow, tour, visit);

            //write to Database with AWS
            SendToAWS(newScan);

            //close this activity and go back to the homescreen
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);

        }

        public async void SendToAWS(QRScan currentScan)
        {
            //add data to the Kinesis stream so it can be processed by the AWS Lambda function

            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials();

            try
            {

                string dataAsJson = JsonConvert.SerializeObject(currentScan);
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
                    requestRecord.StreamName = "exploreOTCQRData";

                    //give partition key that is used to place record in particular shard
                    requestRecord.PartitionKey = "qrscan";

                    //add record as memory stream
                    requestRecord.Data = memoryStream;

                    //PUT the record to Kinesis

                    PutRecordResponse response = await kinesisClient.PutRecordAsync(requestRecord);
                    Console.WriteLine(response);
                }


            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
    }
}