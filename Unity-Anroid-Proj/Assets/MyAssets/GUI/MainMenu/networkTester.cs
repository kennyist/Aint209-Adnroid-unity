using UnityEngine;
using System.Collections;

public class networkTester : MonoBehaviour {

    int i = 0;

    [RPC]
    void NETWORKTEST()
    {
        i++;
    }

    void OnGUI()
    {
        if (i != 2)
        {
            GUI.Box(new Rect(Screen.width / 2, Screen.height / 2, 100, 50), "Waiting: "+i);
        }
    }
}
