using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SlowField : Photon.MonoBehaviour {

    public float distToHit;
    public float speedWhileSlowed;

    private float normalSpeed = 4;

    public float timeToDespawn;
    float timer;

    Text UI;

    void OnDespawn()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        float dist = Vector3.Distance(transform.position, player.transform.position);
        if(dist <= distToHit)
        {
            player.GetComponent<PlayerMovement>().speed = normalSpeed;
            UI.text = "";
        }
    }

	// Use this for initialization
	void Start () {
        timer = timeToDespawn;
        UI = GameObject.FindGameObjectWithTag("StunnedUI").GetComponent<Text>();
    }
	// Update is called once per frame
	void Update () {


        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player.GetComponent<PhotonView>().isMine)
        {
            float dist = Vector3.Distance(transform.position, player.transform.position);

            if (dist <= distToHit)
            {
                player.GetComponent<PlayerMovement>().speed = speedWhileSlowed;
                UI.text = "Slowed";
            }
            else
            {
                player.GetComponent<PlayerMovement>().speed = normalSpeed;
                UI.text = "";
            }
        }

        if(timer <= 0)
        {
            OnDespawn();
            PhotonNetwork.Destroy(this.gameObject);
        }
        timer -= Time.deltaTime;
	}
}
