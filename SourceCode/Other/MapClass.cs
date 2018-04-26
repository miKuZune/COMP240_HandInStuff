using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MapClass 
{
    //Variable
	public Sprite image;
	public string desc;
    //Store variables to be dispalyed to people who are not hosting the game.
	public MapClass (Sprite image, string desc)
	{
		image = image;
		desc = desc;	
	}
}
