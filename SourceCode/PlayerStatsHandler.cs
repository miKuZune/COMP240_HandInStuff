using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsHandler : Photon.MonoBehaviour {

    GameObject scoreboard;
    public Text scoreboardTextPrefab;
    GameObject playersPanel;

    public int numOfEnemy = 0;
	public int emptyNumOfEn = 0;

    public bool roundEnded;
	int playerPing;

    LocalSettings localSettings;

    public List<GameObject> players = new List<GameObject>();

	// Use this for initialization
	void Start ()
    {
        if (photonView.isMine)
        {
            localSettings = GameObject.Find("LocalSettings").GetComponent<LocalSettings>();
            playersPanel = GameObject.Find("PlayersPanel");
            scoreboard = GameObject.Find("ScoreboardPanel");
            scoreboard.SetActive(false);

            players.AddRange(GameObject.FindGameObjectsWithTag("Player"));

          //  Reset all scores
            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
                player.SetScore(0);
            }
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (photonView.isMine)
        {
            if (Input.GetKeyDown(KeyCode.Tab) && !roundEnded)
            {
                // show scoreboard
                DestroyOld();
                scoreboard.SetActive(true);
                CalculatePositions();
            }
            else if(Input.GetKeyUp(KeyCode.Tab) && !roundEnded)
            {
                DestroyOld();
                NewRound();
            }

        }
	}

    public void RoundEnded()
    {

        if (roundEnded)
        {
            scoreboard.SetActive(true);
            DestroyOld();
            CalculatePositions();
        }
    }

    public void NewRound()
    {
      //  DestroyOld();
        scoreboard.SetActive(false);
    }

    void CalculatePositions()
    {

       // if (localSettings.addBots == true)
      //  {

       // int i, j;
      //  GameObject key;
		//
       // for (i = 1; i < players.Count; i++)
       // {

                //numOfEnemy ++;
                //int a, j;
			//	GameObject playerObj = players[i];

			//	PlayerProfile playerProfile = playerObj.GetComponent<PlayerProfile>();
			//	BOT_Profile botProfile = playerObj.GetComponent<BOT_Profile>();
                /* 
					if (playerProfile != null)
					{

                         while (j >= 0 && players[j].GetComponent<PlayerProfile>().Kills > playerProfile.Kills)
                          {
                                players[j + 1] = players[j];
                                 j = j - 1;
                             }

                                    players[j + 1] = key;

                      //  return;
					}
                    else
                    {
                        //j = i - 1;
                        //key = players[i];

                         while (j >= 0 && players[j].GetComponent<BOT_Profile>().Kills > botProfile.Kills)
                          {
                                players[j + 1] = players[j];
                                 j = j - 1;
                             }
                                    players[j + 1] = key;
                    }
                    */

               /*     List<GameObject> players1 = new List<GameObject>();

                    foreach (GameObject player in players)
                    {

                        players1.Add(player);

                    }

                    players1.Reverse();

                 //   foreach (GameObject playerF in players1)
                  //  {

                  //  PlayerProfile playerProfile1 = playerF.GetComponent<PlayerProfile>();
				    //BOT_Profile botProfile1 = playerF.GetComponent<BOT_Profile>();


                    if (playerProfile != null)
					{
                            Debug.Log("TESTING THIS SHIT 1");
                         Text playerLabel = Instantiate(scoreboardTextPrefab, playersPanel.transform) as Text;
                         playerLabel.text = playerProfile.userName;

                           //  if (player.GetTeam() == PunTeams.Team.red)
                           //  {
			    	        playerLabel.text = "<color=#ff0000ff>" + playerProfile.userName + "</color>" ;
                            //}
                          //  else if (player.GetTeam() == PunTeams.Team.blue)
                         //    {
				          //   playerLabel.text = "<color=#00ffffff>" + player.NickName + "</color>" + " (" + player.CustomProperties["Ping"] + "ms)";
                          //   }

                                 Text playerKillsLabel = Instantiate(scoreboardTextPrefab, playersPanel.transform) as Text;
                                 playerKillsLabel.text = "Kills:" + playerProfile.Kills;

                             //playerLabel.fontStyle = FontStyle.BoldAndItalic;
                            playerKillsLabel.fontStyle = FontStyle.BoldAndItalic;

                          //   return;
                    }

                    if (botProfile != null)
					{
                          Debug.Log("TESTING THIS SHIT 2");
                         Text playerLabel = Instantiate(scoreboardTextPrefab, playersPanel.transform) as Text;
                         playerLabel.text = botProfile.botName;

                         
                           //  if (player.GetTeam() == PunTeams.Team.red)
                           //  {
			    	        playerLabel.text = "<color=#ff0000ff>" + botProfile.botName + "</color>" ;
                            //}
                          //  else if (player.GetTeam() == PunTeams.Team.blue)
                         //    {
				          //   playerLabel.text = "<color=#00ffffff>" + player.NickName + "</color>" + " (" + player.CustomProperties["Ping"] + "ms)";
                          //   }

                                 Text playerKillsLabel = Instantiate(scoreboardTextPrefab, playersPanel.transform) as Text;
                                 playerKillsLabel.text = "Kills:" + botProfile.Kills;

                             //playerLabel.fontStyle = FontStyle.BoldAndItalic;
                            playerKillsLabel.fontStyle = FontStyle.BoldAndItalic;
                       // return;
					}

        }
    }
     else
        {
	*/
             int i, j;
        PhotonPlayer key;

        for (i = 1; i < PhotonNetwork.playerList.Length; i++)
        {

            key = PhotonNetwork.playerList[i];
            j = i - 1;

            while (j >= 0 && PhotonNetwork.playerList[j].GetScore() > key.GetScore())
            {
                PhotonNetwork.playerList[j + 1] = PhotonNetwork.playerList[j];
                j = j - 1;
            }
            PhotonNetwork.playerList[j + 1] = key;
        }

        List<PhotonPlayer> players = new List<PhotonPlayer>();

        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            players.Add(player);

        }

        players.Reverse();

        foreach (PhotonPlayer player in players)
        {
            Text playerLabel = Instantiate(scoreboardTextPrefab, playersPanel.transform) as Text;
            playerLabel.text = player.NickName;
            if (player.GetTeam() == PunTeams.Team.red)
            {
				playerLabel.text = "<color=#ff0000ff>" + player.NickName + "</color>" + " (" + player.CustomProperties["Ping"] + "ms)";
            }
            else if (player.GetTeam() == PunTeams.Team.blue)
            {
				playerLabel.text = "<color=#00ffffff>" + player.NickName + "</color>" + " (" + player.CustomProperties["Ping"] + "ms)";
            }

            Text playerKillsLabel = Instantiate(scoreboardTextPrefab, playersPanel.transform) as Text;
            playerKillsLabel.text = "Kills:" + player.GetScore();

            if (player.IsLocal)
            {
                playerLabel.fontStyle = FontStyle.BoldAndItalic;
                playerKillsLabel.fontStyle = FontStyle.BoldAndItalic;
            }
        }
        }


  void DestroyOld()
    {
       foreach (Transform child in playersPanel.transform)
       {
            Destroy(child.gameObject);
       }
   }

    [PunRPC]
   public void AddKill(PhotonPlayer player)
   {
        // show killfeed thingo


      //   update my copy of their score
       player.AddScore(1);
        
    }
}

    
