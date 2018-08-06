using UnityEngine;
using System.Collections;

public class ObjectiveManager : MonoBehaviour {

    Objective[] objectives;

	// Use this for initialization
	void Start () {
        objectives = FindObjectsOfType<Objective>();
        Debug.Log(objectives.Length);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
