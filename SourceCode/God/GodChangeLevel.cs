using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodChangeLevel : MonoBehaviour {

    public GameObject[] layers;
	GameObject player;
	int current;

	float startCooldownTimer;
	float roundTimer;
	float roundCooldownTimer;
	// Use this for initialization
	void Start () {
		current = 0;
		player = GameObject.FindGameObjectWithTag("Player");
		GetTimers();
	}

	void RoundReset(){
		foreach (GameObject layer in layers) {
			layer.SetActive (true);
		}
		current = 0;

	}
		
	void GetTimers()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		startCooldownTimer = player.GetComponent<Rounds>().gameStartCountdown;
		roundTimer = player.GetComponent<Rounds>().roundLengthInSecs;
		roundCooldownTimer = player.GetComponent<Rounds>().roundFinishCooldown;
	}


    public void OnUpLevel()
    {
        Debug.Log(current);
        int check = current - 1;
        
        layers[current].SetActive(true);

        if (check < 0)
        {

            return;
        }

        current--;
    }

    public void OnDownLevel()
    {
        Debug.Log(current);
        int check = current + 1;
        if (check > layers.Length - 1)
        {
            return;
        }

        layers[current].SetActive(false);

        current++;
    }

	// Update is called once per frame
	void Update () {
		if (startCooldownTimer > 0) {
			startCooldownTimer -= Time.deltaTime;
		} else if (roundTimer > 0) {
			roundTimer -= Time.deltaTime;
		} else {
			RoundReset ();
			roundCooldownTimer -= Time.deltaTime;
			if (roundCooldownTimer < 0) {
				GetTimers ();
			}
		}
		if (player.GetComponent<GodSetup> ().isGod) {
			if (Input.GetKeyDown (KeyCode.Q)) {
				OnDownLevel ();
			} else if (Input.GetKeyDown (KeyCode.E)) {
				OnUpLevel ();
			}
		}
	}
}
