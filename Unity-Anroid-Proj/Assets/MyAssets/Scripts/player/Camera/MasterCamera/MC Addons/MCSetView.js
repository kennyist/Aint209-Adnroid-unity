/*
Master Camera Addon to allow setting of camera variables on key press.
All code (c) Jesse Falleur.
www.ThoughtVandal.com 
*/

#pragma strict

var horizontalRotation : float = 30;
var verticalRotation: float = 60;
var setDistance : float = 5.0;
var setKey : String = "r";

private var mc : MasterCamera;
	
function Awake(){
	mc = GetComponent("MasterCamera") as MasterCamera;
	
}
		
function Update () {

	if(Input.GetKeyDown(setKey)){
	
		mc.preferredDistance = setDistance;

        mc.SetVerticalRotation(verticalRotation);

        mc.SetHorizontalRotation(horizontalRotation);
	}

}