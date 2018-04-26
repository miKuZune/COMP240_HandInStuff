using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Respawn : Photon.MonoBehaviour {
    //Variables
	PlayerShoot shoot;
	GameManager gameManager;
	PlayerMovement movement;

	public string team;

	public float respawnTime = 6f;
	float storedRespawnTime;
	public bool dead;

	SkinnedMeshRenderer body;
	public GameObject shotgun;

    public CharacterController collider;
    public Canvas nameTag;

	Text respawnTxt;

	PlayerHealth health;

    CameraLook cameraLook;

    public GameObject playerMesh;

	void Start(){
        //Start the player off at their teams spawn point.
		if (photonView.isMine)
		{
			if (photonView.owner.GetTeam () == PunTeams.Team.blue) {
				team = "blue";
			}
			else if(photonView.owner.GetTeam() == PunTeams.Team.red)
			{
				team = "red";
			}

            playerMesh = GameObject.Find("WaterPistol_Idle");
		}
        //Get references to objects in the scene.
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();

		if (photonView.isMine) {
			
			respawnTxt = GameObject.Find ("RespawnText").GetComponent<Text>();
			respawnTxt.text = "";

            
		}

        cameraLook = GetComponent<CameraLook>();
        shoot = GetComponent<PlayerShoot> ();
		storedRespawnTime = respawnTime;
		movement = GetComponent<PlayerMovement> ();
		health = GetComponent<PlayerHealth> ();
		body = GetComponentInChildren<SkinnedMeshRenderer> ();
        collider = GetComponent<CharacterController>();
        nameTag = GetComponentInChildren<Canvas>();
	}

	public void OnDeath()
    {
        //Set health back to full.

		if (!photonView.isMine) {
			body.enabled = false;
			shotgun.SetActive (false);
            collider.enabled = false;
            nameTag.enabled = false;
		}


        if (photonView.isMine)
        {
            respawnTxt.text = "Respawning in: 6 seconds.";

            // reset stuns
            movement.isStunned = false;
            playerMesh.SetActive(false);
        }

        // Disable everything
        // What to disable:
        // movement
        movement.enabled = false;
		// shooting
		shoot.enabled = false;
		// model
		health.enabled = false;
		// do countdown
		dead = true;

    }

	GameObject[] GetPlayers()
	{
		GameObject[] allPlayers;
		allPlayers = GameObject.FindGameObjectsWithTag ("Player");

		//Collect all the non god players 
		GameObject[] noGodPlayers;
        noGodPlayers = new GameObject[allPlayers.Length];
        int notGodIndex = 0;
		for (int i = 0; i < allPlayers.Length; i++)
		{
			if (!allPlayers [i].GetComponent<GodSetup> ().isGod) 
			{
				noGodPlayers [notGodIndex] = allPlayers [i];
				notGodIndex++;
			}
		}

		//Sort into players who are not on the players team

		GameObject[] notMyTeamPlayers;
		int otherTeamCount = 0;
		for (int i = 0; i < noGodPlayers.Length; i++) 
		{
			if (noGodPlayers [i].GetComponent<Respawn> ().team != team)
			{
				otherTeamCount++;
			}
		}
		notMyTeamPlayers = new GameObject[otherTeamCount];
		int otherTeamIndex = 0;
		for (int i = 0; i < noGodPlayers.Length; i++) 
		{
			if (noGodPlayers [i].GetComponent<Respawn> ().team != team)
			{
				notMyTeamPlayers [otherTeamIndex] = noGodPlayers [i];
				otherTeamIndex++;
			}
		}
			
		return notMyTeamPlayers;
	}
    //Chooses a respawn point based on which point is furthest away from all enemies.
	int chooseRespawnPoint()
	{
		int respawnPointID = 0;

		GameObject[] players = GetPlayers (); 


		float currHighestDist = 0;
		for (int i = 0; i < gameManager.respawnPoints.Length; i++) 
		{
			float totalDistForAll = 0;
			for (int j = 0; j < players.Length; j++) 
			{
				totalDistForAll += Vector3.Distance (gameManager.respawnPoints [i].position, players [j].transform.position);
			}

			if (totalDistForAll > currHighestDist) {
				currHighestDist = totalDistForAll;
				respawnPointID = i;
			}
		}

		return respawnPointID;
	}
		

	public void Respawned()
	{
        // re-enable all

        Vector3 respawnPos = new Vector3(0,0,0);

		if (photonView.isMine) {

			respawnTxt.text = "";
			shoot.reloadWarningPanel.SetActive (false);
			int respawnPointID = chooseRespawnPoint();
			respawnPos = gameManager.respawnPoints[respawnPointID].position;
			shoot.ammoTxt.text = shoot.ammo.ToString();

            // reset stuns
            movement.isStunned = false;

            playerMesh.SetActive(true);

        }
        else if(!photonView.isMine)
		{
			Debug.Log ("That happened");
			body.enabled = true;
			shotgun.SetActive (true);
            collider.enabled = true;
            nameTag.enabled = true;
		}


		// movement
		movement.enabled = true;
		// shooting
		shoot.enabled = true;
		health.enabled = true;

        if(photonView.isMine)
        {
            //Does this later so the player isn't seen then disappears
            transform.position = respawnPos;
        }

    }

	void Update()
    {
		if (dead)
        {
			respawnTime -= Time.deltaTime;

			if (photonView.isMine) {
				respawnTxt.text = "Respawning in: " + Mathf.RoundToInt(respawnTime) + " seconds.";
			}

			if (respawnTime <= 0f) {
				respawnTime = storedRespawnTime;
				Respawned ();
				dead = false;
			}
		}
	}

}