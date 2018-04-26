﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Photon.PunBehaviour {

    public int damage;
    public PhotonPlayer owner;
    public float destroyAfterSeconds = 3f;
	public int bulletPlayerID = 0;


    void Update()
    {
        if (photonView.isMine)
        {
            destroyAfterSeconds -= Time.deltaTime;
            if (destroyAfterSeconds <= 0f)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

 /*   void OnCollisionEnter(Collision wank)
    {
        if (wank.collider.gameObject.CompareTag("Player"))
        {
            if(wank.collider.gameObject.GetComponent<PhotonView>().owner.GetTeam() == owner.GetTeam())
            {
                wank.collider.gameObject.GetComponent<PlayerHealth>().lastToDamageMe = owner;
                wank.collider.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.All, damage);
                PhotonNetwork.Destroy(gameObject);

            }
        }
    }
    */
    [PunRPC]
	public void SetInfo(PhotonPlayer Owner, int Damage, Vector3 force1, Vector3 force2, bool team, int playerID)
    {
        owner = Owner;
        damage = Damage;
		GetComponent<Rigidbody> ().AddForce (force1);
		GetComponent<Rigidbody> ().AddForce (force2);
        
        GetComponent<PlayerProfile>().red = team;

        if(GetComponent<PlayerProfile>().red == true)
        {
        GetComponent<PlayerProfile>().blue = false;

        }else
        {
            GetComponent<PlayerProfile>().blue = true;
        }
  			
		bulletPlayerID = playerID;
    }
}
