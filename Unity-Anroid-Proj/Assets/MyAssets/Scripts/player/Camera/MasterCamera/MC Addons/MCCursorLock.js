
#pragma strict

var cursorLockKey : String = "g";
var disableRotOnUnlock : boolean = true;
var lockOnDefault : boolean = false;

private var mc : MasterCamera;
private var rotModeBuffer : CameraRotation;

function Awake(){
	mc = GetComponent("MasterCamera") as MasterCamera;
	rotModeBuffer = mc.cameraRotationMode;
	Screen.lockCursor = lockOnDefault;
}

function Update () {
	if(Input.GetKeyDown(cursorLockKey)){
		Screen.lockCursor = !Screen.lockCursor;		
		if(disableRotOnUnlock && !Screen.lockCursor){
			rotModeBuffer = mc.cameraRotationMode;
			mc.cameraRotationMode = CameraRotation.None;
		} else {
			mc.cameraRotationMode = rotModeBuffer;
		}
	}
}