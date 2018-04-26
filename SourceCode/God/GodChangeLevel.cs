using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodChangeLevel : MonoBehaviour {
    //Variables to define the layers of the map.
    public GameObject[] layers;
	GameObject player;
	int current;

	float startCooldownTimer;
	float roundTimer;
	float roundCooldownTimer;

	void Start ()
    {
		current = 0;
		player = GameObject.FindGameObjectWithTag("Player");
		GetTimers();
	}
    //Set all the layers of the map to be visible again.
	void RoundReset()
    {
		foreach (GameObject layer in layers) {
			layer.SetActive (true);
		}
		current = 0;

	}
	//Get round timers.
	void GetTimers()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		startCooldownTimer = player.GetComponent<Rounds>().gameStartCountdown;
		roundTimer = player.GetComponent<Rounds>().roundLengthInSecs;
		roundCooldownTimer = player.GetComponent<Rounds>().roundFinishCooldown;
	}
    //Show the level above the current one.
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
    //Show the level beneath the current one.
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

	void Update ()
    {
        //Count down the initial start time for the game to start.
		if (startCooldownTimer > 0) {
			startCooldownTimer -= Time.deltaTime;
		} else if (roundTimer > 0) { //Then count down the round time.
			roundTimer -= Time.deltaTime;
		} else {//After the round is over reset.
			RoundReset ();
			roundCooldownTimer -= Time.deltaTime;
			if (roundCooldownTimer < 0) {
				GetTimers ();
			}
		}
        //Gives gods the ability to swap between layers using keybindings.
		if (player.GetComponent<GodSetup> ().isGod) {
			if (Input.GetKeyDown (KeyCode.Q)) {
				OnDownLevel ();
			} else if (Input.GetKeyDown (KeyCode.E)) {
				OnUpLevel ();
			}
		}
	}
}
