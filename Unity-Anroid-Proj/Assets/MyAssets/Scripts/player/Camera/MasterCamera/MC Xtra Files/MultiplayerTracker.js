

var targets : GameObject[];
var camBufferDistance : float = 3;
var camAngleAdjustment : float = 0;

private var targetPosBuffer : Vector3 = Vector3(0,0,0);
private var camDistance : float = 0;
private var camDistanceBuffer : float = 0;
private var rotX : float;

private var mc : MasterCamera;

function Awake(){
	mc = GameObject.Find("MasterCameraRig").GetComponent("MasterCamera") as MasterCamera;
	rotX = (mc.vertical.minimumAngle + mc.vertical.maximumAngle) / 2;
}


function Update () {
	
	camDistance = 0;
	for(var i = 0 ; i < targets.Length ; i++){		
		targetPosBuffer = targetPosBuffer + targets[i].transform.position;		
		
		if(i < targets.Length -1){
				
			for(var id = i + 1 ; id < targets.Length ; id++){
				camDistanceBuffer = (targets[i].transform.position - targets[id].transform.position).magnitude;
				
				if(camDistanceBuffer > camDistance){
					camDistance = camDistanceBuffer;
		
					
				}
	
			}

		
		}
		
	}
	

	
	camDistance = camDistance + camBufferDistance;
	
	targetPosBuffer = targetPosBuffer / (targets.Length + 1);	
	gameObject.transform.position = targetPosBuffer;
	
	
	
	
	
	transform.LookAt(targets[0].transform);
	
//	mc.scroller.transform.localPosition.z = Mathf.Lerp(Mathf.Abs(mc.scroller.transform.localPosition.z), camDistance, 0.5) * -1;
	
	mc.preferredDistance = camDistance;
	
	mc.transform.eulerAngles.y = transform.eulerAngles.y;
	mc.rotationObject.transform.localEulerAngles.y = (45 * targets.Length) + camAngleAdjustment;

}
