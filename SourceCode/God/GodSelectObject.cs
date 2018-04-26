using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodSelectObject : Photon.MonoBehaviour {

    public GameObject current;
    public Camera cam;
    public Vector3 GetPosition;


    void Start()
    {
        if (photonView.isMine && PhotonNetwork.connected)
        {
            cam = Camera.main;
        }
    }

    void GetNewObject()
    {
        RaycastHit hitInfo;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo, 10000))
        {
            if (current != null)
            {
                current.GetComponent<Renderer>().material.color = Color.green;
            }
            
            current = hitInfo.transform.gameObject;
            Debug.Log("GameObject is: " + current.name);
            if(current.GetComponent<PhotonView>() == null)
            {
                GetPosition = hitInfo.point;
                GetComponent<GodHealth>().spawnPos = GetPosition;
                current = null;
                return;
            }

            if (current.GetComponent<PhotonView>() == null)
            {
                return;
            }
            current.GetComponent<Renderer>().material.color = new Color(0.5f, 1, 1);
            if (current.GetComponent<PhotonView>().owner != PhotonNetwork.player)
                {
                    current.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.player);
                }
            
        }
    }

	// Update is called once per frame
	void Update () {
        if (!GetComponent<GodSetup>().isGod)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            GetNewObject();
        }
	}
}