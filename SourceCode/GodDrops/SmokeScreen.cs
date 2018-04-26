using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeScreen : MonoBehaviour {

    public float timeToDespawn;
    float timer;
    public bool canSmoke;
    private void Start()
    {
        timer = timeToDespawn;
    }

    // Update is called once per frame
    void Update () {
        if (!canSmoke)
        {
            this.GetComponent<ParticleSystem>().enableEmission = false;
            return;
        }


        timer -= Time.deltaTime;
        if(timer < 0)
        {
           PhotonNetwork.Destroy(this.gameObject);
        }
	}
}
