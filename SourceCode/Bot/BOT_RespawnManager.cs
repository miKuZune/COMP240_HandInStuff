using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BOT_RespawnManager : Photon.PunBehaviour
 {

	public enum BOT_FStateMachineRespawn
	{

		Stop, Death, RespawnTimer, Respawn

	}

	public BOT_FStateMachineRespawn currState;

	 public Transform mesh;

	 public float timer = 300f;
	public float curTimer;

	BOT_Manager bOT_Manager;
	BOT_Profile bOT_Profile;
	GameManager gameManager;
	// Use this for initialization
	void Start () 
	{

		currState = BOT_FStateMachineRespawn.Stop;
		bOT_Profile = GetComponent<BOT_Profile>();
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		mesh = transform.Find("Player_3PNew:Character_Unwrapped:Egor");
		bOT_Manager = GetComponent<BOT_Manager>();
		
	}

	void Update() 
	{

		switch (currState)
		{

			case BOT_FStateMachineRespawn.Stop:
			break;

			case BOT_FStateMachineRespawn.Death:
			Death();
			break;

			case BOT_FStateMachineRespawn.RespawnTimer:
			RespawnTimer();
			break;

			case BOT_FStateMachineRespawn.Respawn:
			Respawn();
			break;

		}
		
	}

	public void Death()
	{

		mesh.gameObject.SetActive(false);
		bOT_Manager.currentStateAttack = BOT_Manager.BOT_FStateMachineAttack.Stop;
		bOT_Manager.currentStateWander = BOT_Manager.BOT_FStateMachineWander.Stop;

		GetComponent<NavMeshAgent>().Warp(gameObject.transform.position = new Vector3(99999, 99999, 99999));

		curTimer = timer;
		currState = BOT_FStateMachineRespawn.RespawnTimer;
		bOT_Profile.isDead = true;

		
	}
	
	public void RespawnTimer()
	{

		curTimer-= Time.deltaTime;

		if (curTimer < 0)
		{

			currState = BOT_FStateMachineRespawn.Respawn;

		}

	}

	public void Respawn()
	{

		int id = Random.Range(0, gameManager.respawnPoints.Length);
		GetComponent<NavMeshAgent>().Warp(gameManager.respawnPoints[id].transform.position);
		bOT_Profile.isDead = false;
		currState = BOT_FStateMachineRespawn.Stop;

	}
}
