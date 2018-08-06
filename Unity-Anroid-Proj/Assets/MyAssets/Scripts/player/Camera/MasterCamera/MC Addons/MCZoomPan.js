/*
This is a Master Camera Addon Script to allow the camera to rotate in a given direction as it zooms / scroll in or out.
(c) 2012 Jesse Falleur
*/



@script RequireComponent(MCScroll)
#pragma strict

var farRotation : Vector3 = Vector3(0,0,0);  // This is the rotation at the far limit.
var nearRotation : Vector3 = Vector3(-50,0,0);  // This is the rotation at the near limit
var rotationSpeed : float = 10.0;
private var mcCam : GameObject;
private var scroll : MCScroll;
private var mc : MasterCamera;
private var targetRotation: Vector3;
private var rotationTimer: float;
var rotationTimeAllowance : float = 1.0;

function Awake () {
	mcCam = gameObject.Find("MC.Camera");
	scroll = GetComponent("MCScroll") as MCScroll;
	mc = GetComponent("MasterCamera") as MasterCamera;
	targetRotation = farRotation + ((nearRotation - farRotation) * ((scroll.LimitOuter - mc.preferredDistance) / (scroll.LimitOuter - scroll.LimitInner )));
}

function Update () {
	//we only want to do something when there is a change in the zoom
	if (Input.GetAxis("Mouse ScrollWheel") != 0){ 
		var rotationApplied =  ((scroll.LimitOuter - mc.preferredDistance) / (scroll.LimitOuter - scroll.LimitInner )) ;
		targetRotation = farRotation + ((nearRotation - farRotation) * rotationApplied);
		rotationTimer = Time.time + rotationTimeAllowance;
		
	}
	if( Time.time < rotationTimer){		
		mcCam.transform.localRotation =  Quaternion.Slerp(mcCam.transform.localRotation,(Quaternion.Euler(targetRotation)), Time.deltaTime * rotationSpeed);
	
	}
	
}