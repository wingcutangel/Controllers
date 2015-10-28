using UnityEngine;
using System.Collections;

public class moveToClick : MonoBehaviour {
	NavMeshAgent agent;
	public bool faceMovementDirection = true;
	void Start() {
		agent = GetComponent<NavMeshAgent> ();
		agent.updateRotation = faceMovementDirection;
	}

	void Update() {
		Vector3 target = new Vector3 ();
		if (Input.GetButtonDown ("Fire1")) {
			bool raycastHit = CastRayToWorld(out target);
			if (raycastHit == true){
				agent.SetDestination(target);
			}
		}

	}
	
	private bool CastRayToWorld(out Vector3 target) {
		Ray theRay = Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth/2f, Camera.main.pixelHeight/2f, 0f));    
//		Ray theRay = Camera.main.ScreenPointToRay(Input.mousePosition);    
		Debug.DrawRay (theRay.origin, theRay.direction);
		RaycastHit hit = new RaycastHit ();
		bool didRayHit = Physics.Raycast (theRay, out hit);
		NavMeshHit meshHit = new NavMeshHit();
		target = Vector3.zero;
		if (didRayHit == false) {
			return false;
		}
		NavMesh.SamplePosition (hit.point, out meshHit, 1f, NavMesh.AllAreas);
		target = meshHit.position;
		return true;
	}
}
