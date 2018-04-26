using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;

public class Launcher : Photon.PunBehaviour 
{

	public string gameVersion = "1";
	public PhotonLogLevel logLevel = PhotonLogLevel.Informational;
	public byte maxPlayers = 8;

    public GameObject controlPanel;
    public GameObject progressLabel;
    Text progressLabelText;
    public GameObject roomListPanel;

    public GameObject dropDownMapList;

    public GameObject joinButtonPrefab;
    public GameObject roomsPanel;

    public Text availableRooms;
    public Text playersOnline;

    public GameObject lobbyPanel;
	public GameObject mainMenuPanel;

    public GameObject optionsScreen;

    public Slider sensitivty;
    public Text sensitivityNum;


	Text mustEnterNameTxt;
	Text mustEnterLobbyNameTxt;

	public GameObject loadingPanel;



    bool isConnecting;

    public string levelToLoad;

	[Header("IS IT A PLAYTEST?")]
	public bool playTest;

	void Start () 
	{
		mustEnterNameTxt = GameObject.Find("MustEnterNameText").GetComponent<Text>();
		mustEnterNameTxt.enabled = false;
		mustEnterLobbyNameTxt = GameObject.Find("MustEnterRoomName").GetComponent<Text>();
		mustEnterLobbyNameTxt.enabled = false;
        progressLabelText = progressLabel.GetComponent<Text>();

        controlPanel.SetActive(true);

        HideAll();
        Cursor.visible = true;


		if (PlayerPrefs.GetFloat ("Sensitivity") == 0) {
			PlayerPrefs.SetFloat ("Sensitivity", 50);

		} else {
			float sens = PlayerPrefs.GetFloat ("Sensitivity");
			sensitivty.value = sens;
			sensitivityNum.text = "" + sens;
		}


        PhotonNetwork.logLevel = logLevel;

		// Don't need to auto-join as we will have room list.
		PhotonNetwork.autoJoinLobby = false;

		// Allow master client to change scene for all 
		// clients.
		PhotonNetwork.automaticallySyncScene = true;

		//Connect ();
	}

	public void ShowLoginScreen()
	{
		mainMenuPanel.SetActive (false);
		controlPanel.SetActive (true);
	}

	public void QuitGame(){
		Application.Quit ();
	}

	public void Connect()
	{
		if(PhotonNetwork.playerName != "" && PhotonNetwork.playerName != " ")
		{
			mustEnterNameTxt.enabled = false;
			isConnecting = true;
			progressLabel.SetActive(true);
			controlPanel.SetActive(false);

			// Check if connected, if not, connect.
			if (PhotonNetwork.connected) 
			{
            

				//PhotonNetwork.JoinRandomRoom();
				PhotonNetwork.JoinLobby();
            

			} else 
			{
				PhotonNetwork.ConnectUsingSettings (gameVersion);
			}
		}else
		{
			mustEnterNameTxt.enabled = true;
		}
	}

    public void RefreshRoomListings()
    {
        ShowRooms();
    }

    public void CreateRoom()
    {
        string name = GameObject.Find("RoomNameInputField").GetComponent<RoomNameInputField>().roomName;
		if(name != "")
		{
			mustEnterLobbyNameTxt.enabled = false;
			PhotonNetwork.CreateRoom(name, new RoomOptions() { MaxPlayers = maxPlayers }, null);
			Debug.Log("Room created.");
		}else
		{
			//warning 
			mustEnterLobbyNameTxt.enabled = true;
		}
    }

    public static void JoinRoom(string roomToJoin)
    {
        PhotonNetwork.JoinRoom(roomToJoin);
    }


    public void LeaveRoom()
    {
        lobbyPanel.GetComponent<LobbyScreen>().Reset();
        PhotonNetwork.LeaveRoom();
    }

    public void StartMatch()
    {
		if (dropDownMapList.GetComponentInChildren<Text> ().text != "Please Choose") {
			

			bool startGame = true;
			// Check all players have joined a team
			foreach (PhotonPlayer player in PhotonNetwork.playerList) {
				if (player.GetTeam () != PunTeams.Team.red && player.GetTeam () != PunTeams.Team.blue) {
					// not all players have joined a team!
					startGame = false;
				}
			}

			if (startGame) {
				PhotonNetwork.room.IsOpen = false;

                // disable all UI for loading screen
                HideAll();
				loadingPanel.SetActive (true);

				PhotonNetwork.LoadLevel (levelToLoad);
			}
		}
    }

