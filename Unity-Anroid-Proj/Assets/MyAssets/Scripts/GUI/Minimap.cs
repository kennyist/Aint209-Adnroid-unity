using UnityEngine;
using System.Collections;

public class Minimap : MonoBehaviour {

	public Camera miniMapCam;
	public Material miniMapMat;
	public RenderTexture miniMapTex;

	float curSize = 5f;
	public float camSize = 5f;
	public float largerSize = 10f;
	public float increaseSpeed = 2f;

	private bool larger = false;

	void FixedUpdate () {

		miniMapCam.transform.position = new Vector3(transform.position.x,transform.position.y + 5, transform.position.z);

		if(Input.GetKeyDown(KeyCode.M)){
			larger = !larger;
		}

		if(larger){

			if(curSize < largerSize){
				curSize += Time.deltaTime * increaseSpeed;
			}

			miniMapCam.orthographicSize = curSize;

		} else {
		
			if(curSize > camSize){
				curSize -= Time.deltaTime * increaseSpeed;
			}

			miniMapCam.orthographicSize = curSize;

		}

	}

	void OnGUI(){
		GUI.Box(new Rect(300,20,100,20), larger+":"+curSize);
	}
}
