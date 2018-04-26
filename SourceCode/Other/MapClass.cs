using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MapClass 
{

	public Sprite image;
	public string desc;

	public MapClass (Sprite image, string desc)
	{

		image = image;
		desc = desc;
	
	}
}
