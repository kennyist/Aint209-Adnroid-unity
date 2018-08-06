using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {
	
	public Transform camera;
	public float hoverHeight;
	public bool isSeeker = false;
	public float maxCamHeight;
	public float minCamHeight;
	public string xboxLb = "win LB";
	public string xboxRb = "win RB";

	void Start(){
		if (Application.platform == RuntimePlatform.OSXPlayer){
			xboxLb = "mac LB";
			xboxRb = "mac RB";
		} else {
			xboxLb = "win LB";
			xboxRb = "win RB";
		}
	}

	void Update () {
		camera.position = new Vector3(transform.position.x,transform.position.y + hoverHeight,transform.position.z);
		
		if(isSeeker){
			if (Input.GetKey(KeyCode.Q) || Input.GetButton(xboxLb))
			{
				hoverHeight += 5 * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.E) || Input.GetButton(xboxRb))
			{
				hoverHeight -= 5 * Time.deltaTime;
			}
			
			if(hoverHeight > maxCamHeight) { hoverHeight = maxCamHeight; }
			if(hoverHeight < minCamHeight) { hoverHeight = minCamHeight; }
		}
	}
}
