﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnObj : MonoBehaviour {

    //To despawn any obj that dosn't despawn naturally.

    public float timer;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
            if (timer < 0)
            {
                PhotonNetwork.Destroy(GetComponent<PhotonView>());
            }
            timer -= Time.deltaTime;
	}
}
