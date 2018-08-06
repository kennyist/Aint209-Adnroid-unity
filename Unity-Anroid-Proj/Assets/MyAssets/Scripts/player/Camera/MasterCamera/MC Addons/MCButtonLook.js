/*
Master Camera addon script to allow for button looking.
All code (c) Jesse Falleur unless otherwise noted.
www.ThoughtVandal.com 
*/

#pragma strict

class ButtonHorizontal extends System.Object {
	var mouseSensitivity : float = 15;
	var minimumAngle : float = -360;
	var maximumAngle : float = 360;
}

class ButtonVertical extends System.Object {
	var mouseSensitivity : float = 15;
	var minimumAngle : float = -60;
	var maximumAngle : float = 60;
}

var inputButton : String = "Fire1";
var returnToOnClick : boolean = true;
var clickToToggle : boolean = false;
var buttonHorizontal = ButtonHorizontal();
var buttonVertical = ButtonVertical();



private var vRotation : float;
private var hRotation : float;
private var vReturn : float;
private var hReturn : float;
private var toggled : boolean = false;

private var mc : MasterCamera;

var cameraReturnSpeed :  float = 10;
private var camTime : float = 0;

function Awake(){
	mc = GetComponent("MasterCamera") as MasterCamera;
}

function Update () {
	
	if(Time.time < camTime){
		mc.rotationObject.transform.localEulerAngles.y = Mathf.LerpAngle(mc.rotationObject.transform.localEulerAngles.y, hReturn, Time.deltaTime * cameraReturnSpeed);
		mc.rotationObject.transform.localEulerAngles.x = Mathf.LerpAngle(mc.rotationObject.transform.localEulerAngles.x, vReturn, Time.deltaTime * cameraReturnSpeed);
		
	}


	if(returnToOnClick){
	
		if(clickToToggle && toggled){
			if(Input.GetButtonDown(inputButton)){	
				camTime = Time.time +  ( .1 /  (Time.deltaTime * cameraReturnSpeed));
				//mc.rotationObject.transform.localEulerAngles.y = hReturn;
				//mc.rotationObject.transform.localEulerAngles.x = vReturn;
			}
		
		}
	
		if(!clickToToggle){
			if(Input.GetButtonUp(inputButton)){
				camTime = Time.time +  ( .1 /  (Time.deltaTime * cameraReturnSpeed));	
				//mc.rotationObject.transform.localEulerAngles.y = hReturn;
				//mc.rotationObject.transform.localEulerAngles.x = vReturn;
			}
		}
	}


	if(Input.GetButtonDown(inputButton)){
	
		
		
	
		if(returnToOnClick && !toggled){
			hRotation = mc.rotationObject.transform.localEulerAngles.y;
			vRotation = mc.rotationObject.transform.localEulerAngles.x;
			hReturn = mc.rotationObject.transform.localEulerAngles.y;
			vReturn = mc.rotationObject.transform.localEulerAngles.x;
		}
		
		if(clickToToggle){
			toggled = !toggled;
		}

	}

	if(Input.GetButton(inputButton) || toggled ){
		hRotation += Input.GetAxisRaw("Mouse X") * buttonHorizontal.mouseSensitivity;			
		hRotation = mc.AngleClamp( hRotation, buttonHorizontal.minimumAngle, buttonHorizontal.maximumAngle);			
		mc.rotationObject.transform.localEulerAngles.y =  hRotation;
				
		vRotation = vRotation - Input.GetAxisRaw("Mouse Y") * buttonVertical.mouseSensitivity;			
		vRotation = mc.AngleClamp( vRotation, buttonVertical.minimumAngle, buttonVertical.maximumAngle);		
		mc.rotationObject.transform.localEulerAngles.x = vRotation;
	}
	
	
	
	
}