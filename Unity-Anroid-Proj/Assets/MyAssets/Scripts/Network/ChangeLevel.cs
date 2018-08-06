using UnityEngine;
using System.Collections;

public class ChangeLevel : MonoBehaviour {
	
	void Start () {
		DontDestroyOnLoad(this);
		networkView.group = 1;
	}

	public void changeLevel(string levelName, int levelPrefix){
		StartCoroutine(startChange(levelName,levelPrefix));
	}

	IEnumerator startChange(string levelName, int levelPrefix){
		Network.RemoveRPCsInGroup(0);
		Network.RemoveRPCsInGroup(1);
		Network.SetSendingEnabled(0, false);
		Network.isMessageQueueRunning = false;
		Network.SetLevelPrefix(levelPrefix);
		Application.LoadLevel(levelName);
		yield return new WaitForSeconds(3);
		Network.isMessageQueueRunning = true;
		Network.SetSendingEnabled(0, true);

		GameObject.FindGameObjectWithTag("Lobby").BroadcastMessage("OnNetworkLevelLoaded",SendMessageOptions.DontRequireReceiver);
        
	}
}
