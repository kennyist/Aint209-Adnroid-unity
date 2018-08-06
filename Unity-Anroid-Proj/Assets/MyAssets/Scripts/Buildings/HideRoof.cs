using UnityEngine;
using System.Collections;

public class HideRoof : MonoBehaviour {

	public GameObject Roof;
	public float distance = 5f;
	public bool isTrigger = false;
	GameObject player;

	void OnTriggerEnter(Collider other) {
		if(isTrigger){
			Roof.renderer.enabled = false;
		}
	}
	
	void OnTriggerExit(Collider other) {
		if(isTrigger){
			Roof.renderer.enabled = true;
		}
	}

	void FixedUpdate () {

		if(!player){
			player = GameObject.FindGameObjectWithTag("Player");
			Debug.Log("Finding");
		} else {
			if(!isTrigger){
				if(Vector3.Distance(Roof.transform.position,player.transform.position) < distance){
					Roof.renderer.enabled = false;
				} else {
					Roof.renderer.enabled = true;
				}
			}
		}
	}
}
