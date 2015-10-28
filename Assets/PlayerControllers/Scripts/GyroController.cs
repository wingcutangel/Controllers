// Gyroscope-controlled camera for iPhone  Android revised 2.26.12
// Perry Hoberman <hoberman@bway.net>
//
// Usage:
// Attach this script to main camera.
// Note: Unity Remote does not currently support gyroscope.
//
// This script uses three techniques to get the correct orientation out of the gyroscope attitude:
// 1. creates a parent transform (camParent) and rotates it with eulerAngles
// 2. for Android (Samsung Galaxy Nexus) only: remaps gyro.Attitude quaternion values from xyzw to wxyz (quatMap)
// 3. multiplies attitude quaternion by quaternion quatMult
// Also creates a grandparent (camGrandparent) which can be rotated with localEulerAngles.y
// This node allows an arbitrary heading to be added to the gyroscope reading
// so that the virtual camera can be facing any direction in the scene, no matter what the phone's heading
//
// Ported to C# by Simon McCorkindale <simon <at> aroha.mobi>

using UnityEngine;

public class GyroController : IBasicControlls
{
	private Gyroscope gyro;
	private Quaternion rotFix;
	Quaternion quatMap;
	private Transform theTransform;


	public bool rotateOnFireDown = false;

	public GyroController (Transform trans)
	{
		theTransform = trans;
		quatMap = new Quaternion();
		Transform currentParent = theTransform.transform.parent;
		GameObject camParent = new GameObject ("GyroCamParent");
		camParent.transform.position = theTransform.transform.position;
		theTransform.transform.parent = camParent.transform;
		GameObject camGrandparent = new GameObject ("GyroCamGrandParent");
		camGrandparent.transform.position = theTransform.transform.position;
		camParent.transform.parent = camGrandparent.transform;
		camGrandparent.transform.parent = currentParent;

			
		gyro = Input.gyro;
		gyro.enabled = true;
		camParent.transform.eulerAngles = new Vector3 (90, 180, 0);
		rotFix = new Quaternion (0, 0, 1, 0);
				if (Screen.orientation == ScreenOrientation.LandscapeLeft) {
			camParent.transform.eulerAngles = new Vector3 (90, 180, 0);
		} else if (Screen.orientation == ScreenOrientation.Portrait) {
			camParent.transform.eulerAngles = new Vector3 (90, 180, 0);
		} else if (Screen.orientation == ScreenOrientation.PortraitUpsideDown) {
			camParent.transform.eulerAngles = new Vector3 (90, 180, 0);
		} else if (Screen.orientation == ScreenOrientation.LandscapeRight) {
			camParent.transform.eulerAngles = new Vector3 (90, 180, 0);
		} else {
			camParent.transform.eulerAngles = new Vector3 (90, 180, 0);
		}
		
		if (Screen.orientation == ScreenOrientation.LandscapeLeft) {
			rotFix = new Quaternion (0, 0, 1, 0);
		} else if (Screen.orientation == ScreenOrientation.Portrait) {
			rotFix = new Quaternion (0, 0, 1, 0);
		} else if (Screen.orientation == ScreenOrientation.PortraitUpsideDown) {
			rotFix = new Quaternion (0, 0, 1, 0);
		} else if (Screen.orientation == ScreenOrientation.LandscapeRight) {
			rotFix = new Quaternion (0, 0, 1, 0);
		} else {
			rotFix = new Quaternion (0, 0, 1, 0);
		}
		
//		Screen.sleepTimeout = 0;

	}
	public float getHorizontal(){
		return 0f;
	}
	
	public float getVertical(){
		return 0f;
	}
	
	public float getViewX (){
		#if UNITY_IPHONE
		quatMap = gyro.attitude;
		#elif UNITY_ANDROID
		quatMap = new Quaternion(gyro.attitude.x,gyro.attitude.y,gyro.attitude.z,gyro.attitude.w);
		#endif
		Quaternion rotationQuat = quatMap * rotFix;
		return (-rotationQuat.eulerAngles.x);
	}
	
	public float getViewY (){
		#if UNITY_IPHONE
		quatMap = gyro.attitude;
		#elif UNITY_ANDROID
		quatMap = new Quaternion(gyro.attitude.x,gyro.attitude.y,gyro.attitude.z,gyro.attitude.w);
		#endif
		Quaternion rotationQuat = quatMap * rotFix;
		return (rotationQuat.eulerAngles.y);
	}

	public float getZoom(){
		return 0f;
	}

	public Quaternion getViewQuat(){
		#if UNITY_IPHONE
		quatMap = gyro.attitude;
		#elif UNITY_ANDROID
		quatMap = new Quaternion(gyro.attitude.x,gyro.attitude.y,gyro.attitude.z,gyro.attitude.w);
		#endif
		Quaternion rotationQuat = quatMap * rotFix;
		return rotationQuat;
	}
	
	public bool isRunning (){
		return false;
	}
}