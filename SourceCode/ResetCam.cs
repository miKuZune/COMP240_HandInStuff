using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetCam : MonoBehaviour {


	void Start ()
    {
        // Let players find their cams again
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            //player.GetComponent<CameraFollow>().FindCamera();
        }
    }

}
