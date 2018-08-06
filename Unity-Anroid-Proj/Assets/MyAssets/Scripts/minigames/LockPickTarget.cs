using UnityEngine;
using System.Collections;

public class LockPickTarget : MonoBehaviour {

    public bool collided = false;

    void OnCollisionEnter()
    {
        collided = true;
	}

    void OnCollisionExit()
    {
        collided = false;
    }
}
