using UnityEngine;
using System.Collections;

public class Android : MonoBehaviour {

    public Texture2D divider;

    public int menuItemsCnt = 6;
    public float menuHeight = 300;
    float menuItemSize;

    public GUISkin skin;
    public Vector3 pos;
    public float objective;

    public GameObject LockpickingGame;
    GameObject temp;

    bool test = false, menu = true;

    void Start()
    {
        if (!networkView.isMine)
        {
            enabled = false;
        }

        menuItemSize = Screen.width / menuItemsCnt - 1f;
        Debug.Log(menuItemSize);
    }

	void OnGUI () {
        GUI.skin = skin;

        if (menu)
        {
            GUI.skin.label.fontSize = 60;
            GUI.Label(new Rect(50, 200, 1200, 200), "OBJECTIVE DISTANCE: " + objective.ToString("0.00"));

            float y = Screen.height - menuHeight, x = 0;

            if (GUI.Button(new Rect(x, y, menuItemSize, menuHeight), "item 1", "ShopButton"))
            {
                GameObject.FindGameObjectWithTag("Lobby").BroadcastMessage("ShopItem", 1, SendMessageOptions.RequireReceiver);
            }
            x += menuItemSize;
            GUI.DrawTexture(new Rect(x, y, 2, menuHeight), divider);
            x += 2;
            
            if (GUI.Button(new Rect(x, y, menuItemSize, menuHeight), "item 2", "ShopButton"))
            {
                GameObject.FindGameObjectWithTag("Lobby").BroadcastMessage("ShopItem", 2, SendMessageOptions.RequireReceiver);
            }
            x += menuItemSize;
            GUI.DrawTexture(new Rect(x, y, 2, menuHeight), divider);
            x += 2;

            if (GUI.Button(new Rect(x, y, menuItemSize, menuHeight), "item 3", "ShopButton"))
            {
                GameObject.FindGameObjectWithTag("Lobby").BroadcastMessage("ShopItem", 3, SendMessageOptions.RequireReceiver);
            }
            x += menuItemSize;
            GUI.DrawTexture(new Rect(x, y, 2, menuHeight), divider);
            x += 2;

            if (GUI.Button(new Rect(x, y, menuItemSize, menuHeight), "item 4", "ShopButton"))
            {
                GameObject.FindGameObjectWithTag("Lobby").BroadcastMessage("ShopItem", 4, SendMessageOptions.RequireReceiver);
            }
            x += menuItemSize;
            GUI.DrawTexture(new Rect(x, y, 2, menuHeight), divider);
            x += 2;

            if (GUI.Button(new Rect(x, y, menuItemSize, menuHeight), "item 5", "ShopButton"))
            {
                GameObject.FindGameObjectWithTag("Lobby").BroadcastMessage("ShopItem", 5, SendMessageOptions.RequireReceiver);
            }
            x += menuItemSize;
            GUI.DrawTexture(new Rect(x, y, 2, menuHeight), divider);
            x += 2;

            if (GUI.Button(new Rect(x, y, menuItemSize, menuHeight), "item 6", "ShopButton"))
            {
                GameObject.FindGameObjectWithTag("Lobby").BroadcastMessage("ShopItem", 6, SendMessageOptions.RequireReceiver);
            }
            x += menuItemSize;
            GUI.DrawTexture(new Rect(x, y, 2, menuHeight), divider);
            x += 2;

        }
	}

    public void Spawn(int i)
    {
        if (i == 1)
        {
            temp = (GameObject) GameObject.Instantiate(LockpickingGame);
            menu = false;
        }
    }

    public void GameComplete()
    {
        menu = true;
        GameObject.Destroy(temp); 
        GameObject.FindGameObjectWithTag("Lobby").BroadcastMessage("GameComplete");
    }
}
