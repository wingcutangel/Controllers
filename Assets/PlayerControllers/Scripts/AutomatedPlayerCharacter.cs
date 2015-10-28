using UnityEngine;
using UnityEngine.VR;
using System.Collections;

public class AutomatedPlayerCharacter : MonoBehaviour {
#if (UNITY_ANDROID || UNITY_IOS) && (!UNITY_EDITOR)
	private MobileController controller = new MobileController();
#else
	private StandaloneController controller = new StandaloneController();
#endif
	private Vector3 moveDirection;
	private Vector3 lookDirection;
	private CharacterController characterController;
	private const float CHARACTER_MASS = 70f;
	private Camera theCamera;

	public bool lookOnFirePressed = false;
	public float walkSpeed = 1f;
	public float runMultiplier = 2f;
	public bool manualRotate = false;
	private bool isWalking = false;
	private bool isWalkingBack = false;

	void Start(){
		characterController = GetComponent<CharacterController> ();
		controller.rotateOnFireDown = lookOnFirePressed;

		theCamera = GetComponentInChildren<Camera> ();
	}
	void Update(){
//		if (Input.GetButtonDown ("Fire1")) {
//			isWalking = !isWalking;
//			isWalkingBack = false;
//		}
//		if (Input.GetButtonDown ("Fire2")) {
//			isWalking = false;
//			isWalkingBack = !isWalkingBack;
//		}
	}

	// Update is called once per frame
	void LateUpdate () {
		translatePlayer ();
		if (manualRotate) {
			rotatePlayer ();
		}
	}

	private void translatePlayer(){
		float finalSpeed = walkSpeed;
		if (controller.isRunning()) {
			finalSpeed = walkSpeed * runMultiplier;
		}
		float walking = 0f;
		if (isWalking) {
			walking = 1f;
		} else {
			if (isWalkingBack) {
				walking = -1f;
			}
		}
		moveDirection = new Vector3(0f, 0f,
		                        walking);
//		if (!VRDevice.isPresent) {
//			moveDirection = transform.TransformDirection (moveDirection);
//		} else {
//			moveDirection = theCamera.transform.TransformDirection (moveDirection);
//		}
		moveDirection = theCamera.transform.TransformDirection (moveDirection);
		moveDirection *= finalSpeed;

		moveDirection.y += Physics.gravity.y * CHARACTER_MASS * Time.deltaTime;
		characterController.Move (moveDirection * Time.deltaTime);

	}

	private void rotatePlayer(){
		lookDirection = new Vector3 (controller.getViewX(), controller.getViewY(), 0f);
		transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3 (0f, lookDirection.x, 0f));
		theCamera.transform.rotation = Quaternion.Euler(theCamera.transform.rotation.eulerAngles + new Vector3 (-lookDirection.y, 0f, 0f));
	}

	public void enableManualRotate(){
		manualRotate = true;
	}
	public void disableManualRotate(){
		manualRotate = false;
	}
	public void toggleWalk(){
		isWalking = !isWalking;
	}
}
