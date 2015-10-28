using UnityEngine;
using System.Collections;

public class CameraPoint : MonoBehaviour {
	public float minAngle;
	public float maxAngle;
	public float startAngle;
	public float minDistance;
	public float maxDistance;
	public float startDistance;

	public void Start(){
	
	}

	void OnDrawGizmosSelected() {
		float _x = transform.localEulerAngles.y;
		float _y = startAngle;
		Quaternion rotat = Quaternion.Euler (_y, _x, 0.0f);
		Gizmos.color = Color.gray;

		Gizmos.DrawLine(transform.position, transform.position + rotat * new Vector3(0.0f,0.0f,-minDistance));
		Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position + rotat * new Vector3(0.0f,0.0f,-minDistance), transform.position + rotat * new Vector3(0.0f,0.0f,-maxDistance));
		Gizmos.color = Color.red;
		Gizmos.DrawSphere (transform.position + rotat * new Vector3 (0.0f, 0.0f, -startDistance), 0.1f);
//		Gizmos.DrawSphere (transform.position, 10f);
	}
}
