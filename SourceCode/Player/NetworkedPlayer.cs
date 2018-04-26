using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedPlayer : Photon.MonoBehaviour {
    //Variables
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

    void Start()
    {
        anim = GetComponent<Animator>();

		playerMovement = GetComponent<PlayerMovement> ();

		if (photonView.isMine) {
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

				updatePing = 2f;
			}
        }else
        {
            transform.position = Vector3.Lerp(transform.position, realPos, 0.1f);
            transform.rotation = Quaternion.Lerp(transform.rotation, realRot, 0.1f);
        }
    }

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {

				stream.SendNext(transform.position);
				stream.SendNext(transform.rotation);  
		
			stream.SendNext (firstPersonStuff.GetBool ("isFiring"));
			stream.SendNext (firstPersonStuff.GetBool ("isReloading"));

			stream.SendNext(playerMovement.speedTriggerThings);


        }else
        {
			
            realPos = (Vector3)stream.ReceiveNext();
            realRot = (Quaternion)stream.ReceiveNext();

			anim.SetBool("isFiring", (bool)stream.ReceiveNext());
			anim.SetBool("isReloading", (bool)stream.ReceiveNext());

			playerMovement.speedTriggerThings = (Vector2)stream.ReceiveNext ();
        }
    }
}
