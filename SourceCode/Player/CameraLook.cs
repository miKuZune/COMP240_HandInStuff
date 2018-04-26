using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : Photon.MonoBehaviour
{
	public Transform myCam;

	//public Transform aimBall;
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

    void GetSensitivity()
    {
        baseSensitivity = PlayerPrefs.GetFloat("Sensitivity") * 3;
    }

	void playerLook()
	{
		transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0) * baseSensitivity * Time.deltaTime);

		//limit Y rot
		xRot = Input.GetAxis("Mouse Y") * baseSensitivity / 60;
        
        originalRot *= Quaternion.Euler (-xRot, 0f, 0f);
		originalRot = ClampRotationAroundXAxis (originalRot);

		//Quaternion yQuat = Quaternion.AngleAxis (rot.x, Vector3.left);
		//myCam.transform.localRotation = originalRot * yQuat;

		myCam.transform.localRotation = originalRot;

		//myCam.transform.eulerAngles = new Vector3 (rot.x, myCam.transform.eulerAngles.y, myCam.transform.eulerAngles.z);


		//RaycastHit hitInfo;
		//Ray ray = myCam.GetComponent<Camera>().ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2));
		//if(Physics.Raycast(ray, out hitInfo, 100))
		//{
		//	aimBall.transform.position = hitInfo.point;
		//}


	}


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

	// Update is called once per frame
	void Update ()
	{
		#region maxStuff
		if (GetComponent<GodSetup>().isGod)
		{
			return;
		}else if(inPauseMenu)
		{
			return;
		}
		#endregion

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

			/*if (Input.GetKeyDown(KeyCode.Escape))
			{
				//Cursor.visible = true;
			}*/
		}

		if (!photonView.isMine) {
			// have the legs move if rotated more than a certain amount
		}
	}
}
