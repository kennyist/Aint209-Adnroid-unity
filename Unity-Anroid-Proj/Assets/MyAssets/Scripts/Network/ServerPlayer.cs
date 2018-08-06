using UnityEngine;
using System.Collections;
using System;

public class ServerPlayer : MonoBehaviour {

    public GameObject PlayerOBJ;
    public GameObject AndroidOBJ;
    public Texture2D black;

    public GameObject player, android;
    InteractableObject safe;

    private bool GUIstart = false;

    public NetworkView devicePairPlayer;
    public bool isDevice = false;

    float time = Time.time;
    bool show = false;
    string text;

    bool running = false;

    void Start()
    {
        if (!networkView.isMine)
        {
            enabled = false;
        }

        transform.parent = GameObject.FindGameObjectWithTag("Lobby").transform;
    }

    void StartGameS()
    {
        if (!running)
        {
            devicePairPlayer.RPC("StartGame", RPCMode.All, 1);
        }
    }

    void Update()
    {
        if (Time.time > time)
        {
            show = false;
        }
    }

    void OnNetworkLevelLoaded()
    {
        GUIstart = true;
        if (networkView.isMine)
        {
            transform.parent.gameObject.networkView.RPC("PlayerReady", RPCMode.All);
        }
    }

    public void androidLoad()
    {
        if (networkView.isMine)
        {
            transform.parent.gameObject.networkView.RPC("PlayerReady", RPCMode.All);
        }
    }

    [RPC]
    void Spawn(Vector3 spawnPos, string GUID)
    {
        if (networkView.isMine && GUID == Network.player.guid) 
        {
            GUIstart = false;
            #if UNITY_ANDROID && !UNITY_EDITOR
            android = (GameObject) Network.Instantiate(AndroidOBJ, spawnPos, Quaternion.identity, 0);
            #else
            player = (GameObject) Network.Instantiate(PlayerOBJ, spawnPos, Quaternion.identity, 0);
            player.GetComponentInChildren<NetworkParse>().lobbyObj = networkView;
            //safe = GameObject.FindGameObjectWithTag("safe").GetComponent<InteractableObject>();
            //safe.Complete += StartGameS;
            #endif
        }
    }

    void LateUpdate()
    {
        if (player != null)
        {
            //devicePairPlayer.RPC("sendPos", RPCMode.All, player.transform.Find("PlayerController").transform.position);
        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            devicePairPlayer.RPC("StartGame", RPCMode.All, 1);
        }
    }

    void ShopItem(int i)
    {
        devicePairPlayer.RPC("ShopItemRec", RPCMode.All, i);
    }

    [RPC]
    void ShopItemRec(int i)
    {
        Debug.Log("Shop: " + i);
        text = "Used inventory item "+i;
        show = true;
        time = Time.time + 3f;
    }
    
    [RPC]
    void StartGame(int game){
        if (android != null)
        {
            android.GetComponent<Android>().Spawn(game);
        }
    }

    public void GameComplete()
    {
        devicePairPlayer.RPC("GameCompleteRec", RPCMode.All);
    }

    [RPC]
    void GameCompleteRec()
    {
        running = false;
        Debug.Log("SERVER PLAYER DONE");
        text = "Objective Complete";
        show = true;
        time = Time.time + 3f;
        player.GetComponentInChildren<XPmanager>().AddXp(1500);
        player.GetComponentInChildren<ThirdPersonMovement>().enabled = true;
        player.BroadcastMessage("GetComplete",SendMessageOptions.DontRequireReceiver);
    }

    void sendPosBeg(float pos)
    {
        devicePairPlayer.RPC("sendPos", RPCMode.All, pos);
    }

    [RPC]
    void sendPos(float pos)
    {
        if(android != null){
            android.GetComponent<Android>().objective = pos;
        }
    }

    void OnGUI()
    {
        if (show)
        {
            GUI.skin.label.fontSize = 30;
            GUI.Label(new Rect((Screen.width / 2) - 200, Screen.height / 2, 400, 100), text);
        }
    }

    void SendEnd()
    {
        networkView.RPC("RecEnd", RPCMode.All);
    }

    [RPC]
    void RecEnd()
    {
        Application.Quit();
    }
}
