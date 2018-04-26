using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipperyFloor : MonoBehaviour {

	public float speedMultiplier = 1.5f;
	float storedSpeed;

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("Player")) 
		{
			if (other.gameObject.GetComponent<PhotonView> ().isMine) 
			{
				storedSpeed = other.gameObject.GetComponent<PlayerMovement> ().speed;
				other.gameObject.GetComponent<PlayerMovement> ().speed = storedSpeed * speedMultiplier;
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag ("Player")) 
		{
			if (other.gameObject.GetComponent<PhotonView> ().isMine) 
			{
				other.gameObject.GetComponent<PlayerMovement> ().speed = storedSpeed;
			}
		}
	}
}
