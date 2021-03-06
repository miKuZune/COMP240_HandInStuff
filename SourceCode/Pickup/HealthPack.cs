using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour {

    public int healAmount;
    Transform parent;

    PlayerHealth h;

	void Start ()
    {
        parent = transform.parent;
    }

    [PunRPC]
    public void Destroy()
    {
        if(parent != null)
        {
            parent.GetComponent<RespawnPickup>().spawned = false;
            parent.GetComponent<RespawnPickup>().respawnTimer = parent.GetComponent<RespawnPickup>().timeToRespawn;
        }
        
        PhotonNetwork.Destroy(this.gameObject);
        Destroy(this.gameObject);
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.GetComponent<PlayerHealth>().currHealth < other.gameObject.GetComponent<PlayerHealth>().maxHealth)
            {
                h = other.GetComponent<PlayerHealth>();

                h.GetComponent<PhotonView>().RPC("AddHealth", PhotonTargets.All, healAmount);

                this.GetComponent<PhotonView>().RPC("Destroy", PhotonTargets.All);
            }
        }
    }
}