using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeScreen : MonoBehaviour {
    //Variables.
    public float timeToDespawn;
    float timer;
    public bool canSmoke;

    private void Start()
    {
        timer = timeToDespawn;
    }

    void Update () {
        //Don't do anything if spawned by god.
        if (!canSmoke)
        {
            this.GetComponent<ParticleSystem>().enableEmission = false;
            return;
        }
        //Destroy the smoke after a certain time.
        timer -= Time.deltaTime;
        if(timer < 0)
        {
           PhotonNetwork.Destroy(this.gameObject);
        }
	}
}
