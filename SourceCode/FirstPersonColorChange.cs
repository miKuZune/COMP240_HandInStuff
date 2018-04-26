using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonColorChange : Photon.PunBehaviour
{

    //If false then we are blue
    public bool teamColor = false;

	public Color color;


    // Use this for initialization
    void Start()
    {


        if (PhotonNetwork.player.GetTeam() == PunTeams.Team.red)
        {
            teamColor = true;

        }
        else
        {
            teamColor = false;
        }

        if (teamColor == true)
        {

			GetComponent<Renderer>().material.color = color;

        }
        else
        {

			GetComponent<Renderer>().material.color = color;

        }
    }
}