using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GodLookAandMove : Photon.MonoBehaviour
{

    public float moveSpeed;
    public float sensitivity;

    private Quaternion newRot;

    Vector3 moveDir = Vector3.zero;
    CharacterController charController;

    public bool canMove;

    public Transform myCam;

    GameObject god = null;

    bool godLooking;
    public bool movingToPlayer;
    public GameObject moveToPlayerPos;

    const float zUpLimit = 70, zLowLimit = -70;
    const float xUpLimit = 60, xLowLimit = -60;
    const float yUpLimit = 100, yLowLimit = 10;

    // Use this for initialization
    void Start()
    {
        if (!GetComponent<GodSetup>().isGod)
        {
            return;
        }
        god = gameObject;

        moveSpeed = 10;
        canMove = true;
        charController = GetComponent<CharacterController>();

        movingToPlayer = false;

        GameObject UI = GameObject.Find("GodUI");
        UI.SetActive(true);

        if (!photonView.isMine)
        {
            GetComponentInParent<Camera>().enabled = false;
            enabled = false;
        }
        if (photonView.isMine && PhotonNetwork.connected)
        {

            myCam = Camera.main.transform;

        }
    }

    public void SetCanMove()
    {
        Debug.Log("Getting god");
        getGod();
        Debug.Log("Setting god move");
        god.GetComponent<GodLookAandMove>().canMove = !god.GetComponent<GodLookAandMove>().canMove;
        newRot = transform.rotation;
    }

    void getGod()
    {
        god = GameObject.Find("Player(Clone)");
    }

    void LookWithMouse()
    {
        //transform.Rotate(-Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity, Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity, 0);
    }

    void Move()
    {
        moveDir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Mouse ScrollWheel") * 30000 * Time.deltaTime, -Input.GetAxis("Vertical"));
        moveDir = transform.TransformDirection(moveDir);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<GodSetup>().isGod)
        {
            return;
        }
        if (photonView.isMine == false && PhotonNetwork.connected == true)
        {
            return;
        }

        if (godLooking)
        {
            LookWithMouse();
        }
        Move();
    }
    private void FixedUpdate()
    {
        if (!GetComponent<GodSetup>().isGod)
        {
            return;
        }
        if (photonView.isMine)
        {
            if (canMove)
            {
                //GetComponent<CharacterController>().Move(moveDir * Time.deltaTime * moveSpeed);
                transform.Translate(moveDir * Time.deltaTime * moveSpeed);

                GameObject god = GameObject.Find("Player(Clone)");

                if (god.transform.position.y > yUpLimit)
                {
                    Vector3 newPos = god.transform.position;
                    newPos.y = yUpLimit;
                    god.transform.position = newPos;
                }
                else if (god.transform.position.y < yLowLimit)
                {
                    Vector3 newPos = god.transform.position;
                    newPos.y = yLowLimit;
                    god.transform.position = newPos;
                }
                if (god.transform.position.x > xUpLimit)
                {
                    Vector3 newPos = god.transform.position;
                    newPos.x = xUpLimit;
                    god.transform.position = newPos;
                }
                else if (god.transform.position.x < xLowLimit)
                {
                    Vector3 newPos = god.transform.position;
                    newPos.x = xLowLimit;
                    god.transform.position = newPos;
                }
                if (god.transform.position.z > zUpLimit)
                {
                    Vector3 newPos = god.transform.position;
                    newPos.z = zUpLimit;
                    god.transform.position = newPos;
                }
                else if (god.transform.position.z < zLowLimit)
                {
                    Vector3 newPos = god.transform.position;
                    newPos.z = zLowLimit;
                    god.transform.position = newPos;
                }

            }
            else if (movingToPlayer && !canMove)
            {
                GameObject god = GameObject.Find("Player(Clone)");

                if(god == null || moveToPlayerPos == null)
                {
                    movingToPlayer = false;
                    canMove = true;
                    return;
                }

				Vector3 moveToPos = moveToPlayerPos.transform.position;
				moveToPos.y += 7.5f;

				god.transform.position = Vector3.MoveTowards(god.transform.position, moveToPos, 100.0f * Time.deltaTime);
                float dist = Vector3.Distance(god.transform.position, moveToPlayerPos.transform.position);
                //Debug.Log(dist);

                float xDist = god.transform.position.x - moveToPlayerPos.transform.position.x;
                float zDist = god.transform.position.z - moveToPlayerPos.transform.position.z;
                Vector2 godV2 = new Vector2(god.transform.position.x, god.transform.position.z);
                Vector2 playerV2 = new Vector2(moveToPlayerPos.transform.position.x, moveToPlayerPos.transform.position.z);
                dist = Vector2.Distance(godV2, playerV2);

				/*if ()
                {
                    getGod();
                    god.GetComponent<GodLookAandMove>().canMove = true;
                    //god.GetComponent<GodLookAandMove>().movingToPlayer = false;
                    //god.GetComponent<GodLookAandMove>().moveToPlayerPos = new Vector3(0, 0, 0);
                }*/

                //21 130
                /*
                if (transform.position.y < 21)
                {
                    transform.position = new Vector3(transform.position.x, 21, transform.position.z);

                }
                if (transform.position.y > 130)
                {
                    transform.position = new Vector3(transform.position.x, 130, transform.position.z);
                }*/

            

			}else if(!movingToPlayer && !canMove)
			{
				getGod();
				god.GetComponent<GodLookAandMove>().canMove = true;
			}
        }
    }
}