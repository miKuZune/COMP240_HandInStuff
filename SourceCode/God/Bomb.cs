using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : Photon.MonoBehaviour {

    public float distToHit;
    public float damage;
    public float timeToExplode;
    public float dropAmount;
    float dropHeight;
    public bool canExplode;

	bool timerOver = false;

	float destroyTimer = 1f;
	bool destroyme = false;

	public AudioSource bombAudioSrc;
	public AudioClip bombfall;
	public AudioClip bombexplode;

    PhotonPlayer owner;

    float timer;

    public void SetOwner(PhotonPlayer newOwner)
    {
        this.owner = newOwner;
    }

	// Use this for initialization
	void Start () {
        timer = timeToExplode;

        //GetComponentInParent<DespawnObj>().timer = timer;
        if (canExplode)
        {
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
	
	// Update is called once per frame
	void Update () {
        if (!canExplode)
        {
            return;
        }
        timer -= Time.deltaTime;
		if(timer <= 0 && !timerOver)
        {
			timerOver = true;
            OnExplode();
        }
        Vector3 newPos = transform.position;
        newPos.y -= (dropHeight / timeToExplode) * Time.deltaTime;
        transform.position = newPos;

		if (destroyme) {
			destroyTimer -= Time.deltaTime;
			if (destroyTimer <= 0f) {
				PhotonView parent = GetComponentInParent<PhotonView>();
				PhotonNetwork.Destroy(this.GetComponent<PhotonView>());
				PhotonNetwork.Destroy(parent);
			}
		}
	}

    void OnExplode()
    {
		// play explode
		bombAudioSrc.clip = bombexplode;
		bombAudioSrc.Play ();

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