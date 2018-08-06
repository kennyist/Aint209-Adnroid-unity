
#pragma strict

var movementBoundaryLayer: LayerMask;
var followTerrain : boolean = true;
var terrainLayer : LayerMask;
var controllerHeight : float = 1;
var viewBorder : boolean = true;
var slowControllerBorder : int = 30;
var fastControllerBorder : int = 10;
var slowMoveSpeed : float = 5;
var fastMoveSpeed : float = 15;
private var slowrectLeft: Rect; 
private var slowrectRight: Rect; 
private var slowrectTop: Rect; 
private var slowrectBottom: Rect; 
private var fastrectLeft: Rect; 
private var fastrectRight: Rect; 
private var fastrectTop: Rect; 
private var fastrectBottom: Rect;
private var camObject : Transform;


function Awake () {
	slowrectLeft = Rect(0,0,slowControllerBorder,Screen.height);
	slowrectRight = Rect(Screen.width-slowControllerBorder,0,slowControllerBorder,Screen.height);
	slowrectTop = Rect(0,0,Screen.width,slowControllerBorder);
	slowrectBottom = Rect(0,Screen.height-slowControllerBorder,Screen.width,slowControllerBorder);
	
	fastrectLeft = Rect(0,0,fastControllerBorder,Screen.height);
	fastrectRight = Rect(Screen.width-fastControllerBorder,0,fastControllerBorder,Screen.height);
	fastrectTop = Rect(0,0,Screen.width,fastControllerBorder);
	fastrectBottom = Rect(0,Screen.height-fastControllerBorder,Screen.width,fastControllerBorder);
	camObject = Camera.main.transform;
}

function Update(){
	var forward = camObject.transform.eulerAngles;
	transform.eulerAngles.y = forward.y;
	
	
	
	
	if(slowrectLeft.Contains(Input.mousePosition) && !Physics.Raycast(transform.position, -transform.right, 1, movementBoundaryLayer)){
		transform.Translate( -Time.deltaTime * slowMoveSpeed,0,0);
	} else if(slowrectRight.Contains(Input.mousePosition) && !Physics.Raycast(transform.position, transform.right, 1, movementBoundaryLayer)){
		transform.Translate( Time.deltaTime * slowMoveSpeed,0,0);
	}
	
	if(slowrectTop.Contains(Input.mousePosition) && !Physics.Raycast(transform.position, -transform.forward, 1, movementBoundaryLayer)){
		transform.Translate(0,0, -Time.deltaTime * slowMoveSpeed);
	} else	if(slowrectBottom.Contains(Input.mousePosition) && !Physics.Raycast(transform.position, transform.forward, 1, movementBoundaryLayer)){
		transform.Translate(0,0,Time.deltaTime * slowMoveSpeed);
	}
	
	if(fastrectLeft.Contains(Input.mousePosition) && !Physics.Raycast(transform.position, -transform.right, 1, movementBoundaryLayer)){
		transform.Translate( -Time.deltaTime * fastMoveSpeed,0,0);
	} else if(fastrectRight.Contains(Input.mousePosition) && !Physics.Raycast(transform.position, transform.right, 1, movementBoundaryLayer)){
		transform.Translate( Time.deltaTime * fastMoveSpeed,0,0);
	}
	
	if(fastrectTop.Contains(Input.mousePosition) && !Physics.Raycast(transform.position, -transform.forward, 1, movementBoundaryLayer)){
		transform.Translate(0,0, -Time.deltaTime * fastMoveSpeed);
	} else	if(fastrectBottom.Contains(Input.mousePosition) && !Physics.Raycast(transform.position, transform.forward, 1, movementBoundaryLayer)){
		transform.Translate(0,0,Time.deltaTime * fastMoveSpeed);
	}
	
	if(followTerrain){
		var hit : RaycastHit;
		if(Physics.Raycast(transform.position, Vector3.down, hit, 10, terrainLayer)){
		transform.position.y = hit.point.y + controllerHeight;
		} else {
		 transform.position.y += 5;
		}	
	}

}

function OnGUI () {
	if(viewBorder){
		GUI.Box(slowrectLeft,"slow move left");
		GUI.Box(slowrectRight,"slow move right");
		GUI.Box(slowrectTop,"slow move forward");
		GUI.Box(slowrectBottom,"slow move backward");
		
		GUI.Box(fastrectLeft,"fast move left");
		GUI.Box(fastrectRight,"fast move right");
		GUI.Box(fastrectTop,"fast move forward");
		GUI.Box(fastrectBottom,"fast move backward");		
	}	
}