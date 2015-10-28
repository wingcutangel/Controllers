using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class AutopilotController : MonoBehaviour {
	[SerializeField] Transform[] waypoints;

	private int destPoint;
	private NavMeshAgent agent;
	private bool isEnabled = false;

	public void startAutopilot () {
		agent = GetComponent<NavMeshAgent>();

		isEnabled = true;

		// Disabling auto-braking allows for continuous movement
		// between points (ie, the agent doesn't slow down as it
		// approaches a destination point).
		agent.autoBraking = false;
		destPoint = findClosest();
		GotoNextPoint();
	}
	
	public void endAutopilot(){
		destPoint = 0;
		isEnabled = false;
	}

	void GotoNextPoint() {
		// Returns if no points have been set up
		if (waypoints.Length == 0)
			return;
		
		// Set the agent to go to the currently selected destination.
		agent.destination = waypoints[destPoint].position;
		
		// Choose the next point in the array as the destination,
		// cycling to the start if necessary.
		destPoint = (destPoint + 1) % waypoints.Length;
	}
	
	
	void Update () {
		// Choose the next destination point when the agent gets
		// close to the current one.
		if (isEnabled) {
			if (agent.remainingDistance < 0.2f)
				GotoNextPoint ();
		}
	}

	int findClosest(){
		int closestPoint = -1;
		float closestDistance = 999999f;
		int i = 0;
		foreach (Transform t in waypoints) {
			if (Vector3.Distance(t.position, transform.position) < closestDistance){
				closestPoint = i;
				closestDistance = Vector3.Distance(t.position, transform.position);
			}
			i++;
		}

		return closestPoint;
	}
}
