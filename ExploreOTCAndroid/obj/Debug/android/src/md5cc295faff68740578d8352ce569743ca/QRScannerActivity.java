package md5cc295faff68740578d8352ce569743ca;


public class QRScannerActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer,
		android.view.SurfaceHolder.Callback,
		com.google.android.gms.vision.Detector.Processor
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_surfaceChanged:(Landroid/view/SurfaceHolder;III)V:GetSurfaceChanged_Landroid_view_SurfaceHolder_IIIHandler:Android.Views.ISurfaceHolderCallbackInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_surfaceCreated:(Landroid/view/SurfaceHolder;)V:GetSurfaceCreated_Landroid_view_SurfaceHolder_Handler:Android.Views.ISurfaceHolderCallbackInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_surfaceDestroyed:(Landroid/view/SurfaceHolder;)V:GetSurfaceDestroyed_Landroid_view_SurfaceHolder_Handler:Android.Views.ISurfaceHolderCallbackInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_receiveDetections:(Lcom/google/android/gms/vision/Detector$Detections;)V:GetReceiveDetections_Lcom_google_android_gms_vision_Detector_Detections_Handler:Android.Gms.Vision.Detector/IProcessorInvoker, Xamarin.GooglePlayServices.Vision\n" +
			"n_release:()V:GetReleaseHandler:Android.Gms.Vision.Detector/IProcessorInvoker, Xamarin.GooglePlayServices.Vision\n" +
			"";
		mono.android.Runtime.register ("ExploreOTCAndroid.QRScannerActivity, ExploreOTCAndroid", QRScannerActivity.class, __md_methods);
	}


	public QRScannerActivity ()
	{
		super ();
		if (getClass () == QRScannerActivity.class)
			mono.android.TypeManager.Activate ("ExploreOTCAndroid.QRScannerActivity, ExploreOTCAndroid", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public void surfaceChanged (android.view.SurfaceHolder p0, int p1, int p2, int p3)
	{
		n_surfaceChanged (p0, p1, p2, p3);
	}

	private native void n_surfaceChanged (android.view.SurfaceHolder p0, int p1, int p2, int p3);


	public void surfaceCreated (android.view.SurfaceHolder p0)
	{
		n_surfaceCreated (p0);
	}

	private native void n_surfaceCreated (android.view.SurfaceHolder p0);


	public void surfaceDestroyed (android.view.SurfaceHolder p0)
	{
		n_surfaceDestroyed (p0);
	}

	private native void n_surfaceDestroyed (android.view.SurfaceHolder p0);


	public void receiveDetections (com.google.android.gms.vision.Detector.Detections p0)
	{
		n_receiveDetections (p0);
	}

	private native void n_receiveDetections (com.google.android.gms.vision.Detector.Detections p0);


	public void release ()
	{
		n_release ();
	}

	private native void n_release ();

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
