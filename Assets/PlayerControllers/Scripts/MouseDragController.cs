using UnityEngine;
using System.Collections;

public class MouseDragController : MonoBehaviour, IBasicControlls {
	private bool isDragging;
	private Vector2 dragOrigin;
	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
		if ((Input.touchCount > 0 || Input.GetMouseButton (0)) && !isDragging) {
			isDragging = true;
			dragOrigin = getCursorPosition ();
		} else {
			if ((Input.touchCount < 0 || !Input.GetMouseButton (0)) && isDragging) {
				isDragging = false;
			}
		}
	}

	#region IBasicControlls implementation
	public float getHorizontal ()
	{
		return 0f;
	}
	public float getVertical ()
	{
		return 0f;
	}
	public float getViewX ()
	{
		if (isDragging) {
			Vector2 delta = getCursorPosition() - dragOrigin;
			return delta.x * Time.deltaTime;;
		} else {
			return 0;
		}
	}
	public float getViewY ()
	{
		if (isDragging) {
			Vector2 delta = getCursorPosition() - dragOrigin;
			return delta.y * Time.deltaTime;
		} else {
			return 0;
		}
	}
	public float getZoom ()
	{
		return 0f;
	}
	public bool isRunning ()
	{
		return false;
	}
	#endregion

	private Vector2 getCursorPosition(){
#if (UNITY_ANDROID || UNITY_IOS) && (!UNITY_EDITOR)
		return Input.touches[0].position;
#else
		return new Vector2(Input.mousePosition.x, Input.mousePosition.y);
#endif
	}
}
