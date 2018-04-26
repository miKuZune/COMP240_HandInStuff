using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShoot : Photon.MonoBehaviour {

	public float fireRate = 0.5f;
	float cooldown = 0f;
	public float damage = 25f;
	public int magSize = 30;
	public int ammo;

	float muzzleFlashIntensity = 2.31f;

	Light muzzleFlash;
	ParticleSystem myParts;

	public GameObject myGun;

	public GameObject bloodFx;
	public GameObject ricochetFx;

	PlayerHealth h;

	Animator gunAnimator;

	public Text ammoTxt;
	public GameObject reloadWarningPanel;

	public bool reloading;
	public float reloadTakesSecs = 2f;
	float reloadReset;
	GameObject reloadSlider;

	public AudioSource gunShotSFX;

	WeaponDatabase weaponDatabase;
	public Item currentWeapon;
	public Transform hand;
	public Transform leftHand;

	PlayerMovement playerMovement;

    public Inventory inventory;

	Animator actualGunAnimator;

	Animator thirdPersonAnimator;
	bool a = false;

	public bool inPauseMenu;
	public bool team;
	
	public Vector2 gunInaccuracy = new Vector2(1,1);

    public Shoot shootScript;

    public GameObject bulletspawnPoss;

	public RuntimeAnimatorController gun4;
	public RuntimeAnimatorController gun5;
	public RuntimeAnimatorController gun1;
	public RuntimeAnimatorController gun3;
	public RuntimeAnimatorController gun2;

	public RuntimeAnimatorController rdgun1;
	public RuntimeAnimatorController rdgun2;
	public RuntimeAnimatorController rdgun3;
	public RuntimeAnimatorController rdgun4;
	public RuntimeAnimatorController rdgun5;

	public bool updatingPos = true;

	public GameObject flamerZippo;
	public GameObject waterBottle;

	public AudioClip hitMarkerAudio;
	public AudioClip ironSightsIn;
	public AudioClip ironSightsOut;

	public Material zippoRed;
	public Material zippoBlue;
	public Material canRed;
	public Material canBlue;
	public Material harpoonRed;
	public Material harpoonBlue;
	public Material vacRed;
	public Material vacBlue;

	GameObject hitMarker;
	float hMarkActiveTime = 0;

    // Use this for initialization
    void Start ()
	{
		if (PhotonNetwork.player.GetTeam() == PunTeams.Team.red)
		{
			team = true;

		}else{
			team = false;
		}


        inventory = GetComponent<Inventory>();
        shootScript = GetComponent<Shoot>();

		thirdPersonAnimator = GetComponent<Animator> ();
		playerMovement = GetComponent<PlayerMovement> ();
		weaponDatabase = FindObjectOfType<WeaponDatabase> ();
		hand = GameObject.Find ("Character1_RightHand").transform;
		leftHand = GameObject.Find ("Character1_LeftHand").transform;
		if (currentWeapon == null) {
			currentWeapon = weaponDatabase.database [0];
		}

		

		if (!photonView.isMine) 
		{
			reloadWarningPanel = GameObject.Find("ReloadWarningPanel");
			if (GetComponent<PhotonView>().owner.GetTeam() == PunTeams.Team.red)
			{
				myGun.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
			}else if(GetComponent<PhotonView>().owner.GetTeam() == PunTeams.Team.blue)
			{
				myGun.GetComponentInChildren<MeshRenderer>().material.color = Color.blue;

			}

			/// AUDIO
			gunShotSFX.spatialBlend = 1f;

			// gunshotfx - parts are on 3rd person, child of shotgun
			muzzleFlash = GetComponentInChildren<Light> ();
			myParts = GetComponentInChildren<ParticleSystem> ();

		} else if(photonView.isMine)
		{
            bulletspawnPoss = GameObject.Find("BulletSpawnPos");
			muzzleFlash = Camera.main.GetComponentInChildren<Light> ();
			myParts = Camera.main.GetComponentInChildren<ParticleSystem> ();


			myGun.GetComponentInChildren<MeshRenderer> ().enabled = false;
			reloadWarningPanel = GameObject.Find("ReloadWarningPanel");
            gunAnimator = Camera.main.transform.Find("WaterPistol_Idle").GetComponent<Animator>();
			//gunAnimator = Camera.main.GetComponentInChildren<Animator>();
			ammo = magSize;
			ammoTxt = GameObject.Find("AmmoText").GetComponent<Text>();
			ammoTxt.text = ammo.ToString();

			reloadWarningPanel.SetActive(false);
			reloadReset = reloadTakesSecs;
			reloadSlider = GameObject.Find("ReloadSlider");
			reloadSlider.SetActive(false);

			hitMarker = GameObject.Find ("HitMarker");
			hitMarker.GetComponent<Image> ().enabled = false;
		}



		myGun.transform.localScale = Vector3.one;
		StartCoroutine("FuckWaitAss");


	}

    bool WeaponIDValid(int ID, int[] IDs)
    {
        bool isValid = true;

        //Return false if the ID is the same as one of the other IDs
        for(int i = 0; i < IDs.Length; i++)
        {
            if(ID == IDs[i])
            {
                Debug.Log("ID is the same as another id");
                isValid = false;
            }
        }

        //Ignore any value above the amount of guns
        if(ID > 6)
        {
            isValid = false;
        }

        //Ignore some guns (Idk which ones and i'm too lazy to look them up)
		if(ID == 0)
        {
            isValid = false;
        }

        return isValid;
    }


    void GetRandomWeapons()
    {
        int[] IDs = { 999, 999, 999, 999 };

        for(int i = 0; i < IDs.Length; i++)
        {
            int newID = 0;
            bool IDfoundIsValid = false;
            //Generate new weapon ID while weapon ID is not valid.
            while(!IDfoundIsValid)
            {
                
                newID = Random.Range(0, 6);
                IDfoundIsValid = WeaponIDValid(newID, IDs);
            }

            IDs[i] = newID;

            inventory.guns.Add(weaponDatabase.FetchItemByID(newID));
        }
    }

    //Choose random fuckass weapon
	IEnumerator FuckWaitAss(){
		yield return new WaitForSeconds (0.2f);

        // 0, 1, 3, 4, 5,
        /*for(int i = 0; i < 4; i++)
		{
			int number = Mathf.RoundToInt (Random.Range (0, 5));
			while (number == 2 || number == 0) 
			{
                //Random number
				number = Mathf.RoundToInt (Random.Range (0, 5));
			}
			inventory.guns.Add (weaponDatabase.FetchItemByID (number));
		}*/
        GetRandomWeapons();

        SetWeaponFirstPerson(inventory.guns[0].ID);
        inventory.equippedWeapon = (inventory.guns[0]);
		yield return null;
	}

	public void SetWeaponFirstPerson(int weaponID)
	{
		if (photonView.isMine) {
			if (reloadWarningPanel != null) {
				reloadWarningPanel.SetActive (false);
			}




			// switch anims
			int layer = currentWeapon.ID;
			gunAnimator.SetLayerWeight (layer, 0);

			currentWeapon = weaponDatabase.FetchItemByID (weaponID);
			magSize = currentWeapon.MagSize;
			fireRate = currentWeapon.FireRate;
			ammo = currentWeapon.CurrentAmmo;
			ammoTxt.text = ammo.ToString ();
			reloadTakesSecs = currentWeapon.ReloadTime;
			reloadReset = reloadTakesSecs;
			damage = currentWeapon.Damage;
			gunShotSFX.clip = currentWeapon.ShootSfx;



			if (weaponID == 4) {
				gunAnimator.runtimeAnimatorController = gun4;
			} else if (weaponID == 5) {
				gunAnimator.runtimeAnimatorController = gun5;
			} else if (weaponID == 1) {
				gunAnimator.runtimeAnimatorController = gun1;
			} else if (weaponID == 3) {
				gunAnimator.runtimeAnimatorController = gun3;
			} else if (weaponID == 2) {
				gunAnimator.runtimeAnimatorController = gun2;
			}

			//DESTROYY CURRENT GUN
			foreach (Transform bit in hand) {
				if (bit.name.Contains ("Character")) {
					// do nothing
				} else {
					Destroy (bit.gameObject);
				}
			}

			foreach (Transform bit in leftHand) {
				if (bit.name.Contains ("Character")) {
					// do nothing
				} else {
					Destroy (bit.gameObject);
				}
			}

			GameObject actualGun = null;


			PunTeams.Team myTeam = PhotonNetwork.player.GetTeam ();

			if (currentWeapon.Slug == "harpoon") {
				actualGun = Instantiate (Resources.Load<GameObject> ("Models/items/" + currentWeapon.Slug), leftHand) as GameObject;

			} else {
				actualGun = Instantiate (Resources.Load<GameObject> ("Models/items/" + currentWeapon.Slug), hand) as GameObject;
			}

			if (currentWeapon.Slug == "flamer") {
				actualGun.transform.localPosition = new Vector3 (-8.483f, -5.098f, -2.425f);
				actualGun.transform.localRotation = Quaternion.Euler (32.156f, -241.299f, 45.742f);
            	
				GameObject zippo = Instantiate (flamerZippo, leftHand.transform);
				zippo.transform.localPosition = new Vector3(-0.67f,0.292f, -0.262f);
				zippo.transform.localRotation = Quaternion.Euler(new Vector3 (17.126f, 88.453f, -88.47f));

				if (myTeam == PunTeams.Team.blue) {
					//Set mat to blue teams mat
					zippo.GetComponent<Renderer>().material = zippoBlue;
					actualGun.GetComponentInChildren<Renderer>().material = canBlue;
				} else if (myTeam == PunTeams.Team.red)
				{
					//Set mat to red team mats
					actualGun.GetComponentInChildren<Renderer>().material = canRed;
					zippo.GetComponent<Renderer>().material = zippoRed;
				}

				//17.126, 88.453, - 88.47
			}
			if (currentWeapon.Slug == "flaregun") {
				actualGun.transform.localPosition = new Vector3 (0.217f, 0.073f, -0.327f);
				actualGun.transform.localRotation = Quaternion.Euler (-0.381f, 151.7f, 180f);
			}
			if (currentWeapon.Slug == "harpoon") {
				actualGun.transform.localPosition = new Vector3 (-0.553f, 0.175f, 0.07f);
				actualGun.transform.localRotation = Quaternion.Euler (-5.588f, -172.08f, 6.407f);
				if (myTeam == PunTeams.Team.blue) 
				{
					actualGun.GetComponent<Renderer> ().material = harpoonBlue;
				} else if (myTeam == PunTeams.Team.red) 
				{
					actualGun.GetComponent<Renderer> ().material = harpoonRed;
				}
			}
			if (currentWeapon.Slug == "vac") {
				actualGun.transform.localScale = new Vector3 (2f, 2f, 2f);
				actualGun.transform.localPosition = new Vector3 (0.86f, -0.11313f, 0.153812f);
				actualGun.transform.localRotation = Quaternion.Euler (11.407f, -87.061f, -94.053f);
				if (myTeam == PunTeams.Team.blue) 
				{
					actualGun.GetComponentInChildren<Renderer> ().material = vacBlue;
				} else if (myTeam == PunTeams.Team.red) 
				{
					actualGun.GetComponentInChildren<Renderer> ().material = vacRed;
				}

			}
			if (currentWeapon.Slug == "water_pistol") {
				actualGun.transform.localPosition = new Vector3 (0.942f, -0.231f, 0.422f);

				GameObject bottle = Instantiate (waterBottle, leftHand.transform);
				bottle.transform.localPosition = new Vector3 (0.583f, -1.83f, 0.468f);
				bottle.transform.localRotation = Quaternion.Euler(new Vector3(-92, -1, 0));
			}
			if (currentWeapon.Slug == "squid") {
				actualGun.transform.localPosition = new Vector3 (0.936f, -0.587f, 0.133f);
				actualGun.transform.localRotation = Quaternion.Euler (3, 278.66f, 0.375f);
			}

			//Setting local size of the gun to whatever is put in the database.
			actualGun.transform.localScale = currentWeapon.FirstPersonSize;


			actualGunAnimator = actualGun.GetComponent<Animator> ();

			muzzleFlash = actualGun.GetComponentInChildren<Light> ();
			myParts = actualGun.GetComponentInChildren<ParticleSystem> ();

			// switch anims
			layer = weaponID;
			gunAnimator.SetLayerWeight (layer, 1);

			GetComponent<PhotonView> ().RPC ("Swap3rdPersonGun", PhotonTargets.All, currentWeapon.ID);
		}
	}

    public void DroppedLastWeapon()
    {
        reloadWarningPanel.SetActive(false);
        foreach (Transform bit in hand)
        {
            Destroy(bit.gameObject);
        }
        currentWeapon = null;
        magSize = 0;
        fireRate = 0;
        ammo = 0;
        ammoTxt.text = ammo.ToString();
        reloadTakesSecs = 0;
        reloadReset = reloadTakesSecs;
        damage = 0;
        gunShotSFX.clip = null;
    }

	void UpdateThirdPerson(RuntimeAnimatorController RAC)
	{
		Vector3 pos = this.transform.position;
		Quaternion rot = this.transform.rotation;
		Vector3 vel = this.GetComponent<Rigidbody> ().velocity;

		thirdPersonAnimator.runtimeAnimatorController = RAC;

		this.transform.position = pos;
		this.transform.rotation = rot;
		this.GetComponent<Rigidbody> ().velocity = vel;
	}

	[PunRPC]
	public void Swap3rdPersonGun(int currentGunId)
	{
		if (!photonView.isMine) 
		{
			updatingPos = false;
			foreach (Transform thing in myGun.transform) {
				Destroy (thing.gameObject);
			}

			// switch anims
			//int layer = 0;
			if (currentWeapon.Slug == "flamer") {
				//layer = currentWeapon.ID + 1;

				//thirdPersonAnimator.runtimeAnimatorController = 

			} else if (currentWeapon.Slug == "flaregun"){
				//layer = currentWeapon.ID + 1;
			}

			if (currentGunId == 4) {
				UpdateThirdPerson (rdgun4);
			} else if (currentGunId == 5) {
				UpdateThirdPerson (rdgun5);
			} else if (currentGunId == 1) {
				UpdateThirdPerson (rdgun1);
			} else if (currentGunId == 3) {
				UpdateThirdPerson (rdgun3);
			} else if (currentGunId == 2) {
				UpdateThirdPerson (rdgun2);

			}
			//thirdPersonAnimator.SetLayerWeight (layer, 0);

			currentWeapon = weaponDatabase.FetchItemByID (currentGunId);
			gunShotSFX.clip = currentWeapon.ShootSfx;

			// switch anims
			//layer = currentGunId + 1;
			//thirdPersonAnimator.SetLayerWeight (layer, 1);

			PunTeams.Team myTeam = PhotonNetwork.player.GetTeam ();

			GameObject myNewGun = Instantiate (Resources.Load<GameObject> ("Models/items/" + currentWeapon.Slug), myGun.transform) as GameObject;
			Debug.Log (myNewGun.name);

			if (currentWeapon.Slug == "flamer") {
				myNewGun.transform.localPosition = new Vector3 (4.722f, -7.735f, 2.231f);
				myNewGun.transform.localRotation = Quaternion.Euler (new Vector3 (0f, -83.3f, 0f));
				if (myTeam == PunTeams.Team.blue) {
					//Set mat to blue teams mat

				} else if (myTeam == PunTeams.Team.red)
				{
					//Set mat to red team mats

				}

			} else if (currentWeapon.Slug == "flaregun") {
				myNewGun.transform.localPosition = new Vector3 (0.358f, -0.046f, -0.005f);
				myNewGun.transform.localRotation = Quaternion.Euler (new Vector3 (0f, 90f, 0f));
			}else if(currentWeapon.Slug == "squid")
            {
                //Stop right there criminal scum! >:o
				myNewGun.transform.localPosition = new Vector3 (2.75f, 0.65f , 3.22f);
				myNewGun.transform.localRotation = Quaternion.Euler (-6.08f, 63.792f ,-75.685f);
				myNewGun.transform.localScale = new Vector3 (0.77f, 0.77f, 0.77f);
            }
            else if (currentWeapon.Slug == "harpoon")
            {
				myNewGun.transform.localPosition = new Vector3(1.093f, 0.483f, 3.402f);
				myNewGun.transform.localRotation = Quaternion.Euler(new Vector3(0, -103.776f, 0));
                myNewGun.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            }
            else if (currentWeapon.Slug == "water_pistol")
            {
				myNewGun.transform.localPosition = new Vector3(1.6f, 0.55f, 3.21f);
				myNewGun.transform.localRotation = Quaternion.Euler(new Vector3(-2.719f, 75.98f, 0.589f));
                myNewGun.transform.localScale = new Vector3(22,22,22);
            }
            else if (currentWeapon.Slug == "vac")
            {
				myNewGun.transform.localPosition = new Vector3(1.62f, 0.15f, 3.43f);
				myNewGun.transform.localRotation = Quaternion.Euler(new Vector3(4.25f, 109.433f, -6.586f));
            }

			muzzleFlash = myNewGun.GetComponentInChildren<Light> ();
			myParts = myNewGun.GetComponentInChildren<ParticleSystem> ();
		}
	}



	// Update is called once per frame
	void Update () {
		
		if (Input.GetKeyDown (KeyCode.X)) {
			DoStuff ();
		}

		#region maxStuff
		if (GetComponent<GodSetup>().isGod)
		{
			return;
		}
		#endregion

		if (inPauseMenu) {
			return;
		}

		if (!photonView.isMine) {
			// do 3rd person shit
			if (muzzleFlash != null) {
				if (muzzleFlash.intensity > 0f) {
					muzzleFlash.intensity -= Time.deltaTime * 20;
				}
			}
			return;
		} else {
			if (hitMarker.GetComponent<Image> ().enabled) {
				hMarkActiveTime += Time.deltaTime;
				if (hMarkActiveTime > 0.25f) {
					GameObject.Find("HitMarker").GetComponent<Image> ().enabled = false;
					hMarkActiveTime = 0;
				}
			}
		}

		if (!updatingPos) {
			updatingPos = true;
		}

		if(ammo <= 0)
		{
			gunAnimator.SetBool("isFiring", false);
		}

		if (reloading)
		{
			gunAnimator.SetBool("isFiring", false);
			gunAnimator.SetBool("isReloading", true);

			if (muzzleFlash != null) 
			{
				muzzleFlash.intensity = 0f;
			}
			reloadSlider.GetComponent<Slider>().value = reloadTakesSecs;
			reloadTakesSecs -= Time.deltaTime;

			if(reloadTakesSecs <= 0f)
			{
				reloadSlider.SetActive(false);
				ammo = magSize;
				currentWeapon.CurrentAmmo = ammo;
				ammoTxt.text = ammo.ToString();
				reloading = false;
				gunAnimator.SetBool("isReloading", false);
				gunShotSFX.clip = currentWeapon.ShootSfx;
			}
		}

		if (!reloading)
		{

			cooldown -= Time.deltaTime;
			if (Input.GetButton("Fire1") && !Input.GetButtonUp("Fire1") && !Rounds.roundPaused && currentWeapon != null)
			{
				Fire();
			}
			else if (Input.GetButtonUp("Fire1"))
			{
				gunAnimator.SetBool("isFiring", false);
			}
			else
			{
				gunAnimator.SetBool("isFiring", false);
			}

            if (Input.GetKeyDown(KeyCode.F))
            {
                gunAnimator.SetTrigger("inspect");
				if (currentWeapon.Slug == "squid") {
					actualGunAnimator.SetTrigger ("inspect");
				}
            }

			if (muzzleFlash != null && muzzleFlash.intensity > 0f)
			{
				muzzleFlash.intensity -= Time.deltaTime * 20;
			}

            if(Input.GetMouseButton(1) && !Rounds.roundPaused)
            {
                Camera.main.GetComponent<Animator>().SetBool("ironsights", true);

				//Play sound
				Camera.main.GetComponent<AudioSource>().clip = ironSightsIn;
				Camera.main.GetComponent<AudioSource> ().Play ();
                //gunAnimator.SetBool("ironsights", true);
            }else if(Input.GetMouseButtonUp(1))
            {
                //gunAnimator.SetBool("ironsights", false);
                Camera.main.GetComponent<Animator>().SetBool("ironsights", false);
				//Play sound
				Camera.main.GetComponent<AudioSource>().clip = ironSightsOut;
				Camera.main.GetComponent<AudioSource> ().Play ();
            }

			// FLAMER SHITAAA
			if(currentWeapon.Slug == "flamer" && photonView.isMine)
			{

				if (Input.GetMouseButtonDown (0) && ammo > 0) {
					gunShotSFX.clip = currentWeapon.ShootSfx;

					GetComponent<PhotonView> ().RPC ("PlayThirdPersonFlamerParticles", PhotonTargets.All);
					GetComponent<PhotonView> ().RPC ("PlayGunShot", PhotonTargets.All);
					a = false;
				} else if (Input.GetMouseButtonUp (0) || ammo <= 0 || !Input.GetMouseButton(0)) {
					if (!a) {
						GetComponent<PhotonView> ().RPC ("StopGunShot", PhotonTargets.All);
						GetComponent<PhotonView> ().RPC ("StopThirdPersonFlamerParts", PhotonTargets.All);
						gunShotSFX.Stop ();
						a = true;
					} else {

					}
				}
			}

			if (ammo <= 0)
			{
				if (!Rounds.roundPaused)
				{
					// show reload warning
					reloadWarningPanel.SetActive(true);
				}
				else
				{
					reloadWarningPanel.SetActive(false);
				}
			}

			if (Input.GetKeyDown(KeyCode.R) && !Rounds.roundPaused && ammo < magSize)
			{
				Reload();
			}
		}
	}

	void Fire()
	{
		if(cooldown > 0 || ammo <= 0)
		{

			return;
		}

		gunAnimator.SetBool("isFiring", true);
		if (currentWeapon.Slug == "flaregun") {
			actualGunAnimator.SetTrigger ("fire");
		}
        if (myParts != null && muzzleFlash != null)
        {
            myParts.Play();
            muzzleFlash.intensity = muzzleFlashIntensity;
        }

		//PLAY AUDIO
		if (currentWeapon.Slug != "flamer") 
		{
			GetComponent<PhotonView> ().RPC ("PlayGunShot", PhotonTargets.All);
		}

		ammo--;
		currentWeapon.CurrentAmmo = ammo;
		ammoTxt.text = ammo.ToString();

		cooldown = fireRate;

        // inaccuracyA should be set to currentWeapon.Accuracy
        Vector2 inaccuracyA = currentWeapon.Inaccuracy;
        // the 3rd variable is bullet speed, the 4th is x and y accuracy
        if (Input.GetMouseButton(1))
        {
            // if in ironsights, reduce inaccuracy by 75%
            inaccuracyA = (inaccuracyA / 2) / 2;
        }

        // bulletSpeed should be set to currentWeapon.BulletSpeed
        float bulletSpeed = currentWeapon.BulletSpeed;

		int playerID = PhotonNetwork.player.ID;


		shootScript.Fire(inventory.equippedWeapon.Slug + "_bullet", bulletspawnPoss, bulletSpeed, inaccuracyA, PhotonNetwork.player, currentWeapon.Damage, team, playerID);

	}


	[PunRPC]
	public void PlayThirdPersonFlamerParticles(){
		if (!photonView.isMine) {
			ParticleSystem.MainModule main = myParts.main;
			main.loop = true;
			myParts.Play ();
		}
	}

	[PunRPC]
	public void HitMarkerActivate(int playerID)
	{
		int yourPlayerID = PhotonNetwork.player.ID;

		if (yourPlayerID == playerID) 
		{
			//Show hitmarker
			GameObject.Find ("HitMarker").GetComponent<Image> ().enabled = true;
			hMarkActiveTime = 0;
			//Play sound
			if (hitMarkerAudio != null) {
				Camera.main.GetComponent<AudioSource> ().clip = hitMarkerAudio;
				Camera.main.GetComponent<AudioSource> ().Play ();
			} else {
				Debug.Log ("Forgot to attach the sound.");
			}
		}
	}
		
	void DoStuff()
	{
		GameObject.Find ("HitMarker").GetComponent<Image> ().enabled = true;
		hMarkActiveTime = 0;
	}

	[PunRPC]
	public void StopThirdPersonFlamerParts(){
		if (!photonView.isMine) {
			ParticleSystem.MainModule main = myParts.main;
			main.loop = false;
			myParts.Stop ();
		}
	}

	[PunRPC]
	public void PlayGunShot()
	{
		gunShotSFX.Play ();
		if (myParts != null) 
		{
			myParts.Play ();
		}
		if (muzzleFlash != null) 
		{
			muzzleFlash.intensity = muzzleFlashIntensity;
		}
	}


	[PunRPC]
	public void StopGunShot()
	{
		gunShotSFX.Stop ();
	}

	[PunRPC]
	public void SetAnimTrigger(){
		thirdPersonAnimator.SetTrigger ("switching");
	}

	public void Reload()
	{
		gunShotSFX.clip = currentWeapon.ReloadSfx;
		gunShotSFX.Play ();

		if (currentWeapon.Slug == "flaregun") {
			actualGunAnimator.SetTrigger ("reload");
		}
		reloadWarningPanel.SetActive(false);
		reloadTakesSecs = reloadReset;
		reloadSlider.SetActive(true);
		reloading = true;
	}

	public void DebugReload()
	{
		ammoTxt.text = ammo.ToString();
	}
}
