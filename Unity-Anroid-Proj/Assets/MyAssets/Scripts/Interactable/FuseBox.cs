using UnityEngine;
using System.Collections;

public class FuseBox : MonoBehaviour {

	public Camera cam;
	bool showMessage = false;
	public Light[] lights;

	void OnTriggerEnter(Collider other) {
		showMessage = true;
	}

	void OnTriggerExit(Collider other) {
		showMessage = false;
	}

	void Update () {
		if(showMessage){
			if(Input.GetKeyDown(KeyCode.F)){
				foreach(Light light in lights){
					light.enabled = false;
				}
			}
		}
	}

	void OnGUI(){
		if(showMessage){
			Vector3 boxPos = cam.WorldToScreenPoint(transform.position);
			//boxPos.y = Screen.width - boxPos.y;
			GUI.Box(new Rect(boxPos.x,boxPos.y,100,20),"F");
		}
		GUI.Box(new Rect(10,30,100,20),""+showMessage);
	}
}
