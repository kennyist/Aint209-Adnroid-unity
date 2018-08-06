using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Lobby : MonoBehaviour {

	public GameObject lobbyPlayerObj;
    public GUISkin skin;

    GameObject playerOBJ; 
    NetworkView pairedOBJ;

    ArrayList playerGUID = new ArrayList();
    ArrayList playerNames = new ArrayList();
    ArrayList playerReady = new ArrayList();
    ArrayList playerDevice = new ArrayList();
    ArrayList playerConnectedDevice = new ArrayList();

    bool android = false, lobby = false, pairfound = false, allReady = false, end = false;

    string codeField = "";
    string[] codeEnterKeys = new string[11] {"1","2","3","4","5","6","7","8","9","","0"};
    int codeSel = -1, code, returnCode;

    float countDown = -1f;
    string pairedName, pairedGUID, name;

    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
        code = Random.Range(1000, 9999);
        returnCode = Random.Range(10000, 99999);
    }

    // CLIENT


	void OnConnectedToServer()
    {
        playerOBJ = (GameObject) Network.Instantiate(lobbyPlayerObj, transform.position, transform.rotation, 0);
        name = System.Environment.UserName;
        int isAndroid = 0;

        #if UNITY_ANDROID && !UNITY_EDITOR
            isAndroid = 1;
            android = true;
            name = SystemInfo.deviceModel+":"+Random.Range(1, 99);
            code = 0;
        #endif

        networkView.RPC("addPlayer", RPCMode.All, name, isAndroid);
        Debug.Log("sent");

        lobby = true;
	}

    void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        playerGUID.Clear();
        playerNames.Clear();
        playerReady.Clear();
        playerDevice.Clear();
        playerConnectedDevice.Clear();
    }
	

    // SERVER
    [RPC]
    void log(string log)
    {
        Debug.Log(log);
    }

    [RPC]
    void addPlayer(string name, int isAndroid, NetworkMessageInfo info)
    {
        if (Network.isServer)
        {
            int pos = playerGUID.IndexOf(info.sender.guid);

            playerNames[pos] = name;
            playerDevice[pos] = isAndroid;

            if (isAndroid == 1)
            {
                playerReady[pos] = 1;
            }

            sync();
        }
    }

    void OnServerInitialized(NetworkPlayer player)
    {
        lobby = true;
        playerOBJ = (GameObject)Network.Instantiate(lobbyPlayerObj, transform.position, transform.rotation, 0);
        playerGUID.Add(player.guid);
        playerNames.Add(System.Environment.UserName);
        playerReady.Add(0);
        playerDevice.Add(0);
        playerConnectedDevice.Add(0);
    }

    void OnPlayerConnected(NetworkPlayer player)
    {
        playerGUID.Add(player.guid);
        playerNames.Add("");
        playerReady.Add(0);
        playerDevice.Add(0);
        playerConnectedDevice.Add(0);
    }

	void OnPlayerDisconnected(NetworkPlayer player){
        if (Network.isServer)
        {
            foreach (NetworkPlayer p in Network.connections)
            {
                Network.DestroyPlayerObjects(p);
                Network.CloseConnection(p, true);
            }
        }

        int pos = playerGUID.IndexOf(player.guid);

        playerGUID.RemoveAt(pos);
        playerNames.RemoveAt(pos);
        playerReady.Remove(pos);
        playerDevice.RemoveAt(pos);
        playerConnectedDevice.RemoveAt(pos);

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("LobbyPlayer"))
        {
            GameObject.Destroy(obj);
        }

        Network.Disconnect();
        lobby = false;
	}

    // --------------

    [RPC]
    void LeaveGame()
    {
        OnPlayerDisconnected(networkView.owner);
    }

	void sync(){
        if (Network.isServer)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream guid = new MemoryStream();
            MemoryStream name = new MemoryStream();
            MemoryStream ready = new MemoryStream();
            MemoryStream device = new MemoryStream();
            MemoryStream connect = new MemoryStream();

            bf.Serialize(guid, playerGUID);
            string guids = System.Convert.ToBase64String(guid.GetBuffer());

            bf.Serialize(name, playerNames);
            string names = System.Convert.ToBase64String(name.GetBuffer());

            bf.Serialize(ready, playerReady);
            string readys = System.Convert.ToBase64String(ready.GetBuffer());

            bf.Serialize(device, playerDevice);
            string devices = System.Convert.ToBase64String(device.GetBuffer());

            bf.Serialize(connect, playerConnectedDevice);
            string connected = System.Convert.ToBase64String(connect.GetBuffer());

            networkView.RPC("recSync", RPCMode.All, guids, names, readys, devices, connected);
        }
	}

    [RPC]
    void recSync(string guid, string names, string ready, string dev, string connected)
    {
        if (Network.isClient)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream guids = new MemoryStream(System.Convert.FromBase64String(guid));
            MemoryStream name = new MemoryStream(System.Convert.FromBase64String(names));
            MemoryStream devices = new MemoryStream(System.Convert.FromBase64String(dev));
            MemoryStream connect = new MemoryStream(System.Convert.FromBase64String(connected));
            MemoryStream readys = new MemoryStream(System.Convert.FromBase64String(ready));

            playerGUID = (ArrayList) bf.Deserialize(guids);
            playerNames = (ArrayList)bf.Deserialize(name);
            playerReady = (ArrayList) bf.Deserialize(readys);
            playerDevice = (ArrayList)bf.Deserialize(devices);
            playerConnectedDevice = (ArrayList)bf.Deserialize(connect);
        }
    }

    void Update()
    {
        if (codeSel != -1)
        {
            if (codeField.Length == 4)
            {
                codeField = "";
            }
            if (codeSel != 9)
            {
                codeField = codeField + "" + codeEnterKeys[codeSel];
            }
            codeSel = -1;
        }

        if (codeField.Length == 4 && !pairfound)
        {
            sendCodeCheck();
            codeField = "";
        }
        
        int count = 0;

        for (int i = 0; i < playerGUID.Count; i++)
        {
            if (playerReady[i].ToString() == "1")
            {
                count++;
            }
        }

        if (count == playerGUID.Count && playerGUID.Count > 0 && !allReady)
        {
            allReady = true;
            countDown = 10f;
        }
        else if (count < playerGUID.Count)
        {
            allReady = false;
            countDown = -1f;
        }

        if (countDown > 0 && countDown != -1 && !end)
        {
            countDown -= Time.deltaTime;
        }

        if(countDown < 0f && countDown != -1f && !end){
            end = true;
            lobby = false;
            android = false;
            gameObject.GetComponent<GameManager>().playersOnLaunch = playerGUID.Count;
            gameObject.GetComponent<GameManager>().playerNames = playerNames;
            gameObject.GetComponent<GameManager>().playerDevice = playerDevice;
            gameObject.GetComponent<GameManager>().players = playerGUID;
            playerOBJ.GetComponent<ServerPlayer>().devicePairPlayer = pairedOBJ;
            #if UNITY_STANDALONE
            gameObject.GetComponent<ChangeLevel>().changeLevel("aint209demo", 1);
#else
            gameObject.GetComponent<ChangeLevel>().changeLevel("empty", 2);
#endif
            enabled = false; 
        }
    }

    void sendCodeCheck()
    {
        networkView.RPC("log", RPCMode.All, Network.player.guid+":"+networkView.owner.guid+":"+playerGUID[playerNames.IndexOf(name)]);
        networkView.RPC("CodeCheck", RPCMode.All, codeField, returnCode, Network.player.guid, playerGUID[playerNames.IndexOf(name)], playerOBJ.networkView.viewID);
    }

    [RPC]
    void CodeCheck(string scode, int returncode, string nwvid, string indid, NetworkViewID id, NetworkMessageInfo info)
    {
        if (scode == ""+code)
        {
            networkView.RPC("log", RPCMode.All, nwvid+" : "+indid);
            networkView.RPC("log", RPCMode.All, "ttt: "+Network.player.guid + ":" + networkView.owner.guid);
            networkView.RPC("gotPair", RPCMode.All, Network.player.guid, returncode, playerOBJ.networkView.viewID);

            pairedOBJ = NetworkView.Find(id);
        }
    }

    [RPC]
    void gotPair(string sender, int returncode, NetworkViewID id, NetworkMessageInfo info)
    {
        if (returncode == returnCode)
        {
            networkView.RPC("log", RPCMode.All, sender);
            networkView.RPC("completePair",RPCMode.Server, sender, playerGUID[playerNames.IndexOf(name)]);

            pairedOBJ = NetworkView.Find(id);
            pairedGUID = sender;
            pairedName = playerNames[playerGUID.IndexOf(sender)].ToString();
            pairfound = true;
        }
    }

    [RPC]
    void completePair(string player, string device)
    {
        networkView.RPC("log", RPCMode.All, "p: "+player+" s: "+device);
        int pos = playerGUID.IndexOf(player);
        playerConnectedDevice[pos] = device;
        sync();
    }

	void OnGUI(){

        GUI.skin = skin;

        if (android && lobby)
        {
            if (!pairfound)
            {
                
                GUI.Label(new Rect((Screen.width / 2) - 200, (Screen.height / 2) - 1500, 400, 30), "Enter device code to pair");
                GUI.Label(new Rect((Screen.width / 2) - 100, (Screen.height / 2) - 100, 200, 30), "" + codeField);

                codeSel = GUI.SelectionGrid(new Rect(400, Screen.height - 250, Screen.width - 900, 200), codeSel, codeEnterKeys, 3);
            }
            else
            {
                /*if (pairedOBJ != null)
                {
                    GUI.Label(new Rect((Screen.width / 2) - 200, 100, 400, 50), pairedOBJ.transform.name);
                }*/
                GUI.Label(new Rect((Screen.width / 2) - 250, Screen.height / 2, 500, 40), "You have been paired with " + pairedName);
            }

        } else if (lobby) {
            /*if (pairedOBJ != null)
            {
                GUI.Label(new Rect((Screen.width / 2) - 200, 100, 400, 50), pairedOBJ.transform.name);
            }*/

            GUI.Label(new Rect((Screen.width / 2) - 200, 100, 400, 50), NetworkView.Find(networkView.viewID).name);

            GUI.Label(new Rect((Screen.width / 2) - 200, Screen.height - 100, 400, 50), "Device pair code " + code);

            if (allReady)
            {
                GUI.Label(new Rect((Screen.width / 2) - 200, Screen.height - 50, 400, 50), "STARTING IN " + countDown.ToString("0"));
            }

            GUI.Label(new Rect(10, 40, 200, 30), playerGUID.Count + ":" + playerNames.Count +":"+ playerReady.Count +":" + playerDevice.Count + ":" + playerConnectedDevice.Count);
            GUI.Label(new Rect(10, 10, 200, 30), "CONNECTED");

            int y = 150;
            for(int i = 0; i < playerGUID.Count; i++){
                string title = playerNames[i].ToString();

                if (playerReady[i].ToString() == "1") { title = title + " -- Ready"; } else { title = title + " -- Not Ready"; }
                if (playerConnectedDevice[i].ToString() != "0") { title = title + " -- Device paried: " + playerNames[playerGUID.IndexOf(playerConnectedDevice[i])]; }

                if (playerDevice[i].ToString() != "1")
                {
                    GUI.Label(new Rect(250, y, 900, 30), title);
                    y = y + 50;
                }
            }
        }
    }

    void Ready()
    {
        Debug.Log("ready "+networkView.owner.guid);
        networkView.RPC("recReady", RPCMode.All, networkView.owner.guid);
    }

    [RPC]
    void recReady(string id, NetworkMessageInfo info)
    {
        if (Network.isServer)
        {

            Debug.Log("got ready "+id+"::"+info.sender.guid);
            int pos;

            if (id == "" || id == null)
            {
                pos = playerGUID.IndexOf(info.sender.guid);
            }
            else
            {
                pos = playerGUID.IndexOf(id);
            }


            if (playerReady[pos].ToString() == "0")
            {
                playerReady[pos] = 1;
            }
            else
            {
                playerReady[pos] = 0;
            }
            sync();
        }
    }
}
