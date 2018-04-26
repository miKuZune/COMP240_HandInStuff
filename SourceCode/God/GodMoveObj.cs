using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GodMoveObj : Photon.MonoBehaviour {

    public GameObject curr;
    public float objectMoveSpeed;
    Vector3 moveDir = Vector3.zero;

    bool CheckCanMove()
    {
        bool moving;
        GameObject god = GameObject.Find("Player(Clone)");
        if (god.GetComponent<GodLookAandMove>().canMove || god.GetComponent<GodLookAandMove>().movingToPlayer)
        {
            //Image movingIMG = GameObject.Find("MoveObj").GetComponent<Image>();
            //movingIMG.color = Color.white;
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

    [PunRPC]
    public void MoveObject(float forwardMove, float rightMove)
    {
        moveDir = new Vector3(rightMove, 0, forwardMove);

        //curr.transform.Translate(xMoveAmount, yMoveAmount, zMoveAmount);
    }

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
	// Update is called once per frame
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
