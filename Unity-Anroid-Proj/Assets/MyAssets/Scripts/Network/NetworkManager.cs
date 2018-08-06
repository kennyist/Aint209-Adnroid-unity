using UnityEngine;
using System.Collections;
using System;

public class NetworkManager : MonoBehaviour  {

	private string gameName = "TDHAS-NW-LOBBYTEST";
	private HostData[] hosts;
	private bool refreshHosts = false;

	public bool timedOut = false;
	private float timeOut;
	public bool ServerStarted = false;
	public bool joinedGame = false;

	public void StartServer(){
		DontDestroyOnLoad(this);
		Network.InitializeServer(3,27015,!Network.HavePublicAddress());
		MasterServer.RegisterHost(gameName,"LOBBY","lobby test");
	}

	void Update(){
		if(refreshHosts){
			if(MasterServer.PollHostList().Length > 0){
				hosts = MasterServer.PollHostList();
				refreshHosts = false;
				MatchMake();
			} else if(Time.time > timeOut){
				refreshHosts = false;
				timedOut = true;
			}
		}
	}

	void OnConnectedToServer() {
		joinedGame = true;
		Debug.Log ("joined");
	}

	void OnFailedToConnect(NetworkConnectionError error) {
		Debug.Log("Could not connect to server: " + error);
	}

	void MatchMake(){
		Network.Connect(hosts[0]);
	}

	public void RefreshHosts(){
		MasterServer.RequestHostList(gameName);
		refreshHosts = true;
		timeOut = Time.time + 10f;
	}

	void OnMasterServerEvent(MasterServerEvent mse){
		if(mse == MasterServerEvent.RegistrationFailedGameName || mse == MasterServerEvent.RegistrationFailedGameType || mse == MasterServerEvent.RegistrationFailedNoServer){
			Debug.LogError("Server registration failed");
		}
		if(mse == MasterServerEvent.RegistrationSucceeded){
			ServerStarted = true;
		}
	}

}
