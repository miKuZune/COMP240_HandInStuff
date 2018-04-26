using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : Photon.MonoBehaviour {

    //Variables for chaning how the bomb acts
    public float distToHit;
    public float damage;
    public float timeToExplode;
    public float dropAmount;
    float dropHeight;

    //Variables used for making the bomb work.
    public bool canExplode;

	bool timerOver = false;

	float destroyTimer = 1f;
	bool destroyme = false;

	public AudioSource bombAudioSrc;
	public AudioClip bombfall;
	public AudioClip bombexplode;

    PhotonPlayer owner;

    float timer;

    //Store the player's network details.
    public void SetOwner(PhotonPlayer newOwner)
    {
        this.owner = newOwner;
    }

	void Start () {
        //Set the timer so that it can count to when the bomb should explode.
        timer = timeToExplode;

        //Checks if the bomb can explode. This is used to stop the bomb exploding when havoc players are choosing where to place the bomb.
        if (canExplode)
        {
            //Math to drop the bomb from a certain height and detonate when it hits the ground.
            dropHeight = timeToExplode * dropAmount;
            Vector3 newPos = transform.position;
            newPos.y += dropHeight;
            transform.position = newPos;
        }
        //Set owner
        owner = photonView.owner;
		//play falling sound
		bombAudioSrc.clip = bombfall;
		bombAudioSrc.Play ();
    }
	
	void Update () {
        //Stop this method from running if the god is choosing where to place the bomb.
        if (!canExplode)
        {
            return;
        }
        //Move the bomb downwards and count down until the bomb should deal damage. 
        timer -= Time.deltaTime;
		if(timer <= 0 && !timerOver)
        {
			timerOver = true;
            OnExplode();
        }
        Vector3 newPos = transform.position;
        newPos.y -= (dropHeight / timeToExplode) * Time.deltaTime;
        transform.position = newPos;

        //Destroy the object once the bomb has exploded.
		if (destroyme) {
			destroyTimer -= Time.deltaTime;
			if (destroyTimer <= 0f) {
				PhotonView parent = GetComponentInParent<PhotonView>();
				PhotonNetwork.Destroy(this.GetComponent<PhotonView>());
				PhotonNetwork.Destroy(parent);
			}
		}
	}

    //Find the player's within hit distance of the bomb and deal damage to them.
    void OnExplode()
    {
		// play explode
		bombAudioSrc.clip = bombexplode;
		bombAudioSrc.Play ();
        //Only run this code on the master client.
        if (!PhotonNetwork.isMasterClient)
        {
            return;
        }


        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for(int i = 0; i < players.Length; i++)
        {
            float dist = Vector3.Distance(transform.position, players[i].transform.position);

            if(dist <= distToHit)
            {
                players[i].GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.All, damage);
                players[i].GetComponent<PhotonView>().RPC("SetLastToDamage", PhotonTargets.All, owner);
                GameObject.Find("Debugger").GetComponent<Text>().text = players[i].GetComponent<PlayerHealth>().lastToDamageMe.NickName;
            }
        }
        
		destroyme = true;
    }
}