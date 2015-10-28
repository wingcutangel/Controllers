using UnityEngine;
using System.Collections;

public class StationaryCamera : MonoBehaviour {
	public float minX = -90f;
	public float maxX = 90f;
	public float minY = -60;
	public float maxY = 30f;
	public float sensitivity = 1;

	private IBasicControlls controller;
	private float _x = 0f;
	private float _y;
	private Transform cameraTransform;
	// Use this for initialization
	void Awake () {
		#if (UNITY_ANDROID || UNITY_IOS) && (!UNITY_EDITOR)
		controller = new MobileController();
		#else
		controller = new StandaloneController();
		#endif

		cameraTransform = GetComponentInChildren<Camera> ().gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton("Fire1")){
			rotatePlayer();
		}
	}

	private void rotatePlayer()
	{
		_x += controller.getViewX () * sensitivity;
		_x = Mathf.Clamp (_x, -90f, 90f);
		_y += controller.getViewY () * sensitivity;
		_y = Mathf.Clamp (_y, -60f, 30f);
		transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler (0f, _x, 0f), Time.time);
		cameraTransform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler (-_y, 0f, 0f), Time.time);
	}
}
