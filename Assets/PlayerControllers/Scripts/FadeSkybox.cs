using UnityEngine;
using System.Collections;

public class FadeSkybox : MonoBehaviour {
	public bool fadeSkybox;
	public float maxExposure = 1f;
	public float minExposure = .1f;

	private Material theSkybox;
	// Use this for initialization
	void Start () {
		theSkybox = RenderSettings.skybox;
	}
	
	// Update is called once per frame
	void Update () {
		if (fadeSkybox) {
			float dotCUp = Vector3.Dot (Vector3.up, -transform.forward);
			theSkybox.SetFloat ("_Exposure", Mathf.Lerp (minExposure, maxExposure , 1f - Mathf.Clamp01(dotCUp)));
		}
	}
}
