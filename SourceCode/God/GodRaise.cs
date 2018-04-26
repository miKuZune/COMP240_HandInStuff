using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodRaise : Photon.MonoBehaviour {
    //Script has been discontinued
    private GameObject pillar;
    private float timer;
    private bool isSpawned;
	// Use this for initialization
	void Start () {
        isSpawned = false;
	}

    void SpawnGround()
    {
        if (isSpawned == false)
        {
            pillar = PhotonNetwork.Instantiate("Thingy", transform.position, Quaternion.identity, 0);
            timer = 1.5f;
            isSpawned = true;
        }
        else
        {
            Debug.Log("Already spawned");
        }
    }

    void CheckForDespawn()
    {
        if(timer <= 0)
        {
            PhotonNetwork.Destroy(pillar);
            isSpawned = false;
        }
        timer -= Time.deltaTime;
    }

	// Update is called once per frame
	void Update () {
        if (!photonView.isMine)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SpawnGround();
        }

        if (isSpawned)
        {
            CheckForDespawn();
        }

	}
}
