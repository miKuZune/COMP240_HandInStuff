using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Stun : MonoBehaviour {

    public float distToHit;
    public float timeToExplode;
    public float stunDuration;
    public bool canExplode;

    Text UI;

    float explodeTimer;
    float stunTimer;

    float startSpeed;
    float startSensX;
    float startSensY;

    bool hasExploded;
	// Use this for initialization
	void Start () {
        explodeTimer = timeToExplode;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        startSpeed = 4;
        //startSensX = players[0].GetComponent<CameraLook>().baseSensitivity;
        //startSensY = players[0].GetComponent<CameraLook>().baseSensitivity * 90;
        stunTimer = stunDuration;

        UI = GameObject.Find("StunnedUI").GetComponent<Text>();
        GetComponentInChildren<ParticleSystem>().enableEmission = false;
       }

    // Update is called once per frame
    void Update () {
        if (hasExploded)
        {
            stunTimer -= Time.deltaTime;
        }
        else
        {
            explodeTimer -= Time.deltaTime;
        }

        if(stunTimer <= 0)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject player in players)
            {
                float dist = Vector3.Distance(transform.position, player.transform.position);

                if (dist <= distToHit)
                {
                    player.GetComponent<PlayerMovement>().isStunned = false;
                    UI.text = "";
                }
            }

            if(stunTimer <= -0.5f)
            {
                PhotonNetwork.Destroy(gameObject.GetComponent<PhotonView>());
            }
        }
        if(explodeTimer <= 0 && !hasExploded)
        {
            OnStunExplode();
        }
	}

    void OnStunExplode()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player.GetComponent<PhotonView>().isMine)
        {
            float dist = Vector3.Distance(transform.position, player.transform.position);

            if (dist <= distToHit)
            {
                player.GetComponent<PlayerMovement>().isStunned = true;
                UI.text = "Stunned";
            }
            GetComponentInChildren<ParticleSystem>().enableEmission = true;
        }

        stunTimer = stunDuration;
        hasExploded = true;
    }
    
}
