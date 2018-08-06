/*
Master Camera addon script to control strafing.
All code (c) Jesse Falleur unless otherwise noted.
www.ThoughtVandal.com 
*/
#pragma strict

//How far and how fast do we want the camera to move from side to side of the character, and what keys to control it.
var Speed : float = .5;
var Limit : float = 1;
var SmoothSpeed : float = 5;
var LeftKeyInput : String = "q";
var RightKeyInput : String = "e";

private var mc : MasterCamera;
private var strafeLocation : float;
private var strafeY : float;

function Awake(){
	mc = GetComponent("MasterCamera") as MasterCamera;
	
	if(LeftKeyInput == RightKeyInput){ // if both  strafe keys are the same, we will snap back and fourth between the limits.
			strafeLocation = Limit;		
	} else {
		strafeLocation = 0;
	}	
	
	strafeY = gameObject.Find("MC.StraferPoint").transform.localPosition.y;			
}
function Update () {

	if(LeftKeyInput == RightKeyInput){ // if both  strafe keys are the same, we will snap back and fourth between the limits.
		if (Input.GetKeyDown(RightKeyInput)) {
			strafeLocation = -strafeLocation;	
		} 	
	} else { // This allows greater control over the strafe location.
		if (Input.GetKey(RightKeyInput)  &&  strafeLocation <Limit) {
			strafeLocation += Speed;											
		} else if (Input.GetKey(LeftKeyInput)   && strafeLocation > -Limit) {
			strafeLocation -= Speed;		
		} 
	}	
	
	//Don't let the camera strafer collide through walls.
	if(!(strafeLocation == 0)){
		//Debug.DrawRay(locationAdjustment.transform.position, straferObject.transform.localPosition, Color.green);
		if(Physics.Linecast(mc.locationAdjustment.transform.position, mc.straferObject.transform.position, mc.collisionLayerMask)){
			if(LeftKeyInput == RightKeyInput){ // if both  strafe keys are the same, we will snap back and fourth between the limits.
				strafeLocation = -strafeLocation;
			} else{	
				strafeLocation = 0;
			}
		}		
	}
	mc.straferObject.transform.localPosition = Vector3.Lerp(mc.straferObject.transform.localPosition, (mc.player.transform.TransformDirection(1,0,0) * strafeLocation), Time.deltaTime * SmoothSpeed);
	//reset the y position
	mc.straferObject.transform.localPosition.y = strafeY;
	
}

function StrafeSwap(){
	strafeLocation = -strafeLocation;
}