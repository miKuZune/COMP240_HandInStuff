using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodSetPositionAtMouse : Photon.MonoBehaviour {

    public GameObject objToSpawn;
    Vector3 posToPlace;
    public GameObject tempButton;

    float yPosInc = 5;

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

    void CheckIfDropped()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            OnDrop();
            Destroy(this.gameObject);
        }
    }
    
    void OnDrop()
    {
        tempButton.GetComponentInParent<GodCooldown>().OnCooldown = true;

        Vector3 spawnPos = transform.position;
        spawnPos.y -= yPosInc;
        //objToSpawn.AddComponent<DespawnObj>();
		if (objToSpawn.name.Contains ("HealthPack")) 
		{
			spawnPos.y += 0.25f;
		}

		PhotonNetwork.Instantiate(objToSpawn.name, spawnPos, Quaternion.identity , 0);

    }

    // Update is called once per frame
    void Update () {
        FollowMouse();
        CheckIfDropped();
    }
}
