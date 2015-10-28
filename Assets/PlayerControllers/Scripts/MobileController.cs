using UnityEngine;
using System.Collections;

public class MobileController: IBasicControlls {
	public bool rotateOnFireDown = false;
	public float dHorizontal = 0f;
	public float dVertical = 0f;


	public float getHorizontal(){
		return dHorizontal;
	}
	
	public float getVertical(){
		return dVertical;
	}
	
	public float getViewX(){
		float x = 0f;
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition * Time.deltaTime / Input.GetTouch(0).deltaTime;
			x = touchDeltaPosition.x;
		}
		return x;
	}
	
	public float getViewY(){
		float y = 0f;
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition * Time.deltaTime / Input.GetTouch(0).deltaTime;
			y = touchDeltaPosition.y;
		}
		return y;
	}

	public float getZoom(){
		float deltaMagnitudeDiff = 0f;
		if (Input.touchCount > 1) {
			Touch touchZero = Input.GetTouch (0);
			Touch touchOne = Input.GetTouch (1);
			
			// Find the position in the previous frame of each touch.
			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
			
			// Find the magnitude of the vector (the distance) between the touches in each frame.
			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).sqrMagnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).sqrMagnitude;
			
			// Find the difference in the distances between each frame.
			deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
			return deltaMagnitudeDiff;
		}
		return deltaMagnitudeDiff;
	}
	
	public bool isRunning(){
		return false;
	}
}