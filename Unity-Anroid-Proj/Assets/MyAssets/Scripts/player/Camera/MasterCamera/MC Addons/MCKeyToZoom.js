#pragma strict


private var mc : MasterCamera;

var zoomKey : String = "z";
var zoomDistance : float = 0;
private var returnDistance : float;

function Start(){

	mc = GetComponent(MasterCamera);

}


function Update () {

	
	if (Input.GetKeyDown(zoomKey)) {
			returnDistance = mc.preferredDistance ;	
	} 	
	
	if (Input.GetKey(zoomKey)) {
			mc.preferredDistance = zoomDistance;	
	} 	
	
	if (Input.GetKeyUp(zoomKey)) {
		mc.preferredDistance = returnDistance;	
	}

}