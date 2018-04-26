using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableThirdPersonStuff : Photon.MonoBehaviour {

	public Transform gun;
	public Transform gunfUCK;

	// Use this for initialization
	void Start () {
		if (photonView.isMine) 
		{
			// disable player skin
			// disable gun mesh bits

			transform.Find ("Player_3PNew:Character_Unwrapped:Egor").GetComponent<SkinnedMeshRenderer> ().enabled = false;


			foreach (Transform gunpart in gun.transform) {
				if (gunpart.GetComponent<MeshRenderer> () != null) {
					gunpart.GetComponent<MeshRenderer> ().enabled = false;
				}
			}

			foreach (Transform gunpart in gunfUCK.transform) {
				if (gunpart.GetComponent<MeshRenderer> () != null) {
					gunpart.GetComponent<MeshRenderer> ().enabled = false;
				}
			}

			//transform.Find ("Shotgun_Placeholder:pCube3").GetComponent<MeshRenderer> ().enabled = false;
			//transform.Find ("Shotgun_Placeholder:Pump1").GetComponent<MeshRenderer> ().enabled = false;

			//transform.Find ("Shotgun_Placeholder:pCube2").GetComponent<MeshRenderer> ().enabled = false;
			//transform.Find ("Shotgun_Placeholder:Shotgun").GetComponent<MeshRenderer> ().enabled = false;
			//transform.Find ("Shotgun_Placeholder:Shotgun1").GetComponent<MeshRenderer> ().enabled = false;
			//transform.Find ("Shotgun_Placeholder:Shotgun2").GetComponent<MeshRenderer> ().enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
