using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script has been discontinued.
public class DoorHandlin : Photon.MonoBehaviour {
    //Variables
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
	
    //Check if the dorr is locked, if not do something.
    void testIfLocked()
    {
        if(locked == false)
        {
            transform.Rotate(5, 5, 5);
        }
    }

    //To be sent to all clients to unlock a door.
    [PunRPC]
    public void UnlockDoor()
    {
        door = GameObject.Find("Door(Clone)");
        door.GetComponent<DoorHandlin>().locked = false;
    }
    //To be sent to all clients to lock a door.
    [PunRPC]
    public void LockDoor()
    {
        door = GameObject.Find("Door(Clone)");
        door.GetComponent<DoorHandlin>().locked = true;
    }
    //To be sent to all clients to open a door.
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
    //To be sent to all clients to close a door
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
    //To be sent to all clients to set the door's position to it's origional.
    [PunRPC]
    public void ResetDoor()
    {
        door = GameObject.Find("Door(Clone)");
        transform.position = startPos;
    }
    // Update is called once per frame
    void Update () {
        //Get the door and reset it's position if it has been moved.
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
	}
}