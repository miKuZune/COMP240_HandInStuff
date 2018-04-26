using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodSetPositionAtMouse : Photon.MonoBehaviour {
    //Variables.
    public GameObject objToSpawn;
    Vector3 posToPlace;
    public GameObject tempButton;

    float yPosInc = 5;
    //Have the object follow the players mouse position while they choose to place it.
    void FollowMouse()
    {
        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        

        if (Physics.Raycast(ray, out hitInfo))
        {
            posToPlace = hitInfo.point;
        }
        posToPlace.y += yPosInc;
        
        transform.position = posToPlace;

    }
    //Check for when the player wants to place their object.
    void CheckIfDropped()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            OnDrop();
            Destroy(this.gameObject);
        }
    }
    //When the player drops their object.
    void OnDrop()
    {
        tempButton.GetComponentInParent<GodCooldown>().OnCooldown = true;

        Vector3 spawnPos = transform.position;
        spawnPos.y -= yPosInc;
		if (objToSpawn.name.Contains ("HealthPack")) 
		{
			spawnPos.y += 0.25f;
		}

		PhotonNetwork.Instantiate(objToSpawn.name, spawnPos, Quaternion.identity , 0);

    }

    void Update () {
        FollowMouse();
        CheckIfDropped();
    }
}
