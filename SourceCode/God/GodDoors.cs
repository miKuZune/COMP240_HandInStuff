using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GodDoors : Photon.MonoBehaviour {
    //This script is discontinued.

    //Variables
    public GameObject door;
    public bool hasLocked;
    private float lockTimer;
    public float lockCooldown;
    public Image lockButton;

	void Start ()
    {
        hasLocked = false;
        lockCooldown = 5.0f;
  	}

    //Store the gameobject if there is a PhotonView on it.
    void GetCurrent()
    {
        door = gameObject.GetComponent<GodSelectObject>().current;
        if (door != null)
        {
            if (door.GetComponent<PhotonView>() == null)
            {
                door = null;
            }
        }
    }

    //Swap the door to be locked/unlocked.
    public void changeLockState()
    {
        if (hasLocked)
        {
            return;
        }

        bool doorLocked = GameObject.Find("Door(Clone)").GetComponent<DoorHandlin>().locked;

        if (doorLocked)
        {
            GameObject.Find("Door(Clone)").GetComponent<PhotonView>().RPC("UnlockDoor", PhotonTargets.All);
        }
        else
        {
            GameObject.Find("Door(Clone)").GetComponent<PhotonView>().RPC("LockDoor", PhotonTargets.All);
        }
        //Setup cooldown
        GameObject.Find("Player(Clone)").GetComponent<GodDoors>().hasLocked = true;
        GameObject.Find("Player(Clone)").GetComponent<GodDoors>().lockTimer = GameObject.Find("Player(Clone)").GetComponent<GodDoors>().lockCooldown;
        lockButton = GameObject.Find("LockDoor").GetComponent<Image>();
        lockButton.color = Color.red;
    }
    
    public void OpenDoor()
    {
        bool doorLocked = GameObject.Find("Door(Clone)").GetComponent<DoorHandlin>().locked;

        if (!doorLocked)
        {
            Debug.Log("Doing the door thing");
            GameObject.Find("Door(Clone)").GetComponent<PhotonView>().RPC("OpenDoor", PhotonTargets.All);
        }
    }

    public void CloseDoor()
    {
        bool doorLocked = GameObject.Find("Door(Clone)").GetComponent<DoorHandlin>().locked;

        if (!doorLocked)
        {
            Debug.Log("Doing the door thing");
            GameObject.Find("Door(Clone)").GetComponent<PhotonView>().RPC("CloseDoor", PhotonTargets.All);
        }
    }

    void Update()
    {
        GetCurrent();
        if (hasLocked)
        {
            GameObject.Find("Player(Clone)").GetComponent<GodDoors>().lockTimer = GameObject.Find("Player(Clone)").GetComponent<GodDoors>().lockTimer - Time.deltaTime;
            if (lockTimer < 0)
            {
                GameObject.Find("Player(Clone)").GetComponent<GodDoors>().hasLocked = false;
                lockButton = GameObject.Find("LockDoor").GetComponent<Image>();
                lockButton.color = Color.white;
            }
        }
    }
}
