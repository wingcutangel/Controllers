using UnityEngine;
using System.Collections;

public class StandaloneController: MonoBehaviour, IBasicControlls {
	public bool rotateOnFireDown = false;
	
	public float getHorizontal(){
		float x  = Input.GetAxis ("Horizontal");
		return x;
	}
	
	public float getVertical(){
		float y  = Input.GetAxis ("Vertical");
		return y;
	}
	
	public float getViewX(){
		float x = 0f;

		if (rotateOnFireDown) {
			if (Input.GetButton ("Fire1")) {
				x = Input.GetAxis ("Mouse X");
			}
		} else {
			x = Input.GetAxis ("Mouse X");
		}

		return x;
	}
	
	public float getViewY(){
		float y = 0f;
	
		if (rotateOnFireDown) {
			if (Input.GetButton ("Fire1")) {
				y = Input.GetAxis ("Mouse Y");
			}
		} else {
			y = Input.GetAxis ("Mouse Y");
		}

		return y;
	}

	public float getZoom(){
		return Input.GetAxis ("Mouse ScrollWheel");
	}

	public bool isRunning(){
		return Input.GetButton ("Sprint");
	}
}