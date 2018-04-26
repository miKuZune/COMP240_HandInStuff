using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using UnityEngine.UI;

// This should be on the player!
public class KillStreaks : Photon.PunBehaviour 
{
	public KillStreak[] streaks;
	KillStreak myStreak;

	GameObject ksCanvas;
	GameObject killstreakNameTxt;
	GameObject killstreakTxt;
	AudioSource killstreakAudioSource;

	bool runTimer = false;
	public float howLongTextAppearsFor = 3f;
	float storedTimer;

	void Start()
	{
		PhotonNetwork.player.SetCustomProperties (new ExitGames.Client.Photon.Hashtable (){ { "kills", 0 } });

		if (photonView.isMine) 
		{
			ksCanvas = GameObject.Find ("KillStreakCanvas");
			killstreakNameTxt = ksCanvas.transform.GetChild (0).gameObject;
			killstreakTxt = ksCanvas.transform.GetChild (1).gameObject;
			killstreakAudioSource = ksCanvas.GetComponent<AudioSource> ();
			ksCanvas.SetActive (false);
		}

		storedTimer = howLongTextAppearsFor;
	}

	[PunRPC]
	public void AddKillToStreak(PhotonPlayer player)
	{
		if (photonView.isMine) 
		{
			int numberOfKills = (int)player.CustomProperties ["kills"];
			numberOfKills++;
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable (){ { "kills", numberOfKills } };

			player.SetCustomProperties (hashtable);

			myStreak = CheckForStreak (numberOfKills);
			if (myStreak != null && myStreak.killsRequired > 1) {
				DoKillStreak (player, myStreak);
			}
		}
	}

	[PunRPC]
	public void ResetStreak(PhotonPlayer player)
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable(){{"kills", 0}};
		player.SetCustomProperties (hashtable);
	}

	// For when game ends
	[PunRPC]
	public void ResetAllStreaks()
	{
		PhotonPlayer[] players = PhotonNetwork.playerList;
		foreach (PhotonPlayer player in players) 
		{
			ResetStreak (player);
		}
	}

	// Trigger text obj to be enabled (so animation runs), meanwhile set the player name and killstreak level
	// Oh and play a cool sound!

	public void DoKillStreak(PhotonPlayer player, KillStreak ks)
	{
		//ksCanvas.SetActive (true);
		//killstreakNameTxt.SetActive (true);
		//killstreakNameTxt.GetComponent<Text> ().text = ks.name;
		//killstreakTxt.SetActive (true);

		//if (ks.audio != null) 
		//{
		//	killstreakAudioSource.clip = ks.audio;
		//}
		//killstreakTxt.GetComponent<Text>().text = player.NickName + " " + ks.descriptiveText;

		runTimer = true;
	}

	public void HideKillStreak()
	{
		//killstreakTxt.SetActive (false);
		//killstreakNameTxt.SetActive (false);
		//ksCanvas.SetActive (false);
	}

	public KillStreak CheckForStreak(int numberOfKills)
	{
		foreach (KillStreak ks in streaks) 
		{
			if (numberOfKills == ks.killsRequired) 
			{
				return ks;
			}
		}
		return null;
	}

	void Update()
	{
		if (!photonView.isMine) 
		{
			return;
		}
		if (runTimer) 
		{
			howLongTextAppearsFor -= Time.deltaTime;

			if (howLongTextAppearsFor <= 0f) {
				howLongTextAppearsFor = storedTimer;
				runTimer = false;
				HideKillStreak ();
			}
		}
	}
}

[System.Serializable]
public class KillStreak
{
	public string name;
	public string descriptiveText;
	public int killsRequired;
	public AudioClip audio;
}
