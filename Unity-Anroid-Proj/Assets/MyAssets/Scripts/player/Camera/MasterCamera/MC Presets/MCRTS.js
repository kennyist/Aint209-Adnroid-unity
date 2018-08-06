#pragma strict

@script ExecuteInEditMode()


private var mc : MasterCamera;
private var components : Component[] ;

function Awake(){
	
	
	components = gameObject.GetComponents(MonoBehaviour);
	for(var i = 0 ; i < components.Length ; i++){
		if(components[i] != this){
			DestroyImmediate(components[i]);
			}
	
	}
	
	
	gameObject.AddComponent(MasterCamera);
	gameObject.AddComponent(MCLOSFade);
	

	mc = GetComponent("MasterCamera") as MasterCamera;
	mc.cameraRotationMode = CameraRotation.None;
	mc.horizontal.minimumAngle = 60;
	mc.horizontal.maximumAngle = 60;
	mc.vertical.minimumAngle = 60;
	mc.vertical.maximumAngle = 60;
	mc.preferredDistance = 15;
	
	
	
	
}

function LateUpdate(){
DestroyImmediate (this);

}