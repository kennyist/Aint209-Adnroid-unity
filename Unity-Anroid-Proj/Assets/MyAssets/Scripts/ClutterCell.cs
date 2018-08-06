using UnityEngine;
using System.Collections;

public class ClutterCell : MonoBehaviour {

    public enum State { open, closed }
    public State state = State.open;

    float time;
    bool sent = false, started = false;

    GameObject spawner;

    public void TestCollision(GameObject spawnerOBJ)
    {
        started = true;
        spawner = spawnerOBJ;
        time = Time.time + 0.02f;
        gameObject.AddComponent<Rigidbody>().useGravity = false;
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    void Update()
    {
        if (Time.time > time && !sent && started)
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
            rigidbody.detectCollisions = false;
            spawner.GetComponent<Clutter>().Add();
            sent = true;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        state = State.closed;
        if (!sent && started)
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
            rigidbody.detectCollisions = false;
            spawner.GetComponent<Clutter>().Add();
            sent = true;
        }
    }
}
