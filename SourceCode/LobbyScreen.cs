using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyScreen : Photon.PunBehaviour
{
    //Variabels
    public GameObject team1playerListPanel;
    public GameObject team2playerListPanel;

    public GameObject playerLabel;

    public GameObject startMatchButton;

    public GameObject chooseMapDropDown;

    public Text team1playerListText;
    public Text team2playerListText;

    public Slider playerLimitSlider;
    public Text playerLimitText;
    public Toggle hiddenRoomToggle;
    public Toggle botToggle;

    public Text lobbyNameTxt;
    public Text warningTeamsTxt;

    public Text teamBalanceTxt;

	public Text MapDesc;
	public Sprite MapThumbNail;

	public List<MapClass> mapClass = new List<MapClass> ();

    public List<PhotonPlayer> team1 = new List<PhotonPlayer>();
    public List<PhotonPlayer> team2 = new List<PhotonPlayer>();

	Launcher launcher;
    LocalSettings localSettings;

    void Start()
    {
        //Get references
		launcher = GameObject.Find ("Launcher").GetComponent<Launcher>();
        lobbyNameTxt.text = "Lobby: " + PhotonNetwork.room.Name;
        hiddenRoomToggle.gameObject.SetActive(false);
        startMatchButton.SetActive(false);
        chooseMapDropDown.SetActive(false);
        localSettings = GameObject.Find("LocalSettings").GetComponent<LocalSettings>();

        teamBalanceTxt.text = "";

        if (!PhotonNetwork.isMasterClient)
        {
            playerLimitSlider.interactable = false;
			botToggle.gameObject.SetActive(false);
			playerLimitSlider.gameObject.SetActive(false);
			playerLimitText.gameObject.SetActive(false);

        }else
        {
			if (!launcher.playTest) {
				startMatchButton.SetActive (true);
			}

			MapDesc.gameObject.SetActive(false);
			GameObject.Find ("MapThumbNail").SetActive (false);
        }

        playerLimitSlider.value = PhotonNetwork.room.MaxPlayers;
        playerLimitText.text = "Player Limit " + "(" + PhotonNetwork.room.MaxPlayers + ")";
    }

    [PunRPC]
    public void UpdatePlayerList()
    {
        if (PhotonNetwork.inRoom)
        {

			lobbyNameTxt.text = "Lobby: " + PhotonNetwork.room.Name;

            foreach (Transform child in team1playerListPanel.transform)
            {
                if (child.gameObject.name != "PlayerListText")
                {
                    Destroy(child.gameObject);
                }
            }

            foreach (Transform child in team2playerListPanel.transform)
            {
                if (child.gameObject.name != "PlayerListText")
                {
                    Destroy(child.gameObject);
                }
            }

            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
                if (player.GetTeam() == PunTeams.Team.red)
                {
                    GameObject label = Instantiate(playerLabel, team1playerListPanel.transform) as GameObject;
                    label.GetComponent<Text>().text = player.NickName;
                    label.GetComponent<Text>().color = Color.white;
                    label.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

                    if (player.IsMasterClient)
                    {
                        label.GetComponent<Text>().text = "<color=#00ffffff>(Host)</color> " + player.NickName;
                    }
                    if (player.IsLocal)
                    {
                        label.GetComponent<Text>().fontStyle = FontStyle.BoldAndItalic;
                    }
                }
                else if (player.GetTeam() == PunTeams.Team.blue)
                {
                    GameObject label = Instantiate(playerLabel, team2playerListPanel.transform) as GameObject;
                    label.GetComponent<Text>().text = player.NickName;
                    label.GetComponent<Text>().color = Color.white;
                    label.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

                    if (player.IsMasterClient)
                    {
                        label.GetComponent<Text>().text = "<color=#00ffffff>(Host)</color> " + player.NickName;
                        //label.GetComponent<Text>().color = Color.cyan;
                    }
                    if (player.IsLocal)
                    {
                        label.GetComponent<Text>().fontStyle = FontStyle.BoldAndItalic;
                    }
                }
            }

            if (PhotonNetwork.isMasterClient)
            {
				launcher = GameObject.Find ("Launcher").GetComponent<Launcher>();
				if (!launcher.playTest) {
					startMatchButton.SetActive (true);
                    chooseMapDropDown.SetActive(true);
					playerLimitSlider.interactable = false;
					hiddenRoomToggle.interactable = true;
				}
            }

            int team1PlayerCount = 0;
            int team2PlayerCount = 0;
            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
                if (player.GetTeam() == PunTeams.Team.red)
                {
                    team1PlayerCount++;
                }
                else if (player.GetTeam() == PunTeams.Team.blue)
                {
                    team2PlayerCount++;
                }
            }

            team1playerListText.text = "Players: (" + team1PlayerCount + "/" + PhotonNetwork.room.MaxPlayers / 2 + ")";
            team2playerListText.text = "Players: (" + team2PlayerCount + "/" + PhotonNetwork.room.MaxPlayers / 2 + ")";
            playerLimitSlider.value = PhotonNetwork.room.MaxPlayers;
            playerLimitText.text = "Player Limit " + "(" + PhotonNetwork.room.MaxPlayers + ")";


            bool showWarningTeamsTxt = false;
            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
                if (player.GetTeam() != PunTeams.Team.red && player.GetTeam() != PunTeams.Team.blue)
                {
                    showWarningTeamsTxt = true;
                }
            }
            if (showWarningTeamsTxt)
            {
                warningTeamsTxt.enabled = true;
            }
            else
            {
                warningTeamsTxt.enabled = false;

				if (launcher.playTest) {
					int red = 0;
					int blue = 0;
					foreach (PhotonPlayer player in PhotonNetwork.playerList)
					{
						if (player.GetTeam () == PunTeams.Team.red) {
							red++;
						} else if (player.GetTeam () == PunTeams.Team.blue) {
							blue++;
						}
					}
					if (red == 5 && blue == 5) {
						//start game
						launcher.StartMatch();

					}
				}
            }


            // check balance
            if(team1.Count > team2.Count || team2.Count > team1.Count)
            {
                teamBalanceTxt.text = "";
            }else
            {
                teamBalanceTxt.text = "";
            }

        }
    }

    //Display to other players which map is going to be played.
    [PunRPC]
	public void UpdateMap(string newText, int value)
    {
		MapDesc.text = mapClass[value].desc;

		if (!PhotonNetwork.isMasterClient) {
			GameObject.Find ("MapThumbNail").GetComponent<Image> ().sprite = mapClass [value].image;
		}
        GameObject.Find("Launcher").GetComponent<Launcher>().levelToLoad = newText;

    }
    
    public void JoinTeam(int team)
    {
        if (team == 1)
        {
            if (team1.Count < PhotonNetwork.room.MaxPlayers / 2)
            {

                bool addPlayer = true;
                if(team1.Contains(PhotonNetwork.player))
                {
                    // do nothing
                    addPlayer = false;
                }
                
                if (addPlayer)
                {
                    team1.Add(PhotonNetwork.player);// local
                    GetComponent<PhotonView>().RPC("AddPlayerToTeam", PhotonTargets.Others, 1, PhotonNetwork.player);
                    PhotonNetwork.player.SetTeam(PunTeams.Team.red);

                    if (team2.Contains(PhotonNetwork.player))
                    {
                        team2.Remove(PhotonNetwork.player);
                    }
                }
            }
        }
        else
        {
            if (team2.Count < PhotonNetwork.room.MaxPlayers / 2)
            {
                bool addPlayer = true;
                if(team2.Contains(PhotonNetwork.player))
                {
                    // do nothing
                    addPlayer = false;
                }

                if (addPlayer)
                {
                    team2.Add(PhotonNetwork.player);
                    GetComponent<PhotonView>().RPC("AddPlayerToTeam", PhotonTargets.Others, 2, PhotonNetwork.player);
                    PhotonNetwork.player.SetTeam(PunTeams.Team.blue);

                    if (team1.Contains(PhotonNetwork.player))
                    {
                        team1.Remove(PhotonNetwork.player);
                    }
                }
            }
        }

        UpdatePlayerList();
        GetComponent<PhotonView>().RPC("UpdatePlayerList", PhotonTargets.Others);
        
    }

    [PunRPC]
    public void AddPlayerToTeam(int team, PhotonPlayer player)
    {
        if(team == 1)
        {
            if (!team1.Contains(player) && team1.Count < PhotonNetwork.room.MaxPlayers/2)
            {
                team1.Add(player);
            }
            if (team2.Contains(player))
            {
                team2.Remove(player);
            }
        }else if(team == 2)
        {
            if (!team2.Contains(player) && team2.Count < PhotonNetwork.room.MaxPlayers/2)
            {
                team2.Add(player);
            }
            if (team1.Contains(player))
            {
                team1.Remove(player);
            }
        }
        UpdatePlayerList();
    }

    [PunRPC]
    public void RemoveDisconnectedPlayerFromTeam(PhotonPlayer player)
    {
        if (team1.Contains(player))
        {
            team1.Remove(player);
        }else if (team2.Contains(player))
        {
            team2.Remove(player);
        }
        UpdatePlayerList();
    }


    public void ToggleBots()
    {
         if (PhotonNetwork.isMasterClient)
        {
        if(botToggle.isOn)
        {

            localSettings.addBots = true;

        }
        else
        {

             localSettings.addBots = false;

        }
        }

    }

    public void SetMaxPlayers()
    {
        PhotonNetwork.room.MaxPlayers = (int)playerLimitSlider.value;
        playerLimitText.text = "Player Limit " + "(" + PhotonNetwork.room.MaxPlayers + ")";
    }


    public void Reset()
    {
        startMatchButton.SetActive(false);
        playerLimitSlider.interactable = false;
        hiddenRoomToggle.interactable = false;
    }
}
