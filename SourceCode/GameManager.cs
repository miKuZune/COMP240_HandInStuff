using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine.UI;

public class GameManager : Photon.PunBehaviour
{
    //Variables
	public GameObject playerPrefab;
	public GameObject botPrefab;

	public int numberOfTickets = 4;

	public GameObject spawnPointRed;
	public GameObject spawnPointBlue;

	public Transform[] respawnPoints = new Transform[6];

	Vector3 spawn;

	SendAnalyticInfo analytics;

	public List<PhotonPlayer> teamRed = new List<PhotonPlayer>();
	public List<PhotonPlayer> teamBlue = new List<PhotonPlayer>();

	public int teamRedCount;
	public int teamBlueCount;

	LocalSettings localSettings;

	private void Start()
	{
        //Get references.
		localSettings = GameObject.Find("LocalSettings").GetComponent<LocalSettings>();

		foreach(PhotonPlayer player in PhotonNetwork.playerList)
		{

			if (player.GetTeam() == PunTeams.Team.red)
			{

				teamRedCount ++;
				teamRed.Add(player);
				Debug.Log(teamRedCount);
			}
			else
			{
				teamBlueCount ++;
				teamBlue.Add(player);
				Debug.Log(teamBlueCount);
			}

		}

		if (PlayerMovement.localPlayerInstance == null)
		{
			if (PhotonNetwork.player.GetTeam() == PunTeams.Team.red) 
			{
				spawn = spawnPointRed.transform.position;
			} else {
				spawn = spawnPointBlue.transform.position;
			}


			PhotonNetwork.Instantiate(playerPrefab.name, spawn, Quaternion.identity, 0);

			analytics = GameObject.FindGameObjectWithTag("Analytics").GetComponent<SendAnalyticInfo>();
		}
        //Handle adding bots into the game.
		if (PhotonNetwork.isMasterClient && localSettings.addBots == true)
		{

			for(int i = teamRedCount; i < 5; i++)
			{
				Debug.Log("adding red bot");
				// spawn bot
				GameObject bot = PhotonNetwork.Instantiate(botPrefab.name, new Vector3(0, 1000,0), Quaternion.identity, 0) as GameObject;

				Text botNameText = (Text)bot.GetComponentInChildren(typeof(Text));
				BOT_Profile bOT_Profile = (BOT_Profile)bot.GetComponent(typeof(BOT_Profile));
				botNameText.color = Color.red;
				bOT_Profile.GetAndSetBotName();
				botNameText.text = "[BOT]" + bOT_Profile.botName;

				bot.GetComponent<BOT_Profile>().red = true;

				if (bot.GetComponent<BOT_Profile>().red && !bot.GetComponent<NavMeshAgent>().isOnNavMesh) 
				{
					bot.GetComponent<NavMeshAgent>().Warp(spawnPointRed.transform.position);
				} else {
					bot.GetComponent<NavMeshAgent>().Warp(spawnPointBlue.transform.position);
				}



				teamRedCount++;
			}


			for(int i = teamBlueCount; i < 5; i++)
			{
				Debug.Log("adding blue bot");
				// spawn bot
				GameObject bot = PhotonNetwork.Instantiate(botPrefab.name, new Vector3(0, 1000,0), Quaternion.identity, 0) as GameObject;

				//

				Text botNameText = (Text)bot.GetComponentInChildren(typeof(Text));
				BOT_Profile bOT_Profile = (BOT_Profile)bot.GetComponent(typeof(BOT_Profile));
				botNameText.color = Color.blue;
				bOT_Profile.GetAndSetBotName();
				botNameText.text = "[BOT]" + bOT_Profile.botName;

                bot.GetComponent<BOT_Profile>().blue = true;

				if (bot.GetComponent<BOT_Profile>().red && !bot.GetComponent<NavMeshAgent>().isOnNavMesh) 
				{
					bot.GetComponent<NavMeshAgent>().Warp(spawnPointRed.transform.position);
				} else {
					bot.GetComponent<NavMeshAgent>().Warp(spawnPointBlue.transform.position);
				}
				teamBlueCount++;
			}

		}
	}

	public override void OnLeftRoom()
	{
		SceneManager.LoadScene(0);
	}

	public void LeaveRoom()
	{
		analytics.inMatch = false;
		PhotonNetwork.LeaveRoom();
	}

	void LoadArena()
	{
		if (!PhotonNetwork.isMasterClient)
		{
			Debug.LogError("Not master client!");
		}
	}

	#region callbacks
	public override void OnPhotonPlayerConnected(PhotonPlayer other)
	{
		if (PhotonNetwork.isMasterClient) {
			LoadArena ();
		}
	}

	public override void OnPhotonPlayerDisconnected(PhotonPlayer other)
	{
		if (PhotonNetwork.isMasterClient)
		{
			LoadArena();
		}
	}
	#endregion

	void PrepareBots()
	{

	}
}
