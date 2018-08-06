using UnityEngine;
using System.Collections;

public class LockPicking : MonoBehaviour {

    public GameObject pick;
    public GameObject target;
    public Vector3 startRot;
    public Vector3 maxRot;
    public Vector3 minRot;
    Vector3 startRotTemp;

    public Texture2D lpopen;
    public Texture2D lpclosed;

    public AudioClip pickComplete;
    public AudioClip pickFailed;

    public GUISkin skin;

    float Iconstart;

    int max, complete = 0;
    float sensitivity = 1.25f;

	void Start () {
        max = Random.Range(3, 6);
        setTargetPos();
        startRotTemp = startRot;
        Input.gyro.enabled = true;
        Iconstart = Screen.width / 2 - (Mathf.FloorToInt(max / 2) * 60) - ((max % 2) * 25);

        target.GetComponent<LockPickTarget>().collided = false;
	}

    void Reset()
    {
        max = Random.Range(3, 6);
        setTargetPos();
        Iconstart = Screen.width / 2 - (Mathf.FloorToInt(max / 2) * 60) - ((max % 2) * 25);
        complete = 0;
    }

    void setTargetPos()
    {
        target.transform.localPosition = new Vector3(0 + Random.Range(-0.30f, 0.30f), 0.5f, 0 + Random.Range(-0.30f, 0.30f));
        float scale = Random.Range(0.05f, 0.1f);
        target.transform.localScale = new Vector3(scale, 0.01f, scale);
    }
	
	void Update () {

        #if UNITY_ANDROID && !UNITY_EDITOR
        startRot.x += Input.gyro.rotationRate.x * sensitivity;
        startRot.y -= Input.gyro.rotationRate.y * sensitivity;
        #else
        startRot.x -= Input.GetAxis("Mouse Y");
        startRot.y -= Input.GetAxis("Mouse X");
        #endif
        
        startRot.x = AngleClamp(startRot.x, minRot.x, maxRot.x);
        startRot.y = AngleClamp(startRot.y, minRot.y, maxRot.y);


        pick.transform.rotation = Quaternion.Euler(startRot.x, startRot.y, startRot.z);
	}

    void OnGUI()
    {
        GUI.skin = skin;
        GUI.skin.label.fontSize = 15;

        if (GUI.Button(new Rect(Screen.width - 200, Screen.height - 200, 200, 200), "Pick"))
        {
            if (target.GetComponent<LockPickTarget>().collided)
            {
                setTargetPos();
                complete++;
                audio.PlayOneShot(pickComplete);
            }
            else
            {
                audio.PlayOneShot(pickFailed);
            }

            if (complete >= max)
            {
                GameObject.FindGameObjectWithTag("android").GetComponent<Android>().GameComplete();
            }
        }

        if (GUI.Button(new Rect(0, Screen.height - 200, 200, 200), "Reset Pick"))
        {
            startRot = startRotTemp;
        }

        float j = Iconstart;
        for (int i = 0; i < max; i++)
        {
            if (i < complete)
            {
                GUI.DrawTexture(new Rect(j, Screen.height - 75, 50, 50), lpclosed);
            }
            else
            {
                GUI.DrawTexture(new Rect(j, Screen.height - 75, 50, 50), lpopen);
            }

            j += 60;
        }

        GUI.Label(new Rect(10, 20, 200, 40), "sensitivity: "+sensitivity);
        sensitivity = GUI.VerticalSlider(new Rect(30, 75, 30, Screen.height - 300), sensitivity, 1.5f, 0.0f);
    }

    float AngleClamp(float rotation, float min, float max)
    {
        if (rotation < -360)
        {
            rotation = rotation + 360;
        }
        if (rotation > 360)
        {
            rotation = rotation - 360;
        }
        rotation = Mathf.Clamp(rotation, min, max);
        return rotation;
    }
}
