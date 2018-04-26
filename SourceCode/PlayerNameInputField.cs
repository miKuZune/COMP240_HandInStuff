using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class PlayerNameInputField : MonoBehaviour
{

    static string playerNamePrefKey = "PlayerName";

	void Start ()
    {
        string defaultName = "";
        InputField inputField = this.GetComponent<InputField>();
        if(inputField != null)
        {
            if (PlayerPrefs.HasKey(playerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                inputField.text = defaultName;
            }
        }

        PhotonNetwork.playerName = defaultName;
	}
	
    public void SetPlayerName(string value)
    {
        // space added incase 'value' is empty
        PhotonNetwork.playerName = value + " ";
        PlayerPrefs.SetString(playerNamePrefKey, value);
    }
}
