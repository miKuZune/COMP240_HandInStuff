using UnityEngine;
using System.Collections;

public class CameraFollow : Photon.MonoBehaviour
{

    GameObject playerCamera;    // The camera
    Vector3 vector;             // A vector
    public float speed = 3f;    // Camera speed

    public float offsetX;
    public float offsetY;
    public float offsetZ;

    public bool run = true;

    // Use this for initialization
    void Start()
    {
        // Assign camera
        if (photonView.isMine && PhotonNetwork.connected)
        {
            FindCamera();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.isMine && PhotonNetwork.connected)
        {
            vector = new Vector3(transform.position.x + offsetX, transform.position.y + offsetY, transform.position.z + offsetZ);

            playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, vector, speed * Time.fixedDeltaTime);

        }

    }

    public void FindCamera()
    {
        playerCamera = Camera.main.gameObject;
    }
}