    void HideAll()
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(false);
        roomListPanel.SetActive(false);
        roomsPanel.SetActive(false);
        lobbyPanel.SetActive(false);
        controlPanel.SetActive(false);
        loadingPanel.SetActive(false);
        optionsScreen.SetActive(false);
    }

    public void ShowOptions()
    {
        HideAll();
        optionsScreen.SetActive(true);
    }

    public void ShowRoomList()
    {
        HideAll();
        roomListPanel.SetActive(true);
    }

    public void OnSensitivityChange()
    {
        PlayerPrefs.SetFloat("Sensitivity", sensitivty.value);
        sensitivityNum.text = "" + PlayerPrefs.GetFloat("Sensitivity");
    }

    #region PunBehaviour Callbacks

    public override void OnConnectedToMaster()
	{
        progressLabelText.text = "Connected to master server.";

        Debug.Log("Launcher.cs: OnConnectedToMaster() called by PUN");

        if (isConnecting)
        {
            //PhotonNetwork.JoinRandomRoom();
            PhotonNetwork.JoinLobby();

        }
	}

    public override void OnDisconnectedFromPhoton()
	{
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
        Debug.LogWarning("Launcher.cs: OnDisconnectedFromPhoton() called by PUN");  
	}

	public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
	{
        //Debug.Log("fuck");
		// Failed to join room, so create a new one.
		PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = maxPlayers }, null);

	}

	public override void OnJoinedRoom()
	{
        progressLabelText.text = "Joined room.";
        Debug.Log("Launcher.cs: OnJoinedRoom() called by PUN. Client in room.");

        // want to load lobby screen tbh

        roomListPanel.SetActive(false);
        roomsPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        lobbyPanel.GetComponent<LobbyScreen>().UpdatePlayerList();



        // join a random team dependent on spaces available?

        //PhotonNetwork.LoadLevel("Room for 1");
       
	}

    public override void OnJoinedLobby()
    {
        roomListPanel.SetActive(true);
        roomsPanel.SetActive(true);
        progressLabel.SetActive(false);
        lobbyPanel.GetComponent<LobbyScreen>().UpdatePlayerList();

		if (playTest) 
		{
			// disable room create and name
			GameObject disableButton = roomListPanel.transform.Find("Button_CreateRoom").gameObject;
			disableButton.SetActive (false);
			GameObject disableInputField = roomListPanel.transform.Find("RoomNameInputField").gameObject;
			disableInputField.SetActive (false);

			RoomInfo[] rooms = PhotonNetwork.GetRoomList ();
			if (rooms.Length < 1) {
				PhotonNetwork.CreateRoom ("playtest", new RoomOptions () { MaxPlayers = maxPlayers }, null);
			} /*else {
				foreach (RoomInfo room in rooms) {
					if (room.Name == "playtest") {
						if (room.IsOpen) {
							PhotonNetwork.JoinRoom ("playtest");
						} else {
							if (rooms.Length == 1) {
								//create
								PhotonNetwork.CreateRoom ("playtest_overflow", new RoomOptions () { MaxPlayers = maxPlayers }, null);
							} else {
								//join
								PhotonNetwork.JoinRoom ("playtest_overflow");
							}
						}
					} 
				}
			}*/



		}

        //ShowRooms();
    }


    public void OnChangeMap()
    {
		if (dropDownMapList.GetComponentInChildren<Text> ().text != "Please Choose") 
		{
			lobbyPanel.GetComponent<PhotonView> ().RPC ("UpdateMap", PhotonTargets.All,dropDownMapList.GetComponentInChildren<Text> ().text, dropDownMapList.GetComponent<Dropdown>().value);
			// dropDownMapList.GetComponentInChildren<Text> ().text, dropDownMapList.GetComponentInChildren<Dropdown>().options[dropDownMapList.GetComponentInChildren<Dropdown>().value].image
		}
    }

    public override void OnReceivedRoomListUpdate()
    {
        RefreshRoomListings();
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        // update playerlist
        lobbyPanel.GetComponent<LobbyScreen>().UpdatePlayerList();
		OnChangeMap ();
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        // update playerlist
        lobbyPanel.GetComponent<LobbyScreen>().UpdatePlayerList();
        lobbyPanel.GetComponent<PhotonView>().RPC("RemoveDisconnectedPlayerFromTeam", PhotonTargets.All, otherPlayer);
    }

    public override void OnLeftRoom()
    {
        
        roomListPanel.SetActive(true);
        roomsPanel.SetActive(true);
        progressLabel.SetActive(false);
        lobbyPanel.SetActive(false);
    }

    public override void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        lobbyPanel.GetComponent<LobbyScreen>().UpdatePlayerList();
        lobbyPanel.GetComponent<LobbyScreen>().UpdateToggles();
    }

    #endregion

    void ShowRooms()
    {
        foreach(Transform child in roomsPanel.transform)
        {
            Destroy(child.gameObject);
        }

        //string roomListString = "";
        RoomInfo[] roomList = PhotonNetwork.GetRoomList();
        foreach (RoomInfo room in roomList)
        {
            //Debug.Log("Room: " + room.Name);
            //roomListString = roomListString + "\n" + room.Name;
            GameObject button = Instantiate(joinButtonPrefab, roomsPanel.transform) as GameObject;
            if (room.IsOpen)
            {
                button.GetComponentInChildren<Text>().text = room.Name + " (" + room.PlayerCount + "/" + room.MaxPlayers + ")\n" + "Creating game...";
            }else
            {
                button.GetComponentInChildren<Text>().text = room.Name + " (" + room.PlayerCount + "/" + room.MaxPlayers + ")\n" + "<color=#FF0000>In Game!</color>";
            }
        }
        availableRooms.text = "Existing Lobbies (" + roomList.Length + "):";
        //progressLabelText.text = roomListString;
        playersOnline.text = "Players Online: " + PhotonNetwork.countOfPlayers;
    }
}
