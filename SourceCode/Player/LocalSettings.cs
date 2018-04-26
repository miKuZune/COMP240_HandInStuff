using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LocalSettings : MonoBehaviour 
{

	public bool addBots = false;
	public bool botsCanStart = false;
	
	void Start()
	{

		DontDestroyOnLoad(gameObject);
		
	}

}
