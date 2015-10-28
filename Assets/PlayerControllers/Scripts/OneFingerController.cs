using UnityEngine;
using UnityEngine.VR;
using System.Collections;

public class OneFingerController : MonoBehaviour {
	private IBasicControlls controller;

	private Vector3 moveDirection;
	private Vector3 lookDirection;
	private CharacterController characterController;
	private NavMeshAgent agent;
	private const float CHARACTER_MASS = 70f;
	private Camera theCamera;
	
	public float walkSpeed = 1f;
	public float xSensitivity = 1f;
	public float ySensitivity = 1f;


	void Awake(){
		controller = (gameObject.AddComponent <MouseDragController>()) as IBasicControlls;
	}
	void Start(){
		characterController = GetComponent<CharacterController> ();
		agent = GetComponent<NavMeshAgent> ();

		theCamera = GetComponentInChildren<Camera> ();
	}

	// Update is called once per frame
	void LateUpdate () {
		translatePlayer ();
		rotatePlayer ();
	}

	private void translatePlayer(){
		float finalSpeed = walkSpeed;

		moveDirection = new Vector3(0f, 0f,
		                            controller.getViewY () * ySensitivity);
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
		lookDirection = new Vector3 (controller.getViewX () * xSensitivity, 0f, 0f);
		transform.rotation = Quaternion.Euler (transform.rotation.eulerAngles + new Vector3 (0f, lookDirection.x, 0f));
		theCamera.transform.rotation = Quaternion.Euler (theCamera.transform.rotation.eulerAngles + new Vector3 (-lookDirection.y, 0f, 0f));
	}
}
