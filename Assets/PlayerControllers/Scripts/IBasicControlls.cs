using UnityEngine;
using System.Collections;

public interface IBasicControlls {
	float getHorizontal ();
	
	float getVertical ();
	
	float getViewX ();
	
	float getViewY ();

	float getZoom();

	bool isRunning ();
}
