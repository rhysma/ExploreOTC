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
    [Activity(Label = "RegistrationActivity", Theme = "@style/CustomTheme", NoHistory = true)]
    public class RegistrationActivity : Activity
    {
        //setup the shared preferences file where main app data items will be stored
        ISharedPreferences prefs = Application.Context.GetSharedPreferences("APP_DATA", FileCreationMode.Private);

        Student currentStudent = new Student();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Registration);

            Button submitButton = FindViewById<Button>(Resource.Id.button1);

            //button click event
            submitButton.Click += SubmitButton_Click;

            //this is the "star" rating bar
            RatingBar perception = FindViewById<RatingBar>(Resource.Id.ratingBar1);

            perception.RatingBarChange += (o, es) => {
                currentStudent.Perception = perception.Rating;
            };

            //The data for populating the spinner can be found in Resources/Values/Strings.xml
            //populate spinner
            Spinner spinner = FindViewById<Spinner>(Resource.Id.schoolSpinner);
            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(
                    this, Resource.Array.school_array, Android.Resource.Layout.SimpleSpinnerItem);



            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;

            //add currently selected spinner value to the student object
            currentStudent.School = spinner.GetItemAtPosition(e.Position).ToString();

            //string toast = string.Format("The school is {0}", spinner.GetItemAtPosition(e.Position));
            //Toast.MakeText(this, toast, ToastLength.Long).Show();
        }

        private async void SubmitButton_Click(object sender, System.EventArgs e)
        {

            EditText firstname = FindViewById<EditText>(Resource.Id.firstName);
            currentStudent.FirstName = firstname.Text;
            EditText lastname = FindViewById<EditText>(Resource.Id.lastName);
            currentStudent.LastName = lastname.Text;
            EditText email = FindViewById<EditText>(Resource.Id.email);
            currentStudent.Email = email.Text;
            EditText phone = FindViewById<EditText>(Resource.Id.phoneNo);
            currentStudent.Phone = phone.Text;

           
            //add data to shared preferences
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutString("firstname", currentStudent.FirstName);
            editor.PutString("lastname", currentStudent.LastName);
            editor.PutString("email", currentStudent.Email);
            editor.PutString("school", currentStudent.School);
            editor.PutString("phone",currentStudent.Phone);

            DateTime time = DateTime.Now;
            editor.PutString("timestamp", time.ToString("T"));


            //write to SP
            editor.Apply();

            //information needs to be added to the database using the AWS Lambda function

            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials("AKIAJLTMHVBLUSHK3P3Q", "Tl8PX/+/mawDgbMn1dXTAoFYgjH1ySPuxIsPXd1g");
            

            try {
                
                string dataAsJson = JsonConvert.SerializeObject(currentStudent);
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
                    requestRecord.StreamName = "exploreOTCData";

                    //give partition key that is used to place record in particular shard
                    requestRecord.PartitionKey = "registration";

                    //add record as memory stream
                    requestRecord.Data = memoryStream;

                    //PUT the record to Kinesis
                    Toast.MakeText(this, "Processing registration...", ToastLength.Long);
                    PutRecordResponse response = await kinesisClient.PutRecordAsync(requestRecord);
                    Console.WriteLine(response);
                }
                     
                
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
            }


            //close this activity
            Finish();
        }
    }
}