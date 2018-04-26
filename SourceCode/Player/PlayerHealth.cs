using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Photon.MonoBehaviour {

    public float maxHealth = 100f;
    public float currHealth;

    GameObject damageFx;

    bool fadePain;
    float a = 1;
    float fadeTimer = 0.3f;

	Slider hpSlider;

	public AudioSource bulletHitFleshSFX;

	Respawn respawn;

    public PhotonPlayer lastToDamageMe;


	// Use this for initialization
	void Start ()
    {
        currHealth = maxHealth;
		respawn = GetComponent<Respawn> ();

		if (photonView.isMine) {
			hpSlider = GameObject.Find ("HitPointsSlider").GetComponent<Slider> ();
			UpdateGUI ();
			damageFx = GameObject.Find ("TakeDamageImage");
			damageFx.GetComponent<Image> ().color = new Color (1, 1, 1, 0);
		} else {
			bulletHitFleshSFX.volume = 0.5f;
			bulletHitFleshSFX.spatialBlend = 1f;
		}
	}
	
    [PunRPC]
	public void TakeDamage(float damage)
    {
		if (respawn.dead) {
			return;
		}

        currHealth -= damage;
        if(currHealth <= 0)
        {
            Die();
        }
		if (photonView.isMine) {
			UpdateGUI ();
            fadePain = true;
            damageFx.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }

		bulletHitFleshSFX.Play ();
    }

    [PunRPC]
    public void AddHealth(int amount)
    {
        currHealth += amount;
        if(currHealth > maxHealth)
        {
            currHealth = maxHealth;
        }
        if (photonView.isMine)
        {
            UpdateGUI();
        }
    }

    private void Update()
    {
        if (photonView.isMine)
        {
            if (fadePain && fadeTimer > 0f)
            {
                //Debug.Log("in");
                a -= Time.deltaTime * 4;

                damageFx.GetComponent<Image>().color = new Color(1, 1, 1, a);
                fadeTimer -= Time.deltaTime;
            }
            if (fadeTimer <= 0f)
            {
                //Debug.Log("out");
                fadePain = false;
                damageFx.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                fadeTimer = 0.3f;
                a = 1f;
            }
        }
    }

    [PunRPC]
    public void SetLastToDamage(PhotonPlayer player)
    {
        this.lastToDamageMe = player;
    }

    public void Die()
    {
		currHealth = maxHealth;
		if (photonView.isMine) {

            // add kill
            if(lastToDamageMe != null)
            {
                GetComponent<PhotonView>().RPC("AddKill", PhotonTargets.All, lastToDamageMe);
                GetComponent<PhotonView>().RPC("AddToKillFeed", PhotonTargets.All, lastToDamageMe, PhotonNetwork.player);
                // maybe show the number of kills somewhere to debug?
                GetComponent<PhotonView>().RPC("AddKillToStreak", PhotonTargets.All, lastToDamageMe);
            }

            

            UpdateGUI ();


            //Reset all gun ammo on death.
            for(int i = 0; i < 4; i++)
            {
                GetComponent<PlayerShoot>().inventory.guns[i].CurrentAmmo = GetComponent<PlayerShoot>().inventory.guns[i].MagSize;
            }

            GetComponent<PlayerShoot>().ammo = GetComponent<PlayerShoot>().magSize;
			GetComponent<PlayerShoot> ().gunShotSFX.Stop ();
			GetComponent<PhotonView> ().RPC ("StopGunShot", PhotonTargets.All);
		}

		GetComponent<Respawn> ().OnDeath ();
    }

	void UpdateGUI()
	{
		hpSlider.value = currHealth;
	}

    void OnTriggerEnter(Collider other)
    {
        if(photonView.isMine)
        {
            if (other.CompareTag("projectile"))
            {
                if (other.GetComponent<PhotonView>().owner.GetTeam() == PhotonNetwork.player.GetTeam())
                {
                    Debug.Log("friendly fire");
                    return;
                }
                lastToDamageMe = other.gameObject.GetComponent<Projectile>().owner;
				GetComponent<PhotonView> ().RPC ("HitMarkerActivate", PhotonTargets.All, other.GetComponent<Projectile> ().bulletPlayerID);
				//Debug.Log (other.gameObject.GetComponent<Projectile> ().bulletPlayerID);
                GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.All, (float)other.gameObject.GetComponent<Projectile>().damage);
                other.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.player);
                //GetComponent<PhotonView>().RPC("DeleteBullet", lastToDamageMe, other.gameObject);
                PhotonNetwork.Destroy(other.gameObject);
                Debug.Log("ouch");

            }
        }
    }

 //   [PunRPC]
  //  public void DeleteBullet(GameObject bullet)
   // {
    //    PhotonNetwork.Destroy(bullet);
   // }
}
