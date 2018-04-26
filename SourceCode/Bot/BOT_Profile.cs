using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOT_Profile : Photon.PunBehaviour 
{

	
	public bool red;
	public bool blue;
	public string botName;
	public string teamString;

	public int Kills = 0;

	public float botHealth = 100f;
	public float botCurHealth;

	PhotonView photonView;

	public TextAsset botNames;
	public string[] arrayOfNames;

	BOT_RespawnManager bOT_RespawnManager;

	public bool isDead;

	// Use this for initialization
	void Start () 
	{

		botCurHealth = botHealth;

		bOT_RespawnManager = GetComponent<BOT_RespawnManager>();

		photonView = GetComponent<PhotonView>();

		if (red == true)
		{
		blue = false;
		photonView.owner.SetTeam(PunTeams.Team.red);
		}
		else
		{

		if(blue == true)
		{
		red = false;
		photonView.owner.SetTeam(PunTeams.Team.blue);
		}
		}
		
	}

	public string GetAndSetBotName()
	{

		arrayOfNames = botNames.text.Split('\n');
		int id = Random.Range(0, arrayOfNames.Length);
		botName = arrayOfNames[id];
		return botName;
	}

 	void OnTriggerEnter(Collider other)
    {


            if (other.CompareTag("projectile"))
            {
                if (other.GetComponent<PlayerProfile>().red == GetComponent<BOT_Profile>().red)
               {
                   Debug.Log("friendly fire RED");
                   return;
                }

				if (other.GetComponent<PlayerProfile>().blue == GetComponent<BOT_Profile>().blue)
               {
                   Debug.Log("friendly fire BLUE");
                   return;
                }


                GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.All, (float)other.gameObject.GetComponent<Projectile>().damage);
                other.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.player);
                PhotonNetwork.Destroy(other.gameObject);
                Debug.Log("ouch");

            }
    }

	[PunRPC]
	public void TakeDamage(float damage)
    {

        botCurHealth -= damage;
        if(botCurHealth <= 0)
        {
          Debug.Log("IM DEAD");
		  bOT_RespawnManager.Death();
        }
    }
}
