using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedPlayer : Photon.MonoBehaviour {

    Vector3 realPos = Vector3.zero;
    Quaternion realRot = Quaternion.identity;
	Quaternion rotStored = Quaternion.identity;
    Animator anim;

	PlayerMovement playerMovement;

	Transform myBall;

	float updatePing = 2f;

	bool updateRot = false;

	ExitGames.Client.Photon.Hashtable customPropertiesHash;

	Vector3 realBallPos = Vector3.zero;

	Animator firstPersonStuff;

	//public int ping;

    void Start()
    {
		

        anim = GetComponent<Animator>();
		//myBall = transform.Find ("AimTarget");

		playerMovement = GetComponent<PlayerMovement> ();

		if (photonView.isMine) {
			//firstPersonStuff = Camera.main.GetComponentInChildren<Animator> ();
			firstPersonStuff = GameObject.Find("WaterPistol_Idle").GetComponent<Animator>();
		} else {
			rotStored = realRot;
		}
    }

    void Update()
    {
        if (photonView.isMine)
        {
			updatePing -= Time.deltaTime;

			if (updatePing <= 0f) {

				customPropertiesHash = new ExitGames.Client.Photon.Hashtable () {{ "Ping", PhotonNetwork.GetPing() }};
				PhotonNetwork.player.SetCustomProperties (customPropertiesHash);

				//PhotonNetwork.player.SetCustomProperties ("Ping", PhotonNetwork.GetPing ()); 
				updatePing = 2f;
			}
        }else
        {
            transform.position = Vector3.Lerp(transform.position, realPos, 0.1f);
            transform.rotation = Quaternion.Lerp(transform.rotation, realRot, 0.1f);

			// calculate rotation diff
			/*
			if (updateRot) {
			transform.rotation = Quaternion.Lerp (transform.rotation, realRot, 0.1f);

				if(transform.rotation == realRot){
					
					rotStored = realRot;
					updateRot = false;
				}


			}else if(realRot.eulerAngles.y - rotStored.eulerAngles.y > 125 || realRot.eulerAngles.y - rotStored.eulerAngles.y < -125){
				updateRot = true;
					
			}



			myBall.position = Vector3.Lerp(myBall.position, realBallPos, 0.1f);
*/
        }
    }

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {

				stream.SendNext(transform.position);
				stream.SendNext(transform.rotation);  
			
            
			//stream.SendNext(myBall.position);

            //stream.SendNext(anim.GetFloat("Speed"));
            //stream.SendNext(anim.GetBool("Jumping"));
			stream.SendNext (firstPersonStuff.GetBool ("isFiring"));
			stream.SendNext (firstPersonStuff.GetBool ("isReloading"));

			stream.SendNext(playerMovement.speedTriggerThings);

			//stream.SendNext (PhotonNetwork.GetPing ());

        }else
        {
			
            realPos = (Vector3)stream.ReceiveNext();
            realRot = (Quaternion)stream.ReceiveNext();
			//realBallPos = (Vector3)stream.ReceiveNext();

            //anim.SetFloat("Speed", (float)stream.ReceiveNext());
            //anim.SetBool("Jumping", (bool)stream.ReceiveNext());
			anim.SetBool("isFiring", (bool)stream.ReceiveNext());
			anim.SetBool("isReloading", (bool)stream.ReceiveNext());

			playerMovement.speedTriggerThings = (Vector2)stream.ReceiveNext ();
			//ping = (int)stream.ReceiveNext();

        }
    }
}
