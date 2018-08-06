using UnityEngine;
using System.Collections;

public class ObjectiveTest : MonoBehaviour {

    public Camera cam;
    public Texture icon;
    public Texture iconb;
    GameObject exit;
    GameObject[] objectiveObj;

	void Start () {
        objectiveObj = (GameObject[])GameObject.FindGameObjectsWithTag("safe");
        exit = (GameObject) GameObject.FindGameObjectWithTag("exit");

        
        #if UNITY_ANDROID && !UNITY_EDITOR
        enabled = false;
        #endif
        
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            objectiveObj = (GameObject[])GameObject.FindGameObjectsWithTag("safe");
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            foreach (GameObject g in objectiveObj)
            {
                g.renderer.enabled = false;
            }
        }
    }
	
	void OnGUI () {
        GUI.skin.label.fontSize = 20;

        if (objectiveObj != null && cam != null && icon != null)
        {
            int y = 30, i = 0;

            foreach (GameObject g in objectiveObj)
            {
                Vector3 screenPos = cam.WorldToScreenPoint(g.transform.position);
                float dis = Vector3.Distance(transform.position, g.transform.position);

                float size = 50 * (dis / 100);
                if (size < 25) { size = 25; }
                if (size > 50) { size = 50; }

                if (i == 0)
                {
                    gameObject.GetComponent<NetworkParse>().sendObjDist(dis);
                }

                if (g.renderer.enabled)
                {
                    if (screenPos.z > 0)
                    {
                        GUI.DrawTexture(new Rect(screenPos.x, (Screen.height - screenPos.y) - 30, size, size), icon);
                    }

                    GUI.DrawTexture(new Rect(30, y, 30, 30), icon);
                    GUI.Label(new Rect(70, y, 500, 30), "Steal the documents");

                    y += 50;
                    i++;
                }
            }

            if (i == 0)
            {
                gameObject.GetComponent<NetworkParse>().sendObjDist(0);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    gameObject.GetComponent<NetworkParse>().End();
                }

                GUI.DrawTexture(new Rect(30, y, 30, 30), iconb);
                GUI.Label(new Rect(70, y, 500, 30), "Escape -- Press E to exit");
            }
        }
	}
}
