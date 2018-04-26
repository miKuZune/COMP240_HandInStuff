using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ticket : MonoBehaviour {

    public float scoreIncrease;

	void OnPickup()
    {
        //Add code here for adding to the players score/ whatever we are doing on ticketpickup.
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            
            GetComponentInParent<RespawnPickup>().spawned = false;
            GetComponentInParent<RespawnPickup>().respawnTimer = GetComponentInParent<RespawnPickup>().timeToRespawn;
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
