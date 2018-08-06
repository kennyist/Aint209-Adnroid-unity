using UnityEngine;
using System.Collections;
using System;

public class LobbyPlayer : MonoBehaviour {

	private GameObject parent;
	private string message ="";

	void Awake(){
		if(networkView.isMine){
			parent = GameObject.FindGameObjectWithTag("Lobby");
			parent.networkView.RPC("RecPlayerName",RPCMode.All,Environment.UserName,networkView.owner);
		} else {
			enabled = false;
		}
	}

	void OnGUI(){
		message = GUI.TextField(new Rect((Screen.width / 2) - 175,Screen.height - 100,300,25),message);
		if(GUI.Button(new Rect((Screen.width / 2) + 125, Screen.height - 100, 50, 25),"Send")){
			if(message != ""){
				parent.networkView.RPC("AddChatMessage",RPCMode.All,message,networkView.owner);
				message = "";
			}
		}
	}
}
