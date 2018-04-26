using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GodSetup : Photon.MonoBehaviour {

    GameObject mainPlayerCanvas = null;
    public GameObject godPlayerCanvas = null;
    public bool isGod = false;

	GameObject myArms;

	GameObject myRig;

	GameObject thirdpersonmesh;
	GameObject thirdpersonGun;
	GameObject playerLabel;

    CharacterController localController;

	//public GameObject[] shitToDisableWhenGod = new GameObject[5];
	List<GameObject> shit = new List<GameObject>();

	void Start(){
		if (photonView.isMine) {
			godPlayerCanvas = GameObject.Find ("GodUI");

			godPlayerCanvas.SetActive (false);
			//myArms = Camera.main.transform.Find ("FPSARMSNEW").gameObject;
			myRig = GameObject.Find("WaterPistol_Idle");
            localController = GetComponent<CharacterController>();
		} 

		if (!photonView.isMine) {
			thirdpersonmesh = transform.Find ("Player_3PNew:Character_Unwrapped:Egor").gameObject;
			thirdpersonGun = GetComponentInChildren<FindMe>().gameObject;
			playerLabel = GetComponentInChildren<Text>().gameObject;
		}

		shit.Add (GameObject.Find ("TakeDamageImage"));
		shit.Add (GameObject.Find ("HitPointsSlider"));
		shit.Add (GameObject.Find ("AmmoText"));
		shit.Add (GameObject.Find ("CrosshairImage"));
		if (GameObject.Find ("myweaponwheel") != null) {shit.Add (GameObject.Find ("myweaponwheel"));}

		//shit.Add (GameObject.Find("ThisIsMyHand"));



	}

    public void InitialSetup()
    {
        Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
        //mainPlayerCanvas.SetActive(false);

        localController.enabled = false;

		if (photonView.isMine) {
			//myArms.GetComponentInChildren<SkinnedMeshRenderer> ().enabled = false;
		}


		foreach (GameObject thing in shit) {
			thing.SetActive (false);
		}

		//myArms.SetActive (false);
		myRig.SetActive(false);


        godPlayerCanvas.SetActive(true);
		GameObject.Find ("Cooldowns").SetActive (true);


        isGod = true;
        transform.position = new Vector3(0, 100, 0);
        transform.rotation = Quaternion.Euler(90, 0, 0);
		Camera.main.transform.localRotation = Quaternion.Euler (0, 0, 0);

		GetComponent<PhotonView> ().RPC ("DisableOther", PhotonTargets.Others);
    }

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

    public void UnSetup()
    {
        Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

        localController.enabled = true;
        //mainPlayerCanvas.SetActive(true);
        godPlayerCanvas.SetActive(false);
        isGod = false;
		//myArms.GetComponentInChildren<SkinnedMeshRenderer> ().enabled = true;
		myRig.SetActive(true);
		foreach (GameObject thing in shit) {
			thing.SetActive (true);
		}
		transform.rotation = Quaternion.Euler(0, 0, 0);
		//myArms.SetActive (true);

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
        

        //mainPlayerCanvas.SetActive(false);
        godPlayerCanvas.SetActive(true);
    }
		
}
