using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotColorChange : MonoBehaviour 
{


	BOT_Profile bOT_Profile;

	public Material redChar;
	public Material blueChar;
	// Use this for initialization
	void Start () 
	{

		bOT_Profile = GetComponentInParent<BOT_Profile>();

		if (bOT_Profile.red == true)
		{
			GetComponent<Renderer>().material = redChar;

		}

		if (bOT_Profile.blue == true)
		{
			GetComponent<Renderer>().material = blueChar;
		}

		
	}
	
}
