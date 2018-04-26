using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : Photon.MonoBehaviour 
{
	///IF ITS TRUE ITS RED IF ITS FALSE ITS BLUE
	public void Fire(string projectileName, GameObject bulletSpawnPosition, float speed, Vector2 accuracy, PhotonPlayer owner, int dmg, bool team, int playerID)
    {

			
		GameObject bullet = PhotonNetwork.Instantiate ("Models/items/" + projectileName, bulletSpawnPosition.transform.position, bulletSpawnPosition.transform.rotation, 0) as GameObject;

			Vector3 finalAccuracy = new Vector3 (Random.Range (-accuracy.x, accuracy.x), Random.Range (-accuracy.y, accuracy.y));
			bullet.GetComponent<PhotonView> ().RPC ("SetInfo", PhotonTargets.All, PhotonNetwork.player, dmg, bulletSpawnPosition.transform.forward * speed,
			bulletSpawnPosition.transform.TransformDirection (finalAccuracy), team, playerID);
        
		 	

    }
}
