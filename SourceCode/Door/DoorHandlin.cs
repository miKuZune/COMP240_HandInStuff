using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandlin : Photon.MonoBehaviour {

    public bool locked = false;
    public GameObject door;
    Vector3 startPos;
    bool hasMoved;
    float timer;
    // Use this for initialization
    void Awake () {
        startPos = transform.position;
        hasMoved = false;
	}
	
    void testIfLocked()
    {
        if(locked == false)
        {
            transform.Rotate(5, 5, 5);
        }
    }


    [PunRPC]
    public void UnlockDoor()
    {
        door = GameObject.Find("Door(Clone)");
        door.GetComponent<DoorHandlin>().locked = false;
    }
    [PunRPC]
    public void LockDoor()
    {
        door = GameObject.Find("Door(Clone)");
        door.GetComponent<DoorHandlin>().locked = true;
    }
    [PunRPC]
    public void OpenDoor()
    {
        door = GameObject.Find("Door(Clone)");
        Vector3 newPos = startPos;
        newPos.z += 3;
        transform.position = newPos;
        timer = 5;
        hasMoved = true;
    }
    [PunRPC]
    public void CloseDoor()
    {
        door = GameObject.Find("Door(Clone)");
        Vector3 newPos = startPos;
        newPos.z -= 3;
        transform.position = newPos;
        timer = 5;
        hasMoved = true;
    }
    [PunRPC]
    public void ResetDoor()
    {
        door = GameObject.Find("Door(Clone)");
        transform.position = startPos;
    }
    // Update is called once per frame
    void Update () {
        door = GameObject.Find("Door(Clone)");
        if (hasMoved)
        {
            timer -= Time.deltaTime;
            if(timer < 0)
            {
                GameObject.Find("Door(Clone)").GetComponent<PhotonView>().RPC("ResetDoor", PhotonTargets.All);
                hasMoved = false;
            }
        }
        
        //testIfLocked();	
	}
}