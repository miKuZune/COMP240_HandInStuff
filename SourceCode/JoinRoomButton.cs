using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinRoomButton : MonoBehaviour
{

    string[] myName;
    string finalName;

	// Use this for initialization
	void Start ()
    {
        myName = GetComponentInChildren<Text>().text.Split('(');
        finalName = myName[0].Remove(myName[0].Length - 1);
	}
	
	public void JoinRoom()
    {
        Launcher.JoinRoom(finalName);
    }
}
