using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfile : Photon.PunBehaviour
{

	public bool red;
	public bool blue;

	public int Kills = 0;

	public string userName;

	PhotonPlayer player;

	void Awake()
	{
		
	//	userName = player.NickName;

	}

	// Use this for initialization
	void Start () 
	{

		userName = transform.name;
		//userName = player.NickName;
		if(gameObject.CompareTag("projectile"))
		{
			return;
		}
		//I DONT REALLY NEED THIS SCRIPT >< 
		if (GetComponent<PhotonView>().owner.GetTeam() == PunTeams.Team.blue)
		{

			red = false;
			blue = true;

		}

		if (GetComponent<PhotonView>().owner.GetTeam() == PunTeams.Team.red)
		{

			red = true;
			blue = false;

		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
