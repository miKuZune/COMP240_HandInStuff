using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GodSetup : Photon.MonoBehaviour {
    //Variables
    GameObject mainPlayerCanvas = null;
    public GameObject godPlayerCanvas = null;
    public bool isGod = false;

	GameObject myRig;

	GameObject thirdpersonmesh;
	GameObject thirdpersonGun;
	GameObject playerLabel;

    CharacterController localController;

	List<GameObject> UIelements = new List<GameObject>();

	void Start(){
		if (photonView.isMine) {
			godPlayerCanvas = GameObject.Find ("GodUI");

			godPlayerCanvas.SetActive (false);
			myRig = GameObject.Find("WaterPistol_Idle");
            localController = GetComponent<CharacterController>();
		} 

		if (!photonView.isMine) {
			thirdpersonmesh = transform.Find ("Player_3PNew:Character_Unwrapped:Egor").gameObject;
			thirdpersonGun = GetComponentInChildren<FindMe>().gameObject;
			playerLabel = GetComponentInChildren<Text>().gameObject;
		}

		UIelements.Add (GameObject.Find ("TakeDamageImage"));
		UIelements.Add (GameObject.Find ("HitPointsSlider"));
		UIelements.Add (GameObject.Find ("AmmoText"));
		UIelements.Add (GameObject.Find ("CrosshairImage"));
		if (GameObject.Find ("myweaponwheel") != null) {UIelements.Add (GameObject.Find ("myweaponwheel"));}
	}
    //Handle setting up for players who are becoming gods.
    public void InitialSetup()
    {
        Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;

        localController.enabled = false;


		foreach (GameObject thing in UIelements) {
			thing.SetActive (false);
		}

		myRig.SetActive(false);


        godPlayerCanvas.SetActive(true);
		GameObject.Find ("Cooldowns").SetActive (true);


        isGod = true;
        transform.position = new Vector3(0, 100, 0);
        transform.rotation = Quaternion.Euler(90, 0, 0);
		Camera.main.transform.localRotation = Quaternion.Euler (0, 0, 0);

		GetComponent<PhotonView> ().RPC ("DisableOther", PhotonTargets.Others);
    }
    //Disable visuals for gods.
	[PunRPC]
	public void DisableOther(){
		if (!photonView.isMine) {
			thirdpersonmesh.SetActive (false);
			playerLabel.SetActive (false);
			foreach (Transform thing in thirdpersonGun.transform) {
				thing.gameObject.SetActive (false);
			}
		}
	}

    //Enables visuals of players coming from god mode.
	[PunRPC]
	public void EnableOther(){
		if (!photonView.isMine) {
			thirdpersonmesh.SetActive (true);
			playerLabel.SetActive (true);
			foreach (Transform thing in thirdpersonGun.transform) {
				thing.gameObject.SetActive (true);
			}
		}
	}
    //Handle players going back to being normal players.
    public void UnSetup()
    {
        Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

        localController.enabled = true;
        godPlayerCanvas.SetActive(false);
        isGod = false;
		myRig.SetActive(true);
		foreach (GameObject thing in UIelements) {
			thing.SetActive (true);
		}
		transform.rotation = Quaternion.Euler(0, 0, 0);

		GetComponent<PhotonView> ().RPC ("EnableOther", PhotonTargets.Others);

    }

    void Awake()
    {
        mainPlayerCanvas = GameObject.Find("PlayerUI");
        godPlayerCanvas = GameObject.Find("GodUI");
        if (!GetComponent<GodSetup>().isGod)
        {
            return;
        }
        
        godPlayerCanvas.SetActive(true);
    }
		
}
