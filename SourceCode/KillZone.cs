using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : Photon.MonoBehaviour {

	PlayerHealth h;
    GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Player hit" + other.transform.name);
            //h = other.GetComponent<PlayerHealth>();
            //h.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.All, 100f);
            if (other.gameObject.GetComponent<PhotonView>().owner.GetTeam() == PunTeams.Team.blue)
            {

                other.transform.position = gameManager.spawnPointBlue.transform.position;

            }
            else
            {
                other.transform.position = gameManager.spawnPointRed.transform.position;

            }

        }
	}

	void OnTriggerStay(Collider other)
	{
        if (other.gameObject.CompareTag("Player"))
        {
            //h = other.GetComponent<PlayerHealth>();
            //h.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.All, 100f);
            if (other.gameObject.GetComponent<PhotonView>().owner.GetTeam() == PunTeams.Team.blue)
            {

                other.transform.position = gameManager.spawnPointBlue.transform.position;

            }
            else
            {
                other.transform.position = gameManager.spawnPointRed.transform.position;

            }
        }
	}

    
}
