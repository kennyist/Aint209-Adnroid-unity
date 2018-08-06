using UnityEngine;
using System.Collections;

public class MoveTo : MonoBehaviour {

    public GameObject location;
    public float speed;
    Vector3 start;
    Vector3 end;

    bool isEnd = false;
    float startTime;

	void Start () {
        start = gameObject.transform.position;
        end = location.transform.position;
        end.y = start.y;
        startTime = Time.time;
	}
	
	void Update () {
        if (isEnd)
        {
            gameObject.transform.position = Vector3.Lerp(end, start, (Time.time - startTime) / speed); 

            if (gameObject.transform.position == start)
            {
                gameObject.transform.Rotate(Vector3.forward, 180);
                startTime = Time.time;
                isEnd = false;
            }
        }
        else
        {
            gameObject.transform.position = Vector3.Lerp(start, end, (Time.time - startTime) / speed);

            if (gameObject.transform.position == end)
            {
                gameObject.transform.Rotate(Vector3.forward, 180);
                startTime = Time.time;
                isEnd = true;
            }
        }
	}
}
