using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollisionSounds : MonoBehaviour {
	
	public List<AudioClip> lightHit;
	public List<AudioClip> mediumHit;
	public List<AudioClip> heavyHit;	
	public AudioClip[] rollSounds;
	
	public float lightMagnitude = 0.5f;
	public float medMagnitude = 1.0f;

	void Start () {
		if(!gameObject.audio){
			gameObject.AddComponent<AudioSource>();
		}
	}
	  
	void OnCollisionEnter(Collision collision) {
		Debug.Log ("test");
		
		if(collider.rigidbody.velocity.magnitude < lightMagnitude){
			audio.PlayOneShot(lightHit[Random.Range(0,lightHit.Count)]);
		} else if (collider.rigidbody.velocity.magnitude < medMagnitude) {
			audio.PlayOneShot(mediumHit[Random.Range(0,mediumHit.Count)]);
		} else {
			audio.PlayOneShot(heavyHit[Random.Range(0,heavyHit.Count)]);	
		}
    }
}
