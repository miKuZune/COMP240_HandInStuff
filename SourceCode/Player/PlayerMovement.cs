using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion;
using RootMotion.FinalIK;

public class PlayerMovement : Photon.MonoBehaviour
{

    public float speed;
    public float ySpeed;
    public float jump;
    //public float gravity;

    float startSpeed;

    //public float topSpeed;

    //public float drag;

    Vector3 moveDir = Vector3.zero;
    public float verticalVel = 0;

    CharacterController charController;
    Animator anim;

    public static GameObject localPlayerInstance;

	public Vector2 speedTriggerThings;

	//AimIK aim;
	//LookAtIK look;

	public AudioSource footstepsSFX;
	public AudioClip leftFoot;
	public AudioClip rightFoot;

	public Animator fpsAnimations;

    public bool isStunned;
    float timeStunned;


	public bool inPauseMenu;

	public float bouncePadTimer = 1.5f;
	float bouncePadStoredTime;
	public bool canBeActedUponByBouncePad;

    float airTime;

    private void Awake()
    {
		if (photonView.isMine) {
			localPlayerInstance = gameObject;
        
		} else {
			footstepsSFX.spatialBlend = 1f;
		}
        DontDestroyOnLoad(gameObject);
    }

    public void ResetSpeed()
    {
        speed = startSpeed;
        isStunned = false;
    }

    // Use this for initialization
    void Start ()
    {
        charController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

		inPauseMenu = false;

		//aim = GetComponent<AimIK> ();
		//look = GetComponent<LookAtIK> ();
		//aim.enabled = true;
		//look.enabled = true;
        isStunned = false;

        timeStunned = 0;

		bouncePadStoredTime = bouncePadTimer;

		if (photonView.isMine) {
			//fpsAnimations = Camera.main.GetComponentInChildren<Animator> ();
			fpsAnimations = GameObject.Find("WaterPistol_Idle").GetComponent<Animator>();
		}

        startSpeed = speed;
        ySpeed = speed;
	}
	

    void CheckForUnStun()
    {
        if(isStunned)
        {
            timeStunned += Time.deltaTime;
            if (timeStunned >= 1.5f)
            {
                isStunned = false;
                timeStunned = 0;
            }
            
        }
    }

	// Update is called once per frame
	void Update ()
    {
		#region maxStuff
		if (GetComponent<GodSetup>().isGod)
		{
			return;
		}
		#endregion

        if(photonView.isMine == false && PhotonNetwork.connected == true)
        {
			anim.SetFloat("InputV", speedTriggerThings.y);
			anim.SetFloat ("InputH", speedTriggerThings.x);
	


			if (speedTriggerThings.x > 1f || speedTriggerThings.y > 1f) {
				PlayFootStepsSFX ();
			}

            return;
        }


		speedTriggerThings = new Vector3(Input.GetAxis("Horizontal") * 10, Input.GetAxis("Vertical") * 10);


		moveDir = new Vector3 (Input.GetAxis ("Horizontal") * speed, 0, Input.GetAxis ("Vertical") * speed);
		
        moveDir = transform.TransformDirection(moveDir);

		if (bouncePadTimer > 0f) {
			canBeActedUponByBouncePad = false;
			bouncePadTimer -= Time.deltaTime;
		}
		if (bouncePadTimer <= 0f) {
			canBeActedUponByBouncePad = true;
		}


        //moveDir *= speed;

		fpsAnimations.SetFloat("speed", moveDir.magnitude);

        //anim.SetFloat("Speed", moveDir.magnitude * speed);
		anim.SetFloat("InputH", Input.GetAxis("Vertical") * 10);
		anim.SetFloat ("InputV", Input.GetAxis ("Horizontal") * 10);


        if (charController.isGrounded && verticalVel < 0)
        {
            airTime = 0;
            anim.SetBool("Jumping", false);

            verticalVel = Physics.gravity.y * Time.deltaTime * ySpeed;
        }
        else
        {
            airTime += Time.deltaTime;
            if(Mathf.Abs(verticalVel) > jump * 0.78f)
            {
                anim.SetBool("Jumping", true);
				//anim.SetTrigger("Jump");

            }
            verticalVel += Physics.gravity.y * Time.deltaTime * ySpeed;
        }


		if ((Input.GetAxis("Vertical") * 10) > 1f || (Input.GetAxis("Horizontal") * 10) > 1f) {
			PlayFootStepsSFX ();
		}

		if (charController.isGrounded && Input.GetButtonDown("Jump") && !inPauseMenu)
		{
			verticalVel = jump;
		}


        moveDir.y = verticalVel;
        

		if (inPauseMenu) 
		{
			moveDir = new Vector3 (0, moveDir.y, 0);
			fpsAnimations.SetFloat ("speed", 0);
		}

	}

    void FixedUpdate()
    {
		#region maxStuff
		if (GetComponent<GodSetup>().isGod)
		{
			return;
		}
        CheckForUnStun();
		#endregion
        if (photonView.isMine && !Rounds.roundPaused && !isStunned)
        {
			charController.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }

	void PlayFootStepsSFX(){
		if (footstepsSFX.clip == leftFoot && !footstepsSFX.isPlaying) {
			footstepsSFX.clip = rightFoot;
			footstepsSFX.Play ();
		}else if(footstepsSFX.clip == rightFoot && !footstepsSFX.isPlaying){
			footstepsSFX.clip = leftFoot;
			footstepsSFX.Play ();
		}
	}

	public void ResetBounceTimer(){
		bouncePadTimer = bouncePadStoredTime;
	}
}
