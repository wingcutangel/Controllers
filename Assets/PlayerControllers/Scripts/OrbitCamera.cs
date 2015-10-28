using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class OrbitCamera : MonoBehaviour
{
	public GameObject _target;
	
	//The default distance of the camera from the target.
	public float _distance = 0f;

	//Control the speed of zooming and dezooming.
	public float _zoomStep = 1.0f;
	
	//The speed of the camera. Control how fast the camera will rotate.
	public float _xSpeed = 3f;
	public float _ySpeed = 3f;

	public float _lowAngle;
	public float _highAngle;
	public float _minDistance;
	public float _maxDistance;

	//The position of the cursor on the screen. Used to rotate the camera.
	private float _x = 0.0f;
	private float _y = 0.0f;
	private float _xtarget = 0.0f;
	private float _ytarget = 0.0f;
	private float zoomTarget = 0.0f;
	private float _dx = 0.0f;
	private float _dy = 0.0f;
	private bool cameraFree = true;
	private bool bAutoOrbit;
	private bool hasGyro;
	//Distance vector. 
	private Vector3 _distanceVector;
	private Vector3 _focusPosition;
	private Gyroscope gyro;

	private IBasicControlls controller;

	private Material theSkybox;
	/**
  * Move the camera to its initial position.
  */

	void Start ()
	{
		#if (UNITY_ANDROID || UNITY_IOS) && (!UNITY_EDITOR)
		controller = new MobileController();
		#else
		controller = new StandaloneController();
		((StandaloneController)controller).rotateOnFireDown = true;
		#endif
	

		if (_target != null) {
			Vector2 angles = _target.transform.localEulerAngles;
			if(!_target.GetComponent<CameraPoint>())
			{
				_x = angles.y;
				_y = 0f;
			} else {
				CameraPoint cp = _target.GetComponent<CameraPoint>();
				_x = angles.y;
				_y = cp.startAngle;
				_distance = cp.startDistance;
				_lowAngle = cp.minAngle;
				_highAngle = cp.maxAngle;
				_minDistance = cp.minDistance;
				_maxDistance = cp.maxDistance;
			}
		} else {
			_target = new GameObject("CameraTarget");
			_x = 0f;
			_y = 0f;
		}
		_distanceVector = new Vector3(0.0f,0.0f,-_distance);
		_focusPosition = _target.transform.position;
		hasGyro = SystemInfo.supportsGyroscope;
		if (hasGyro) {
			Input.gyro.enabled = true;
			gyro = Input.gyro;
		}
		StartCoroutine(autoOrbit(3f));
	}

	void OnEnable() {
		if (hasGyro) {
			Input.gyro.enabled = true;
			gyro = Input.gyro;
		}
	}

	void OnDisable() {
		if (hasGyro) {
			Input.gyro.enabled = false;
		}
	}

	void Update()
	{
		if (hasGyro) {
			Vector3 gyroRotationRate = gyro.rotationRateUnbiased;
			_xtarget += -gyroRotationRate.y * 0.2f;
			_ytarget += -gyroRotationRate.x * 0.15f;
		}
		RotateControls ();
		Zoom ();

		Rotate ();
	}
	
	public void Zoom(float zoom){
		_distance = Mathf.Lerp (_minDistance, _maxDistance, 1f - zoom);
	}

	//adjust rotation target based on user input
	void RotateControls()
	{
		if (!EventSystem.current.IsPointerOverGameObject ()) {

			_dx = controller.getViewX() * _xSpeed;
			_dy = controller.getViewY() * _ySpeed;
			if (_dx != 0f || _dy != 0f){
				bAutoOrbit = false;
				_xtarget += _dx;
				_ytarget += _dy;
				_dx = _dy = 0f;
			}
		}
	}

	//adjust zoom target based on user input
	void Zoom()
	{	
		if (controller.getZoom () != 0f) {
			bAutoOrbit = false;
			if (controller.getZoom () > 0.0f) {
				zoomTarget -= _zoomStep * (_focusPosition - transform.position).magnitude / (100f);
			} else {
				zoomTarget += _zoomStep * (_focusPosition - transform.position).magnitude / (100f);
			}
			zoomTarget = Mathf.Clamp (zoomTarget, _minDistance - _distance, _maxDistance - _distance);
		}
	}

	//apply camera zoom and rotation
	void Rotate()
	{
		float dTime = Time.deltaTime;

		_x = _x + (_xtarget * dTime * 4);
		_xtarget -= _xtarget * dTime * 4;
		if (_xtarget < 0.01f && _xtarget > -0.01f) {
			_xtarget = 0f;
		}

		_y = _y + (_ytarget * dTime * 4);
		_y = Mathf.Clamp (_y, _lowAngle, _highAngle);
		_ytarget -= _ytarget * dTime * 4;
		if (_ytarget < 0.01f && _ytarget > -0.01f) {
			_ytarget = 0f;
		}

		float newDistance = _distance + (zoomTarget * dTime * 2);
		zoomTarget -= zoomTarget * dTime * 2;
		if (newDistance + (zoomTarget * dTime) > _maxDistance || newDistance + (zoomTarget * dTime) < _minDistance) {
			zoomTarget = 0f;
		}
		if (zoomTarget < 0.01f && zoomTarget > -0.01f) {
			zoomTarget = 0f;
		}
		newDistance = Mathf.Clamp (newDistance, _minDistance, _maxDistance);

		_distance = newDistance;
		_distanceVector.z = -_distance;

		//Transform angle in degree in quaternion form used by Unity for rotation.
		Quaternion rotation = Quaternion.Euler (_y, _x, 0.0f);

		//The new position is the target position + the distance vector of the camera
		//rotated at the specified angle.
		Vector3 position = rotation * _distanceVector + _focusPosition;
			
		//Update the rotation and position of the camera.
		transform.rotation = rotation;
		transform.position = position;
	}

	public IEnumerator autoOrbit(float speed){
		bAutoOrbit = true;
		cameraFree = false;
		while (bAutoOrbit) {
			_xtarget += speed * Time.deltaTime;
			yield return null;
		}
		cameraFree = true;
	}

	public void switchFocusPoint(GameObject newTarget){
		StopCoroutine("moveCameraFocus");
		StartCoroutine("moveCameraFocus", newTarget.transform.position);
	}

	public void switchFocusPoint(Vector3 newTarget){
		StopCoroutine("moveCameraFocus");
		StartCoroutine("moveCameraFocus", newTarget);
	}

	public void switchFocusPoint(CameraPoint newPoint){
		StopCoroutine("moveCameraFocus");
		StartCoroutine("moveCameraFocus", newPoint);
	}
	
	IEnumerator moveCameraFocus(Vector3 newPosition){

		Vector3 oldFocus = _focusPosition;
		Vector3 newFocus = newPosition;
		float startTime = Time.time;
		float travelTime = (newFocus-_focusPosition).magnitude / 10f;
		if (travelTime > 0.1f){
			while(Time.time <= startTime + travelTime){
				float factor = (Time.time - startTime)/travelTime;
				_focusPosition = Vector3.Lerp(oldFocus, newFocus, Mathf.SmoothStep(0f, 1f, factor));
				yield return null;
			}

			_focusPosition = newFocus;
//			_target = newTarget;
		}
	}

	IEnumerator moveCameraFocus(CameraPoint newPoint){
		_maxDistance = Mathf.Max(newPoint.maxDistance,_maxDistance);
		_minDistance = Mathf.Min(newPoint.minDistance, _minDistance);
		_highAngle = Mathf.Max(newPoint.maxAngle, _highAngle);
		_lowAngle = Mathf.Min(newPoint.minAngle, _lowAngle);
		Vector3 oldFocus = _focusPosition;
		Vector3 newFocus = newPoint.gameObject.transform.position;
		float oldDistance = _distance;
		float oldAngle = _y;
		Quaternion oldHeading = transform.rotation;
		oldHeading.eulerAngles = new Vector3(0f, oldHeading.eulerAngles.y,0f);
		float startTime = Time.time;
		float travelTime = (newFocus-_focusPosition).magnitude / 10f;
		if (travelTime > 0.1f){
			float factor;
			while(Time.time <= startTime + travelTime){
				factor = (Time.time - startTime)/travelTime;
				_focusPosition = Vector3.Lerp(oldFocus, newFocus, Mathf.SmoothStep(0f, 1f, factor));
				_x = Quaternion.Slerp(oldHeading, newPoint.gameObject.transform.rotation, Mathf.SmoothStep(0f, 1f, factor)).eulerAngles.y;
				_y = Mathf.Lerp(oldAngle, newPoint.startAngle, Mathf.SmoothStep(0f, 1f, factor));
				_distance = Mathf.Lerp(oldDistance, newPoint.startDistance, Mathf.SmoothStep(0f, 1f, factor));
				yield return null;
			}
			
			_focusPosition = newFocus;
			_maxDistance = newPoint.maxDistance;
			_minDistance = newPoint.minDistance;
			_highAngle = newPoint.maxAngle;
			_lowAngle = newPoint.minAngle;
		}
	}
} //End class