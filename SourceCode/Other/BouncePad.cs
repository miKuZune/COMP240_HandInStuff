using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour {

	public float power;

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player") && other.GetComponent<PhotonView> ().isMine) 
		{
			if (other.gameObject.GetComponent<PlayerMovement> ().canBeActedUponByBouncePad) {
				other.gameObject.GetComponent<PlayerMovement> ().verticalVel = power;
				other.gameObject.GetComponent<PlayerMovement> ().ResetBounceTimer();
			}
		}
	}
}
