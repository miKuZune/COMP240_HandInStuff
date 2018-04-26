using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPickup : Photon.MonoBehaviour {

    public GameObject objectToSpawn;

    GameObject pack;
    GameObject paren;
    public float timeToRespawn;
    public bool spawned;

    public float respawnTimer;
	// Use this for initialization
	void Start ()
    {
        paren = GameObject.Find(this.gameObject.name);
        respawnTimer = timeToRespawn;
	}
	
    [PunRPC]
    void Spawn()
    {
        if (PhotonNetwork.isMasterClient)
        {
            Vector3 startPos = paren.transform.position;
            startPos = new Vector3(startPos.x, startPos.y + (transform.localScale.y / 2), startPos.z);
            pack = PhotonNetwork.Instantiate(this.objectToSpawn.name, startPos, Quaternion.identity, 0);
        }
    }

    // Update is called once per frame
    void Update ()
    {

        if (PhotonNetwork.isMasterClient)
        {
            if (photonView.isMine)
            {
                if (spawned == false)
                {
                    respawnTimer -= Time.deltaTime;

                    if (respawnTimer <= 0)
                    {
                        Spawn();
                        spawned = true;
                    }
                }
            }
        }
        
	}
}