using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GodMoveObj : Photon.MonoBehaviour {
    //This script has been discontinued.
    public GameObject curr;
    public float objectMoveSpeed;
    Vector3 moveDir = Vector3.zero;

    //Check if the player can currently move using keyboard commands.
    bool CheckCanMove()
    {
        bool moving;
        GameObject god = GameObject.Find("Player(Clone)");
        if (god.GetComponent<GodLookAandMove>().canMove || god.GetComponent<GodLookAandMove>().movingToPlayer)
        {
            moving = true;
        }
        else
        {
            Image movingIMG = GameObject.Find("MoveObj").GetComponent<Image>();
            movingIMG.color = Color.red;
            moving = false;
        }
        return moving;
    }
    //To be run by all clients to tell them the object has moved.
    [PunRPC]
    public void MoveObject(float forwardMove, float rightMove)
    {
        moveDir = new Vector3(rightMove, 0, forwardMove);
    }
    //Get the current object if it has a photonview on it.
    void GetCurrent()
    {
        curr = gameObject.GetComponent<GodSelectObject>().current;
        if (curr != null)
        {
            if (curr.GetComponent<PhotonView>() == null)
            {
                curr = null;
            }
        }
    }
    //Handle keyboard commands.
    void InputManager()
    {
        if (!CheckCanMove())
        {
            float forMove = 0, rightMove = 0;
            if (Input.GetKey(KeyCode.W))
            {
                forMove = objectMoveSpeed;
            }
            if (Input.GetKey(KeyCode.S))
            {
                forMove = -objectMoveSpeed;
            }
            if (Input.GetKey(KeyCode.D))
            {
                rightMove = objectMoveSpeed;
            }
            if (Input.GetKey(KeyCode.A))
            {
                rightMove = -objectMoveSpeed;
            }
            MoveObject(forMove, rightMove);
        }
        else
        {
            moveDir = new Vector3(0,0,0);
        }
    }

	void Update () {
        if (!GetComponent<GodSetup>().isGod)
        {
            return;
        }
        GetCurrent();
        InputManager();
	}
    private void FixedUpdate()
    {
        if (!GetComponent<GodSetup>().isGod)
        {
            return;
        }
        if (photonView.isMine)
        {
            if(curr != null)
            {
                curr.GetComponent<CharacterController>().Move(moveDir * Time.deltaTime * 1.0f);
            }
        }
    }
}
