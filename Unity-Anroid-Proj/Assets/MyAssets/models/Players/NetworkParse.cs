using UnityEngine;
using System.Collections;

public class NetworkParse : MonoBehaviour {

    public NetworkView lobbyObj;
    GameObject obj;
    bool running = false;
    
    void SendStartGame(GameObject g)
    {
        obj = g;
        lobbyObj.BroadcastMessage("StartGameS",SendMessageOptions.DontRequireReceiver);
        running = true;
    }

    void GetComplete()
    {
        obj.renderer.enabled = false;
        running = false;
    }

    public void sendObjDist(float dis)
    {
        lobbyObj.BroadcastMessage("sendPosBeg", dis);
    }

    public void End()
    {
        lobbyObj.BroadcastMessage("SendEnd", SendMessageOptions.DontRequireReceiver);
    }

    void OnGUI()
    {
        if (running)
        {
            GUI.skin.label.fontSize = 40;
            GUI.Label(new Rect((Screen.width / 2) - 300, (Screen.height / 2) - 100, 600, 200), "Minigame launched on android device, Cannot move intill completed");
        }
    }
}
