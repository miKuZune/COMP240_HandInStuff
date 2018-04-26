using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Rounds : Photon.MonoBehaviour {

	public int roundLengthInSecs = 150;
	public int numberOfRounds = 5;
	List<PunTeams.Team> roundWinners = new List<PunTeams.Team>();
	int currentRound = 1;
	float timeLeft;
	public static bool roundPaused;
	Text roundTimerText;
	Text roundCounter;

	public float syncEverySecs = 3f;
	float sync;

	public float roundFinishCooldown = 10f;
	float coundownFromEnd;

	PlayerStatsHandler playerStats;
	GameObject roundWinPanel;

	SendAnalyticInfo analytics;

	GameManager gameManager;

	PlayerShoot playerShoot;
	PlayerHealth playerHealth;

	public bool gameStarted;
	public float gameStartCountdown;
	public bool startCountdown;

	PlayerMovement movement;
	CameraLook cameraLook;

	GameObject playtestEndPanel;

	bool playTest = false;

	public Sprite victory;
	public Sprite loss;

	GameObject didYouWin;

	LocalSettings localSettings;

	bool playedRoundEndHintSfx = false;
	public bool fadedout = false;

	// Use this for initialization
	void Start ()
	{

		if (photonView.isMine)
		{
			//playtestEndPanel = GameObject.Find ("Playtest-end-panel");
			//playtestEndPanel.SetActive (false);
			//string key = "Ascension_" + PhotonNetwork.gameVersion + "_isPlaytest";
			//if (PlayerPrefs.HasKey (key)) {
			//	if (PlayerPrefs.GetInt (key) == 1) {
			//		// it IS a playtest
			//		playTest = true;
			//	}
			//}

			localSettings = GameObject.Find ("LocalSettings").GetComponent<LocalSettings>();
			roundWinPanel = GameObject.Find("RoundWinTeamPanel");
			roundWinPanel.SetActive(false);

			movement = GetComponent<PlayerMovement> ();
			cameraLook = GetComponent<CameraLook> ();

			roundTimerText = GameObject.Find("RoundTimerText").GetComponent<Text>();
			timeLeft = roundLengthInSecs;
			playerStats = GetComponent<PlayerStatsHandler>();
			coundownFromEnd = roundFinishCooldown;

			analytics = GameObject.FindGameObjectWithTag("Analytics").GetComponent<SendAnalyticInfo>();
			analytics.inMatch = true;

			gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
			playerShoot = GetComponent<PlayerShoot>();


			// 1st round
			roundCounter = GameObject.Find("RoundCounterText").GetComponent<Text>();

			roundTimerText.text = Mathf.CeilToInt(timeLeft).ToString();

			playerShoot.enabled = false;
			movement.enabled = false;
			cameraLook.enabled = false;
			startCountdown = true;

			didYouWin = GameObject.Find ("DidYouWin");
			didYouWin.SetActive (false);


			PunTeams.Team team = PhotonNetwork.player.GetTeam ();
			GameObject teamTextThing = GameObject.Find ("TeamDefineText");

			string teamS = "";
			if (team == PunTeams.Team.blue) {teamS = "blue"; teamTextThing.GetComponent<Text> ().color = Color.blue;} 
			else if (team == PunTeams.Team.red) {teamS = "red"; teamTextThing.GetComponent<Text> ().color = Color.red;}


			teamTextThing.GetComponent<Text> ().text = "You are on " + teamS + " team.";
		}
		if (PhotonNetwork.isMasterClient)
		{
			roundTimerText = GameObject.Find("RoundTimerText").GetComponent<Text>();
			sync = syncEverySecs;
			timeLeft = roundLengthInSecs;
			roundTimerText.text = Mathf.CeilToInt(timeLeft).ToString();
			coundownFromEnd = roundFinishCooldown;
		}

		playerHealth = GetComponent<PlayerHealth>();
	}

	[PunRPC]
	public void AllPlayersConnected()
	{
		if(photonView.isMine)
		{
			startCountdown = true;
			//start music

			InRoundMusic.audioSource.Play ();
		}
	}

	// Update is called once per frame
	void Update ()
	{

		if (localSettings.botsCanStart == false) 
		{

			if (gameStarted == true) 
			{
				
				localSettings.botsCanStart = true;

			}
			
		}

		if (!gameStarted && photonView.isMine) 
		{






			playerShoot.enabled = false;
			movement.enabled = false;
			cameraLook.enabled = false;

			if (startCountdown) {
				gameStartCountdown -= Time.deltaTime;
			}

			if (gameStartCountdown <= 0f) {
				playerShoot.enabled = true;
				movement.enabled = true;
				cameraLook.enabled = true;
				gameStarted = true;
			}

			return;
		}



		if (PhotonNetwork.isMasterClient)
		{
			if (!roundPaused)
			{
				
				if (timeLeft > 0f)
				{
					timeLeft -= Time.deltaTime;
					roundTimerText.text = Mathf.CeilToInt(timeLeft).ToString();
				}

				// Every sync seconds sync player timers to host
				if(sync <= 0)
				{
					if (timeLeft >= 0.05f)
					{
						//if (timeLeft > 1f)
						//{
						GetComponent<PhotonView>().RPC("SetTime", PhotonTargets.Others, timeLeft-0.012f);
						//}else
						//{
						//  GetComponent<PhotonView>().RPC("SetTime", PhotonTargets.Others, timeLeft);

						//}

					}
					sync = syncEverySecs;
				}
				if (sync > 0)
				{
					sync -= Time.deltaTime;
				}

				//if (timeLeft <= 0f)
				//{
				//    timeLeft = 0f;
				//    GetComponent<PhotonView>().RPC("SetTime", PhotonTargets.Others, timeLeft);
				//roundPaused = true;
				//}
			}



		}
		else if (photonView.isMine && !PhotonNetwork.isMasterClient)
		{
			if (!roundPaused)
			{

				if (timeLeft > 0f)
				{
					
					timeLeft -= Time.deltaTime;
					roundTimerText.text = Mathf.CeilToInt(timeLeft).ToString();
				}
			}
		}
		if (photonView.isMine)
		{
			if (!roundPaused)
			{
				if (timeLeft > 11) {
					FindObjectOfType<InRoundMusic> ().NewRound ();
				}

				if (timeLeft <= 146f && !gameStarted) 
				{
					playerShoot.enabled = true;
					movement.enabled = true;
					cameraLook.enabled = true;
					gameStarted = true;

				}

				if (timeLeft <= 15.9f && !fadedout) {
					// fade out old music
					FindObjectOfType<InRoundMusic>().EndOfRound();
					fadedout = true;
				}

				if (timeLeft <= 0f)
				{
					// show scoreboard n shit
					playerStats.roundEnded = true;
					playerStats.RoundEnded();

					roundWinPanel.SetActive(true);


					if (CalculateRoundWinners() == PunTeams.Team.none)
					{
						roundWinPanel.GetComponentInChildren<Text>().text = "Draw!";
						roundWinners.Add(PunTeams.Team.none);
						FindObjectOfType<InRoundMusic> ().PlayLoss ();
					}
					else if (CalculateRoundWinners() == PunTeams.Team.red)
					{
						roundWinPanel.GetComponentInChildren<Text>().text = "Red Team win the round!";
						roundWinners.Add(PunTeams.Team.red);
						if (PhotonNetwork.player.GetTeam () == PunTeams.Team.red) {
							FindObjectOfType<InRoundMusic> ().PlayWin ();
						} else {
							FindObjectOfType<InRoundMusic> ().PlayLoss ();
						}
					}
					else if (CalculateRoundWinners() == PunTeams.Team.blue)
					{
						roundWinPanel.GetComponentInChildren<Text>().text = "Blue Team win the round!";
						roundWinners.Add(PunTeams.Team.blue);
						if (PhotonNetwork.player.GetTeam () == PunTeams.Team.blue) {
							FindObjectOfType<InRoundMusic> ().PlayWin ();
						} else {
							FindObjectOfType<InRoundMusic> ().PlayLoss ();
						}
					}
					//When the game is over
					if (currentRound >= 5) 
					{
						//Count the wins for each team
						int blueTeamWins = 0, redTeamWins = 0;
						for (int i = 0; i < roundWinners.Count; i++) 
						{
							if (roundWinners [i] == PunTeams.Team.blue) {
								blueTeamWins++;
							} else if (roundWinners [i] == PunTeams.Team.red) {redTeamWins++;}
						}

						//Store the winning team
						PunTeams.Team winningTeam = PunTeams.Team.none;
						if (blueTeamWins > redTeamWins) 
						{
							winningTeam = PunTeams.Team.blue;
						} else if (redTeamWins > blueTeamWins) 
						{
							winningTeam = PunTeams.Team.red;
						}

						//Check if the player is on the winning team
						PunTeams.Team playerTeam = PhotonNetwork.player.GetTeam ();

						didYouWin.SetActive (true);
						if (playerTeam == winningTeam) 
						{
							//Display that you have won
							didYouWin.GetComponent<Image> ().sprite = victory;

						} else 
						{
							//Display that you have lost
							didYouWin.GetComponent<Image> ().sprite = loss;

						}
					}

					currentRound = currentRound + 1;
					roundPaused = true;
				}

			}

			if(roundPaused && timeLeft <= 0f)
			{
				if (coundownFromEnd > 0f)
				{
					coundownFromEnd -= Time.deltaTime;
					roundTimerText.text = Mathf.CeilToInt(coundownFromEnd).ToString();

					if (currentRound > numberOfRounds) {
						playtestEndPanel.SetActive (true);
					}

				}
				else
				{
					// if that was last round
					if(currentRound > numberOfRounds)
					{
						analytics.roundsPlayed++;
						analytics.inMatch = false;

						//
						GetComponent<KillStreaks> ().ResetAllStreaks ();

						if (playTest) {
							Application.Quit ();
						} else if (!playTest) {
							roundPaused = false;
							gameManager.LeaveRoom ();
						}

					}else
					{
						// Reset everything tbh
						if(PhotonNetwork.player.GetTeam() == PunTeams.Team.red)
						{
							transform.position = gameManager.spawnPointRed.transform.position;
						}
						else if(PhotonNetwork.player.GetTeam() == PunTeams.Team.blue)
						{
							transform.position = gameManager.spawnPointBlue.transform.position;
						}

						#region maxStuff
						//Set up god
						PhotonPlayer[] gods = TeamGodPick();
						if(PhotonNetwork.player == gods[0] )
						{
							//Red god
							GetComponentInChildren<GodSetup>().isGod = true;
							GetComponentInChildren<GodSetup>().InitialSetup();
						}else if(PhotonNetwork.player == gods[1])
						{
							//Blue god
							GetComponentInChildren<GodSetup>().isGod = true;
							GetComponentInChildren<GodSetup>().InitialSetup();
						}
						else
						{
							GetComponentInChildren<GodSetup>().isGod = false;
							GetComponentInChildren<GodSetup>().UnSetup();
						}

						GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
						foreach(GameObject player in players)
						{
							player.GetComponent<PlayerMovement>().ResetSpeed();
						}


						#endregion

						analytics.roundsPlayed++;

						playerShoot.ammo = playerShoot.magSize;
						playerShoot.DebugReload();
						playerHealth.AddHealth(100);

						movement.isStunned = false;

						timeLeft = roundLengthInSecs;
						coundownFromEnd = roundFinishCooldown;
						playerStats.roundEnded = false;
						roundWinPanel.SetActive(false);
						playerStats.NewRound();

						roundCounter.text = "Round " + currentRound;
						roundPaused = false;
					}
				}

			}
		}
		if (!photonView.isMine && roundPaused && timeLeft <= 0f && currentRound < numberOfRounds)
		{
			playerHealth.AddHealth(100);
		}
	}

	[PunRPC]
	public void SetTime(float time)
	{
		if (photonView.isMine && !roundPaused)
		{
			if (time >= 0f)
			{
				timeLeft = time;
				roundTimerText.text = Mathf.CeilToInt(timeLeft).ToString();
				//Debug.Log("Received RPC for time: " + time);

			}else
			{
				//Debug.Log("Received RPC for invalid time! " + time + " ignoring...");
			}

		}
	}


	#region max'sStuff
	PhotonPlayer[] TeamGodPick()
	{
		PhotonPlayer[] redPlayers = new PhotonPlayer[5];
		PhotonPlayer[] bluePlayers = new PhotonPlayer[5];
		PhotonPlayer[] godPlayers = new PhotonPlayer[2];
		int redPlayerCount = 0;
		int bluePlayerCount = 0;

		//Sets up seperate arrays for blue players and red players.
		foreach(PhotonPlayer player in PhotonNetwork.playerList)
		{
			if(player.GetTeam() == PunTeams.Team.blue)
			{
				bluePlayers[bluePlayerCount] = player; 
				bluePlayerCount++;
			}else if(player.GetTeam() == PunTeams.Team.red)
			{
				redPlayers[redPlayerCount] = player;
				redPlayerCount++;
			}
		}

		int bestPlayerIndex = 0;
		//Finds the best red player and makes them god.
		for (int i = 0; i < redPlayers.Length; i++)
		{
			if (redPlayers[i] != null)
			{
				int currPlayerScore = redPlayers[i].GetScore();
				int bestPlayerScore = redPlayers[bestPlayerIndex].GetScore();
				if (currPlayerScore >= bestPlayerScore)
				{
					bestPlayerIndex = i;
				}
			}
		}
		godPlayers[0] = redPlayers[bestPlayerIndex];
		bestPlayerIndex = 0;
		//Finds the best blue player and makes them god.
		for (int i = 0; i < bluePlayers.Length; i++)
		{
			if (bluePlayers[i] != null)
			{
				int currPlayerScore = bluePlayers[i].GetScore();
				int bestPlayerScore = bluePlayers[bestPlayerIndex].GetScore();
				if (currPlayerScore > bestPlayerScore)
				{
					bestPlayerIndex = i;
				}
			}
		}
		godPlayers[1] = bluePlayers[bestPlayerIndex];

		return godPlayers;
	}

	#endregion

	PunTeams.Team CalculateRoundWinners()
	{
		int blueKills = 0;
		int redKills = 0;

		foreach(PhotonPlayer player in PhotonNetwork.playerList)
		{
			if(player.GetTeam() == PunTeams.Team.red)
			{
				redKills += player.GetScore();
			}else if(player.GetTeam() == PunTeams.Team.blue)
			{
				blueKills += player.GetScore();
			}
		}

		if(redKills > blueKills)
		{
			return PunTeams.Team.red;
		}else if(blueKills > redKills)
		{
			return PunTeams.Team.blue;
		}else
		{
			return PunTeams.Team.none;
		}


	}
}
