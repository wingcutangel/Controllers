using UnityEngine;
using System.Collections;

public class FollowPath : MonoBehaviour {
	public GoDummyPath dummy;
	GoSpline thePath;

	private float startSpeed;
	public float currentSpeed;
	private float targetSpeed;
	private float currentPosition = 0f;

	// Use this for initialization
	void Start () {
		thePath = new GoSpline (dummy.nodes);
		thePath.buildPath ();
		startSpeed = Random.Range (0.01f, .3f);
		targetSpeed = startSpeed;
		currentSpeed = 0f;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		currentSpeed = targetSpeed;//Mathf.SmoothStep (currentSpeed, targetSpeed, Time.deltaTime * 5f);
		currentPosition = currentPosition + (Time.deltaTime * currentSpeed);
		transform.position = thePath.getPointOnPath (currentPosition);
		transform.LookAt (thePath.getPointOnPath (currentPosition + 0.01f));
	}

	public void adjustSpeed(float newSpeed){
		targetSpeed = newSpeed;
	}

	public void resetSpeed(){
		targetSpeed = startSpeed;
	}

}
