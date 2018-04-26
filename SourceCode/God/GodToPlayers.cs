﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodToPlayers : MonoBehaviour {


    GameObject[] players = new GameObject[20];

	// Use this for initialization
	void Start () {
		
	}


    void GetPlayers()
    {
        GameObject[] temp = new GameObject[players.Length];
        temp = GameObject.FindGameObjectsWithTag("Player");

        int notGodIndex = 0;
        for (int i = 0; i < temp.Length; i++)
        {
            if (!temp[i].GetComponent<GodSetup>().isGod)
            {
                players[notGodIndex] = temp[i];
                notGodIndex++;
            }
        }
    }

    public void GoToPlayer(int playerIndex)
    {
        GameObject god = GameObject.FindGameObjectWithTag("Player");

        if(god.GetComponent<GodLookAandMove>().movingToPlayer)
        {
            god.GetComponent<GodLookAandMove>().movingToPlayer = false;
            god.GetComponent<GodLookAandMove>().canMove = true;
        }
        else
        {
            GetPlayers();

            god.GetComponent<GodLookAandMove>().canMove = false;
            god.GetComponent<GodLookAandMove>().movingToPlayer = true;
            god.GetComponent<GodLookAandMove>().moveToPlayerPos = players[playerIndex];

        }


    }
	// Update is called once per frame
	void Update () {
		
	}
}
