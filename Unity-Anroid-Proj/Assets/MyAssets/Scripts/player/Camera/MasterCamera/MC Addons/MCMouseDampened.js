#pragma strict


var damp = 0.5;
var hSpeed = 5.0;
var vSpeed = 5.0;

private var vVelocity = 0.0;
private var hVelocity = 0.0;
private var dampHRotation : float;
private var dampH : float;
private var dampVRotation : float;
private var dampV : float;
private var mc : MasterCamera;

function Awake(){
	mc = GetComponent("MasterCamera") as MasterCamera;
	mc.cameraRotationMode = CameraRotation.None;	
}

function Update () {
	dampHRotation += Input.GetAxis("Mouse X") * hSpeed ;	
	dampH = Mathf.SmoothDampAngle(dampH, dampHRotation, hVelocity, damp, Mathf.Infinity, Time.deltaTime);
	dampH = mc.AngleClamp( dampH, mc.horizontal.minimumAngle, mc.horizontal.maximumAngle);
	mc.rotationObject.transform.localEulerAngles.y = dampH;
	
	
	dampVRotation -= Input.GetAxis("Mouse Y") * vSpeed ;	
	dampV = Mathf.SmoothDampAngle(dampV, dampVRotation, vVelocity, damp, Mathf.Infinity, Time.deltaTime);	
	dampV = mc.AngleClamp( dampV, mc.vertical.minimumAngle, mc.vertical.maximumAngle);
	
	mc.rotationObject.transform.localEulerAngles.x = dampV;
}