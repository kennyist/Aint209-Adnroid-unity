using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	public GameObject playerOBJ;
	GameObject spawnPoint;

	/*void OnNetworkLevelLoaded(){
		spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
		Debug.Log ("changed");
        Network.Instantiate(playerOBJ, new Vector3(0f, 3f, -28.25f), Quaternion.identity, 0);
        networkView.RPC("NETWORKTEST", RPCMode.All);
	}*/
}
