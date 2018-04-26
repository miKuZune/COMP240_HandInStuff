using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColorChange : Photon.PunBehaviour
{

    //If false then we are blue
    public bool teamColor = false;

    public Material blueChar;
    public Material redChar;

    // Use this for initialization
    void Start()
    {
		//if (photonView.isMine) 
		//{

			GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");

			foreach (GameObject player in players) {
				if (player.GetComponent<PlayerProfile> () != null) {

					if (player.GetComponent<PlayerProfile> ().red == true) {

                    gameObject.GetComponent<Renderer>().material = redChar;

					} else {

						gameObject.GetComponent<Renderer> ().material = blueChar;

					}

				} else {

					if (player.GetComponent<BOT_Profile> () != null) {

						if (player.GetComponent<BOT_Profile> ().red == true) {

							gameObject.GetComponent<Renderer> ().material = redChar;

						} else {

							gameObject.GetComponent<Renderer> ().material = blueChar;


						}

					}
				}

			//}
		}
    }
}