
#pragma strict

@script ExecuteInEditMode()


private var mc : MasterCamera;
private var scroll : MCScroll;
private var components : Component[];

function Awake(){
	
	
	components = gameObject.GetComponents(MonoBehaviour);
	for(var i = 0 ; i < components.Length ; i++){
		if(components[i] != this){
			DestroyImmediate(components[i]);
			}
	
	}
	gameObject.AddComponent(MasterCamera);
	
	
	mc = GetComponent("MasterCamera") as MasterCamera;
	mc.cameraRotationMode = CameraRotation.MouseHorizontalAndVertical;
	mc.smooth.Location = 15;
	mc.smooth.Rotation = 5;
	mc.horizontal.minimumAngle = -360;
	mc.horizontal.maximumAngle = 360;
	mc.vertical.minimumAngle = -60;
	mc.vertical.maximumAngle = 60;
	mc.preferredDistance = 5;
	
	gameObject.AddComponent(MCScroll);
	
	scroll = GetComponent("MCScroll") as MCScroll;
	scroll.toggleDistance.enableToggleDistance = true;
	scroll.enableCameraCollision = true;
	scroll.meshHiding.enableMeshHiding = true; 
	
	gameObject.AddComponent("MCStrafe");
	
	
	
	
	
}

function LateUpdate(){
DestroyImmediate (this);

}