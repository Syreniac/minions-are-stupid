using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour {

	public float horizontalSpeed 	= 5.0f;
	public float verticalSpeed 		= 5.0f;
	public float baseSpeed 			= 5.0f;
	public float maxSpeed 			= 100.0f;
	public float acceleration 		= 10.0f;

	public float zoomDistance	= 2000.0f;
	public float maxZoom 		= 550.0f;
	public float minZoom 		= -150.0f;
	
	private float defaultZoom	= 0.0f;
	private float currentZoom;
	

	void Start(){
		horizontalSpeed = baseSpeed;
		verticalSpeed = baseSpeed;
		currentZoom = defaultZoom;
	}

	void Update () {
		TranslateCamera();
		ZoomCamera();
	}

	void TranslateCamera(){
		float horizontal = (Input.GetAxis("Horizontal") * horizontalSpeed) * Time.deltaTime;
		float vertical = (Input.GetAxis("Vertical") * verticalSpeed) * Time.deltaTime;

		if(Mathf.Approximately(horizontal, 0.0f)){
			horizontalSpeed = baseSpeed;
		} else {
			if(horizontalSpeed < maxSpeed){
				horizontalSpeed += acceleration;
			}
		}

		if(Mathf.Approximately(vertical, 0.0f)){
			verticalSpeed = baseSpeed;
		} else {
			if(verticalSpeed < maxSpeed){
				verticalSpeed += acceleration;
			}
		}

		transform.Translate(horizontal, 0, vertical, Space.World);
	}

	void ZoomCamera(){
		float zoom = (Input.GetAxis("Mouse ScrollWheel") * zoomDistance) * Time.deltaTime;

		if(Mathf.Approximately(zoom, 0.0f) == false){
			// if we haven't hit zoom limit, or we have hit zoom limit 
			// but want to zoom the other way...
			if( (currentZoom < maxZoom && zoom > 0.0f) || 
				(currentZoom > maxZoom && zoom < 0.0f) ||
				(currentZoom > minZoom && zoom < 0.0f) ||
				(currentZoom < minZoom && zoom > 0.0f) ){
					// Enforce a hard max zoom limit?
					/*if(currentZoom + zoom > maxZoom){
						zoom = maxZoom - currentZoom;
					}*/
					currentZoom += zoom;
					transform.Translate(0, 0, zoom, Space.Self);
			}
		}
	}
}
