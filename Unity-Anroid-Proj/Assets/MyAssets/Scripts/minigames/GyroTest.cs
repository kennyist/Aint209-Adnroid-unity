using UnityEngine;
using System.Collections;

public class GyroTest : MonoBehaviour {

    public GameObject obj;

	// Use this for initialization
	void Start () {
        Input.gyro.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 rot = obj.transform.position;
        rot.y += Input.gyro.rotationRate.y / 20;
        obj.transform.position = rot;
	}

    void OnGUI()
    {
        GUI.Label(new Rect(10, Screen.height - 50, 400, 40), "GYRO HEIGHT: " + Input.gyro.rotationRate);
    }

}
