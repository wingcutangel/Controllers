using UnityEngine;
using UnityEngine.VR;
using System.Collections;

public class PlayerCharacter : MonoBehaviour {
	private IBasicControlls controller;

	private Vector3 moveDirection;
	private Vector3 lookDirection;
	private CharacterController characterController;
	private NavMeshAgent agent;
	private const float CHARACTER_MASS = 70f;
	private Camera theCamera;

	public bool lookOnFirePressed = false;
	public float walkSpeed = 1f;
	public float runMultiplier = 2f;

	void Awake(){
		#if (UNITY_ANDROID || UNITY_IOS) && (!UNITY_EDITOR)
		controller = new MobileController();
		#else
		controller = new StandaloneController();
		#endif
	}
	void Start(){
		characterController = GetComponent<CharacterController> ();
		agent = GetComponent<NavMeshAgent> ();
//		controller.rotateOnFireDown = lookOnFirePressed;

		theCamera = GetComponentInChildren<Camera> ();
	}

	// Update is called once per frame
	void LateUpdate () {
		translatePlayer ();
		rotatePlayer ();
	}

	private void translatePlayer(){
		float finalSpeed = walkSpeed;
		if (controller.isRunning()) {
			finalSpeed = walkSpeed * runMultiplier;
		}

		moveDirection = new Vector3(controller.getHorizontal(), 0f,
		                        controller.getVertical());
		if (!VRDevice.isPresent) {
			moveDirection = transform.TransformDirection (moveDirection);
		} else {
			moveDirection = theCamera.transform.TransformDirection (moveDirection);
		}
		moveDirection *= finalSpeed;

		moveDirection.y += Physics.gravity.y * CHARACTER_MASS * Time.deltaTime;
		agent.Move (moveDirection * Time.deltaTime);

	}

	private void rotatePlayer(){
		lookDirection = new Vector3 (controller.getViewX (), controller.getViewY (), 0f);
		transform.rotation = Quaternion.Euler (transform.rotation.eulerAngles + new Vector3 (0f, lookDirection.x, 0f));
		theCamera.transform.rotation = Quaternion.Euler (theCamera.transform.rotation.eulerAngles + new Vector3 (-lookDirection.y, 0f, 0f));
	}
}
