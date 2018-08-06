#pragma strict
@script RequireComponent(MCScroll);

private var mc : MasterCamera;


var zoomKey : String = "f";
var initialZoomFOV : int = 40;
var minZoomFOV : int = 40;
var maxZoomFOV : int = 10;
var FOVSpeed : float = 5;
var FOVMouseSpeed : float = 5;
var cameraDistanceOnFOVZoom : float = 0.0;
var cameraMoveSpeed : float = 15;

private var cameraScrollCorrectionSensitivity : float = 0.5;
private var returnFOV : int;
private var returnDistance : float;
private var targetFOV : int;
private var wheelScrollingEnabled : boolean;

function Start(){

	mc = GetComponent(MasterCamera);

}


function Update () {

	if (Input.GetKey(zoomKey) && Input.GetAxis("Mouse ScrollWheel") > 0  && targetFOV >= maxZoomFOV){
		targetFOV -= FOVMouseSpeed;	
		
	} else if (Input.GetKey(zoomKey) && Input.GetAxis("Mouse ScrollWheel") < 0  && targetFOV <= minZoomFOV){
		targetFOV += FOVMouseSpeed;	
	}
	
	if (Input.GetKeyDown(zoomKey)) {
		wheelScrollingEnabled = gameObject.GetComponent(MCScroll).enableWheelScrolling;
		if(wheelScrollingEnabled)
			gameObject.GetComponent(MCScroll).enableWheelScrolling = false;
		returnFOV = Camera.main.fieldOfView;
		returnDistance = mc.preferredDistance ;
		targetFOV = initialZoomFOV;	
	} 	
	
	if (Input.GetKey(zoomKey)) {
		mc.preferredDistance = Mathf.Lerp(mc.preferredDistance, cameraDistanceOnFOVZoom, Time.deltaTime * cameraMoveSpeed);
		Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetFOV, Time.deltaTime *FOVSpeed);	
	} 	
	
	if (Input.GetKeyUp(zoomKey)) {
		if(wheelScrollingEnabled)
			gameObject.GetComponent(MCScroll).enableWheelScrolling = true;
		mc.preferredDistance = returnDistance;	
		Camera.main.fieldOfView = returnFOV;
		
	}

}