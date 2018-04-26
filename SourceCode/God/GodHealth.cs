using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GodHealth : Photon.MonoBehaviour {
    //Used to spawn health packs that players on the ground can pick up.
    
    //Variables
    public GameObject healthPack;
    public Vector3 spawnPos;

    private GameObject current;

    public RectTransform spawnMove;

    public bool hasSpawned;
    public float spawnTimer;
    public float spawnCooldown;

    //Performed when a health pack is spawned.
    public void OnSpawn()
    {
        bool spawned = GameObject.Find("Player(Clone)").GetComponent<GodHealth>().hasSpawned;
        if (spawned)
        {
            return;
        }
        GameObject.Find("Player(Clone)").GetComponent<GodHealth>().hasSpawned = true;
        GameObject.Find("Player(Clone)").GetComponent<GodHealth>().spawnTimer = GameObject.Find("Player(Clone)").GetComponent<GodHealth>().spawnCooldown;
        Image UIimage = GameObject.Find("SpawnHealth").GetComponent<Image>();
        UIimage.color = Color.red;
    }

    //Handle cooldown
    public void SpawnCooldown()
    {
        GameObject.Find("Player(Clone)").GetComponent<GodHealth>().spawnTimer -= Time.deltaTime;
        if(spawnTimer < 0)
        {
            GameObject.Find("Player(Clone)").GetComponent<GodHealth>().hasSpawned = false;
            Image UIimage = GameObject.Find("SpawnHealth").GetComponent<Image>();
            UIimage.color = Color.white;
        }
    }
    
    
    //To be sent to each client to spawn a health pack.
	[PunRPC]
    public void Spawn()
    {
        bool spawned = GameObject.Find("Player(Clone)").GetComponent<GodHealth>().hasSpawned;
        if (spawned)
        {
            return;
        }

        Despawn();
        //Sets the spawn pos.
        SpawnPosByPosRaycast();

        //Will need changing when we have god mode implemented to have some way to decide where to spawn the object.
        Vector3 newSpawnPos;


        newSpawnPos = GameObject.FindGameObjectWithTag("Player").GetComponent<GodSelectObject>().GetPosition;
        newSpawnPos.y += 0.75f;

        current = PhotonNetwork.Instantiate(healthPack.name, newSpawnPos, Quaternion.identity, 0);
                
    }

    //These methods are sent to each client to tell them that the health pack has moved in a direction.

    [PunRPC]
    public void Forward()
    {
        Vector3 newPos;
        newPos = current.transform.position;
        newPos.z += 1;

        current.transform.position = newPos;
        StopShowMoveGUI();
    }
    [PunRPC]
    public void Back()
    {
        Vector3 newPos;
        newPos = current.transform.position;
        newPos.z -= 1;

        current.transform.position = newPos;
        StopShowMoveGUI();
    }
    [PunRPC]
    public void Right()
    {
        Vector3 newPos;
        newPos = current.transform.position;
        newPos.x += 1;

        current.transform.position = newPos;
        StopShowMoveGUI();
    }
    [PunRPC]
    public void Left()
    {
        Vector3 newPos;
        newPos = current.transform.position;
        newPos.x -= 1;

        current.transform.position = newPos;
        StopShowMoveGUI();
    }

    public void ShowMoveGUI()
    {
        spawnMove.gameObject.SetActive(true);
    }
    public void StopShowMoveGUI()
    {
        spawnMove.gameObject.SetActive(false);
    }

    //Destroys accross the network.
    public void Despawn()
    {
        if (current != null) { PhotonNetwork.Destroy(current); }
    }

    //Send a message to all the clients that the healthpack has moved.
    [PunRPC]
    public void Move()
    {
        Vector3 newPos;
        newPos = current.transform.position;
        newPos.x += 1;

        current.transform.position = newPos;
    }
    //Get the Vector3 position of where the users mouse is pointing.
    void SpawnPosByPosRaycast()
    {
        RaycastHit hitInfo;
        Ray ray = new Ray(transform.position,Vector3.down);
        if (Physics.Raycast(ray, out hitInfo))
        {
            spawnPos = hitInfo.point;
        }
    }

    void Update () {
        if (hasSpawned)
        {
            SpawnCooldown();
        }
    }
}