using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : Photon.MonoBehaviour
{
    //Variables.
	public Transform myCam;
    Quaternion originalRot;

    public float baseSensitivity;

    public float MinimumX;
	public float MaximumX;

    public Transform startPos;

	public bool inPauseMenu;

	float xRot;


	void Start()
	{
		if (photonView.isMine && PhotonNetwork.connected)
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;

			inPauseMenu = false;

            GetSensitivity();

			myCam = Camera.main.transform;
			myCam.SetParent(gameObject.transform);
            myCam.localPosition = new Vector3(0, 9.9f, 0.55f);

            originalRot = myCam.transform.localRotation;
		}
	}
    //Load the sensitivity from storage.
    void GetSensitivity()
    {
        baseSensitivity = PlayerPrefs.GetFloat("Sensitivity") * 3;
    }
    //Handle player looking with mouse.
	void playerLook()
	{
		transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0) * baseSensitivity * Time.deltaTime);

		xRot = Input.GetAxis("Mouse Y") * baseSensitivity / 60;
        
        originalRot *= Quaternion.Euler (-xRot, 0f, 0f);
		originalRot = ClampRotationAroundXAxis (originalRot);

		myCam.transform.localRotation = originalRot;
	}

    //Stop the player from looking too far up or down.
    Transform ClampUpLook(Transform t)
    {
        Vector3 newV3 = t.transform.eulerAngles;

        newV3.y = Mathf.Clamp(newV3.y, MinimumX, MaximumX);

        t.eulerAngles = newV3;

        return t;
    }

	Quaternion ClampRotationAroundXAxis(Quaternion q)
	{
		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1.0f;

		float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

		angleX = Mathf.Clamp (angleX, MinimumX, MaximumX);

		q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);
        
		return q;
	}

	void Update ()
	{
        //Stop looking if god or if in a menu
		if (GetComponent<GodSetup>().isGod)
		{
			return;
		}else if(inPauseMenu)
		{
			return;
		}

		if (photonView.isMine && PhotonNetwork.connected)
		{
			if (Rounds.roundPaused)
			{
				return;
			}

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (!player.GetComponent<PlayerMovement>().isStunned)
            {
                playerLook();
            }

            GetSensitivity();
		}
	}
}
