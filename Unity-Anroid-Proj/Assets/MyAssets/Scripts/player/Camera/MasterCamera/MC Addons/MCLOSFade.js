/*
Master Camera Addon to allow line of sight object fading on selectable layers.
All code (c) Jesse Falleur.
www.ThoughtVandal.com 
Special Thanks to nab477 on the Unity Forums for help on the renderers in children sections.
*/
import System.Collections.Generic;
#pragma strict
#pragma downcast
var fadeOutAlpha : float = 0.2;
var fadeLayer : LayerMask;
var fadeShader : String = "Transparent/Diffuse";
	
private var camController : MasterCamera;
private var fadeHits: RaycastHit[];
private var fadeHitCollider: List.<Collider>;
private var fadeHitShader: List.<Shader>;
private var fadeHitShaderAlpha: List.<float>;


function Awake(){
	camController = GetComponent("MasterCamera") as MasterCamera;	
	fadeHitCollider = new List.<Collider>();
	fadeHitShader = new List.<Shader>();
	fadeHitShaderAlpha = new List.<float>();
	
	
	//Combines the scoll collision mask with the fade layer mask automatically.
	var fadeMask : int = 1 << fadeLayer.value;
	var colMask : int = 1 << camController.collisionLayerMask.value;
	var combinedMask : int = fadeMask | colMask;
	
	//camController.collisionLayerMask = combinedMask;
	
	
}

function Update () {

		var fadeCastDirection = camController.rotationObject.transform.position - camController.cam.transform.position;
		Debug.DrawRay(camController.cam.transform.position, fadeCastDirection, Color.green);
		fadeHits = Physics.RaycastAll (camController.cam.transform.position, fadeCastDirection, fadeCastDirection.magnitude, fadeLayer);
		
		if(fadeHitCollider.Count > 0){
			for(var ix = fadeHitCollider.Count ; ix > 0  ; ix--){				
				var ixc = ix-1;	
				var renderCollider = fadeHitCollider[ixc] as Collider;		
				var rendererBuffer = new List.<Renderer>();
				
				if(renderCollider.renderer){				
					rendererBuffer.Add(renderCollider.renderer);
				}
				
				if(rendererBuffer.Count == 0)
				{
					var rbb = renderCollider.gameObject.GetComponentsInChildren(typeof(Renderer));
					
					for (var rbbl = 0 ; rbbl < rbb.length ; rbbl++){
					
						rendererBuffer.Add(rbb[rbbl]);
					}	
					
				}
					
				for(var rbc = 0 ; rbc < rendererBuffer.Count ; rbc++){		
					if (rendererBuffer[rbc]) {				
						rendererBuffer[rbc].material.shader = fadeHitShader[ixc];
						
						if(rendererBuffer[rbc].material.HasProperty("_Color")){
							rendererBuffer[rbc].material.color.a = fadeHitShaderAlpha[ixc];
						}
						
						
					}
				}	
				fadeHitShader.RemoveAt(fadeHitShader.Count-1.0);
				fadeHitShaderAlpha.RemoveAt(fadeHitShaderAlpha.Count-1.0);
				fadeHitCollider.RemoveAt(fadeHitCollider.Count -1.0);		
			}
		}
		for (var ic = 0;ic < fadeHits.length; ic++) {
			var hit : RaycastHit = fadeHits[ic];
			var renderer = new List.<Renderer>();
			
			if(hit.collider.renderer){
				renderer.Add(hit.collider.renderer);
			}
		
            if(renderer.Count == 0){            
            	var rb = hit.collider.gameObject.GetComponentsInChildren(typeof(Renderer));
      
            	for(var rbl = 0 ; rbl < rb.length ; rbl++){
                	renderer.Add(rb[rbl]); 
                }
            }
  
			for(var rl = 0; rl < renderer.Count; rl++){
			
				if(renderer[rl].material.shader.name == fadeShader && renderer[rl].material.color.a == fadeOutAlpha){
				//do nothing
				}else{
					fadeHitCollider.Add(hit.collider);			
					
					if (renderer[rl]) {
						
					
							fadeHitShader.Add(renderer[rl].material.shader);
							
							if(renderer[rl].material.HasProperty("_Color")){
								fadeHitShaderAlpha.Add(renderer[rl].material.color.a);	
							} else {
								//This acts as a placeholder in the array for materials which have no color like Mobile > VertexLit
								fadeHitShaderAlpha.Add(1);
							}
							
															
							renderer[rl].material.shader = Shader.Find(fadeShader);
							renderer[rl].material.color.a = fadeOutAlpha;
						
							
					}
				}
			}
			
		}
}
