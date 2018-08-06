using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public int playersOnLaunch = 1;
    private int loadedPlayers = 0;

    public ArrayList players = new ArrayList();
    public ArrayList playerNames = new ArrayList();
    public ArrayList playerDevice = new ArrayList();

    [RPC]
    void PlayerReady()
    {
        Debug.Log("ready");
        loadedPlayers++;
        if (loadedPlayers >= playersOnLaunch && Network.isServer)
        {
            int j = 0;

            for (int i = 0; i < playersOnLaunch; i++)
            {
                if (playerDevice[i].ToString() == "0")
                {
                    transform.GetChild(i).networkView.RPC("Spawn", RPCMode.All, GameObject.FindGameObjectsWithTag("SpawnPoint")[i].transform.position, players[i].ToString());
                }
                else
                {
                    transform.GetChild(i).networkView.RPC("Spawn", RPCMode.All, GameObject.FindGameObjectsWithTag("SpawnPoint")[j].transform.position, players[i].ToString());
                    j++;
                }
            }
        }
    }

    void OnGUI()
    {
        //GUI.Box(new Rect(300, 300, 100, 200), ""+loadedPlayers+" : "+playersOnLaunch);
    }
}
