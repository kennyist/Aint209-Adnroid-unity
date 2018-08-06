/*
A versitile 3rd/1st person camera rig with scrolling, strafing, collision detection and minimal clipping.
All code (c) Jesse Falleur unless otherwise noted.
www.ThoughtVandal.com 
*/
#pragma strict

var player : GameObject;
@HideInInspector
var locationAdjustment : GameObject;
@HideInInspector
var scroller : GameObject;
@HideInInspector
var cam : GameObject;
@HideInInspector
var rotationObject : GameObject;
@HideInInspector
var straferObject : GameObject;
@HideInInspector
var cameraColliders : GameObject[];
@HideInInspector
var camColliders = CameraColliders();
var collisionLayerMask: LayerMask;

class CameraColliders extends System.Object {
// A group of game objects that set the location for rays to be cast to prevent the camera colliding with meshes, limiting clipping.
	var castUpLocation: GameObject;
	var castDownLocation: GameObject;
	var castLeftLocation: GameObject;
	var castRightLocation: GameObject;
}

class Smooth extends System.Object {
//Camera Follow Smoothing
	var Location : float = 15;
	var Rotation: float = 5;
}

class Horizontal extends System.Object {
	var mouseSensitivity : float = 15;
	var minimumAngle : float = -360;
	var maximumAngle : float = 360;
}

class Vertical extends System.Object {
	var mouseSensitivity : float = 15;
	var minimumAngle : float = -60;
	var maximumAngle : float = 60;
}


enum CameraRotation {Follow, FollowBehind, MouseHorizontal, MouseHorizontalAndVertical, MouseVertical, None}
var cameraRotationMode = CameraRotation.Follow;
var smooth = Smooth();	
var horizontal = Horizontal();	
var vertical = Vertical();
var preferredDistance : float = 5;

private var localRotationZ = 0;
private var vRotation : float;
private var hRotation : float;
private var cameraCollisionCheck : boolean;
private var activateFPS : boolean = false;
private var smoothDampVelocity : float;

function Awake () {
	//Initialize the Rig to find all the rig components.
	straferObject = gameObject.Find("MC.StraferPoint");
	locationAdjustment = gameObject.Find("MC.LocationAdjustment");
	scroller = gameObject.Find("MC.Scroller");
	cam = gameObject.Find("MC.Camera");
	rotationObject = gameObject.Find("MC.RotationPoint");
	cameraColliders = new GameObject[4];
	cameraColliders[0] = gameObject.Find("MC.Collider0");
	cameraColliders[1] = gameObject.Find("MC.Collider1");
	cameraColliders[2] = gameObject.Find("MC.Collider2");
	cameraColliders[3] = gameObject.Find("MC.Collider3");
			
	//Initialize camera rotation
	vRotation = rotationObject.transform.eulerAngles.x;
	hRotation = rotationObject.transform.eulerAngles.y;
		
	//Set the vertical rotation of the camera to the average of the vertical limits.
	rotationObject.transform.localEulerAngles.x = (vertical.minimumAngle + vertical.maximumAngle) / 2;
	rotationObject.transform.localEulerAngles.y = (horizontal.minimumAngle + horizontal.maximumAngle) / 2;
	
	//Set the initial distance from the camera based on the preferred distance
	scroller.transform.localPosition.z = preferredDistance * -1;
}

function LateUpdate (){
	// Follow the Character position with smoothing.
	transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime * smooth.Location);	
}

function Update () {
//
//Determine Camera Rotation
//
	var h : float;	
	switch (cameraRotationMode) {
		case cameraRotationMode.Follow:
			h = Input.GetAxis("Horizontal");		
			if((h < 0 && rotationObject.transform.localEulerAngles.y < horizontal.minimumAngle) || (h > 0 && rotationObject.transform.localEulerAngles.y > horizontal.maximumAngle)){
				h = 0;
			}
			
			rotationObject.transform.Rotate(0,h * smooth.Rotation,vRotation);
			rotationObject.transform.localEulerAngles.z = localRotationZ;
			break;
			
		case cameraRotationMode.FollowBehind:
			h = player.transform.eulerAngles.y;		
			//if((h < 0 && rotationObject.transform.localEulerAngles.y < horizontal.minimumAngle) || (h > 0 && rotationObject.transform.localEulerAngles.y > horizontal.maximumAngle)){
			//	h = 0;
			//}
			
			rotationObject.transform.eulerAngles.y = Mathf.SmoothDampAngle(rotationObject.transform.eulerAngles.y, h, smoothDampVelocity ,  smooth.Rotation / 10);
			rotationObject.transform.localEulerAngles.z = localRotationZ;
			break;
	
		case cameraRotationMode.MouseHorizontalAndVertical:		
			AdjustVertical();			
			
		case cameraRotationMode.MouseHorizontal:
			AdjustHorizontal();		
			break;
			
		case cameraRotationMode.MouseVertical:		
			AdjustVertical();
			break;
		
		case cameraRotationMode.None:
			//do nothing.
			break;
			
	}
	

}// End of Update

function AdjustHorizontal(){
	hRotation += Input.GetAxis("Mouse X") * horizontal.mouseSensitivity ;			
	hRotation = AngleClamp( hRotation, horizontal.minimumAngle, horizontal.maximumAngle);
	rotationObject.transform.localEulerAngles.y = hRotation;	
	
}

function AdjustVertical(){
	vRotation = vRotation - Input.GetAxis("Mouse Y") * vertical.mouseSensitivity;			
	vRotation = AngleClamp( vRotation, vertical.minimumAngle, vertical.maximumAngle);	
			
	rotationObject.transform.localEulerAngles.x = vRotation;
}


function AngleClamp ( rotation : float , min : float, max : float) {

	if(rotation < -360)
		rotation = rotation + 360;
	if(rotation > 360)
		rotation = rotation - 360;
	rotation = Mathf.Clamp(rotation, min, max);
	return rotation;

}

function TestLOS (LOSLayer : LayerMask){
	var LOSCastDirection = rotationObject.transform.position - cam.transform.position;
	if(Physics.Raycast(cam.transform.position, LOSCastDirection, LOSCastDirection.magnitude, LOSLayer)){
		return true;
	} else {
		return false;
	}
}

function GetLocalRotationZ(){
	return localRotationZ;
}

function SetLocalRotationZ(rotation : float){
	localRotationZ = rotation;
}

function SetHorizontalRotation(rotation : float){
	hRotation = rotation;
}

function SetVerticalRotation(rotation : float){
	vRotation = rotation;
}