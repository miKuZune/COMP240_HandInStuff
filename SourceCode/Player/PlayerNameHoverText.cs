using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameHoverText : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Text playerNameTxt = GetComponent<Text>();
        playerNameTxt.text = GetComponentInParent<PhotonView>().owner.NickName;

		if (GetComponentInParent<PhotonView> ().owner.GetTeam () == PunTeams.Team.red) {
			playerNameTxt.color = Color.red;

		}else if(GetComponentInParent<PhotonView> ().owner.GetTeam () == PunTeams.Team.blue){
			playerNameTxt.color = Color.blue;
		}

        if (GetComponentInParent<PhotonView>().isMine)
        {
            playerNameTxt.enabled = false;
        }
	}

    private void Update()
    {
		if (GetComponentInParent<PhotonView> () != null) {
			if (!GetComponentInParent<PhotonView> ().isMine) {
				//transform.LookAt(Camera.main.transform);
				transform.parent.LookAt (Camera.main.transform);
				//transform.Rotate(new Vector3());
				//transform.rotation = Quaternion.Euler(transform.rotation.x, -transform.rotation.y, transform.rotation.z);
			}
		}
    }

}
