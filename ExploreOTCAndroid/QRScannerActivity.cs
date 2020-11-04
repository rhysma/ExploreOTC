using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Vision;
using Android.Gms.Vision.Barcodes;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using static Android.Gms.Vision.Detector;
using Plugin.Vibrate;

namespace ExploreOTCAndroid
{
    [Activity(Label = "QRScannerActivity", Theme = "@style/CustomTheme")]
    public class QRScannerActivity : Activity, ISurfaceHolderCallback, IProcessor
    {

        SurfaceView surfaceView;
        TextView txtResult;
        BarcodeDetector barcodeDetector;
        CameraSource cameraSource;
        const int RequestCameraPermisionID = 1001;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.QRScanner);

            //get the layout controls
            surfaceView = FindViewById<SurfaceView>(Resource.Id.cameraView);
            txtResult = FindViewById<TextView>(Resource.Id.txtResult);

            //setup an initial bitmap
            Bitmap bitMap = BitmapFactory.DecodeResource(ApplicationContext
            .Resources, Resource.Drawable.qrCode);

            //create the barcode detector
            barcodeDetector = new BarcodeDetector.Builder(this)
                .SetBarcodeFormats(BarcodeFormat.QrCode)
                .Build();

            //create the camera that will be used to detect the QR code
            cameraSource = new CameraSource
                .Builder(this, barcodeDetector)
                .SetRequestedPreviewSize(640, 480)
                .Build();

            //add these to the layout
            surfaceView.Holder.AddCallback(this);
            barcodeDetector.SetProcessor(this);


        }


        /// <summary>
        /// necessary for the used Interfaces
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="format"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height)
        {
            
        }

        /// <summary>
        /// Is called when the Surface layout item is created. Sets up the camera permissions needed and starts the camera
        /// </summary>
        /// <param name="holder"></param>
        public void SurfaceCreated(ISurfaceHolder holder)
        {
            if (ActivityCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.Camera) != Android.Content.PM.Permission.Granted)
            {
                //Request Permision  
                ActivityCompat.RequestPermissions(this, new string[]
                {
                    Manifest.Permission.Camera
                }, RequestCameraPermisionID);
                return;
            }
            try
            {
                cameraSource.Start(surfaceView.Holder);
            }
            catch (InvalidOperationException)
            {
            }
        }

        /// <summary>
        /// necessary for the used Interfaces and is called when the layout is closed
        /// </summary>
        /// <param name="holder"></param>
        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            cameraSource.Stop();
        }


        /// <summary>
        /// Called when a QR code is detected
        /// </summary>
        /// <param name="detections"></param>
        public void ReceiveDetections(Detections detections)
        {
            SparseArray qrcodes = detections.DetectedItems;
            if (qrcodes.Size() != 0)
            {
                txtResult.Post(() => {

                    //haptic feedback on success
                    var v = CrossVibrate.Current;
                    v.Vibration(TimeSpan.FromSeconds(1)); // 1 second vibration

                    Console.WriteLine(((Barcode)qrcodes.ValueAt(0)).RawValue);
                    txtResult.Text = "Scan Successful\n";
                    txtResult.Text += "Program: " + ((Barcode)qrcodes.ValueAt(0)).RawValue;

                    //need to save scan to the database

                    //who is this that is making the request?
                    //get the info from the stored Shared Preferences
                    var storedStudent = Application.Context.GetSharedPreferences("APP_DATA", FileCreationMode.Private);
                    string email = storedStudent.GetString("email", null);

                    if(email != null)
                    {
                        //gather the obj information to store
                        QRScan newScan = new QRScan(email, ((Barcode)qrcodes.ValueAt(0)).RawValue);

                        //now that the user as scanned the QR code, we want to open the  layout that lets them choose the 
                        //method(s) of contact they want
                        var intent = new Intent(this, typeof(ScanInfoActivity));
                        intent.PutExtra("current_program_id", newScan.ScannedProgram);
                        intent.PutExtra("student_email", newScan.StudentEmail);
                        StartActivity(intent);

                    }
                    
                   
                });
            }
        }


        /// <summary>
        /// necessary for the used Interfaces and is called when the layout is closed
        /// </summary>
        public void Release()
        {
            
        }
    }
}