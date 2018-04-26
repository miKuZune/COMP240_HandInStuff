using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BOT_Attack : Photon.PunBehaviour
{

	public enum AttackState
	{

		GetPlayers, GetEnemyTeam, CheckIfEnemyIsReachable, Stop,
		StartAttack, MoveTowardsEnemy

	}

	public enum ShootingState
	{
	
		StartShooting, StopShooting

	}

	[Header("Attack State")]
	public AttackState currState;
	public ShootingState currShootingState;

	[Header("Attack Prepare")]
	public List<GameObject> players = new List<GameObject>();
	public List<GameObject> enemyTeam = new List<GameObject>();
	public int numOfEnemy = 0;
	public int emptyNumOfEn = 0;

	[Header("TARGET")]
	public GameObject target;
	public int AttackRange = 10;

	public GameObject rayCastPos;

	BOT_Profile bOT_Profile;
	BOT_Manager bOT_Manager;


	PhotonView photonView;

	Shoot shootScript;

	NavMeshAgent navMeshAgent;

	void Awake()
	{

		currState = AttackState.Stop;
		currShootingState = ShootingState.StopShooting;
		
	}

	public bool checkIfTargetIsInRange()
	{
		if (target != null && Vector3.Distance(transform.position, target.transform.position) < AttackRange)
		{
				Debug.Log("ENEMY IS IN RANGE");
				currShootingState = ShootingState.StartShooting;
				return true;
		}
		else
			Debug.Log("ENEMY IS NOT RANGE");
			currShootingState = ShootingState.StopShooting;
			return false;
	}

	// Use this for initialization
	void Start () 
	{
		navMeshAgent = GetComponent<NavMeshAgent> ();
		shootScript = GetComponent<Shoot> ();
		bOT_Profile = GetComponent<BOT_Profile>();
		bOT_Manager = GetComponent<BOT_Manager>();
		photonView = GetComponent<PhotonView>();
		Debug.Log(photonView.owner.GetTeam());
	}
	
	// Update is called once per frame
	void Update () 
	{

		if(checkIfTargetIsInRange())
		{

			Debug.Log(transform.name + "IN RANGE");

		}

		switch(currState)
		{

			case AttackState.Stop:
			break;

			case AttackState.GetPlayers:
			GetAllPlayers();
			break;

			case AttackState.GetEnemyTeam:
			GetOurEnemyPlayers();
			break;

			case AttackState.CheckIfEnemyIsReachable:
			IsEnemyReachable();
			break;

			case AttackState.StartAttack:
			break;

			case AttackState.MoveTowardsEnemy:
			MoveTowardsEnemy();
			break;

		}

		switch(currShootingState)
		{

			case ShootingState.StopShooting:
			break;

			case ShootingState.StartShooting:
			StartShooting ();
			break;

		}
		
	}

	//get all of the players
	public void GetAllPlayers()
	{

		players.AddRange(GameObject.FindGameObjectsWithTag("Player"));
		numOfEnemy = players.Count;
		currState = AttackState.GetEnemyTeam;

	}

	//Get a list of our enemy players
	public void GetOurEnemyPlayers()
	{

		if (emptyNumOfEn == numOfEnemy)
		{
			//Debug.Log("test");
			currState = AttackState.CheckIfEnemyIsReachable;
		}
			else
			{

			emptyNumOfEn++;

			for (int i = 0; i < numOfEnemy; i++)
			{

				GameObject playerObj = players[i];

				PlayerProfile playerProfile = playerObj.GetComponent<PlayerProfile>();
				BOT_Profile botProfile = playerObj.GetComponent<BOT_Profile>();

			if (!enemyTeam.Contains(playerObj))
			{	
				if (bOT_Profile.red == true)
				{
				
					if (playerProfile != null && playerProfile.blue == true)
					{
						enemyTeam.Add(playerObj);
						return;
					}

					if (botProfile != null && botProfile.blue == true)
					{
						enemyTeam.Add(playerObj);
					}
				}
				else
				{

					if (playerProfile != null && playerProfile.red == true)
					{
						enemyTeam.Add(playerObj);
						return;
					}
	
					if (botProfile != null && botProfile.red == true)
					{
						enemyTeam.Add(playerObj);
					}

				}

			}

				
		}
	}

	}

	//Check if any of the enemies are in sight
	public void IsEnemyReachable()
	{

		if (target == null)
		{
			foreach(GameObject enemy in enemyTeam)
			{

				RaycastHit rayHit;

				if(Physics.Linecast(rayCastPos.transform.position, enemy.transform.position, out rayHit))
				{

					if (rayHit.transform.gameObject == enemy )
					{
						target = rayHit.transform.gameObject;

						Debug.Log("OBJECT HAS BEEN HIT" + transform.name);
						bOT_Manager.currentStateWander = BOT_Manager.BOT_FStateMachineWander.Stop;
						bOT_Manager.targetDestination = target.transform.position;

						if (checkIfTargetIsInRange() && target.GetComponent<BOT_Profile>().isDead == false)
						{
							bOT_Manager.currentMoveState = BOT_Manager.BOT_FStateMachineMoveTowardsEnemy.Stop;
						}
						else
						{

							//bOT_Manager.currentMoveState = BOT_Manager.BOT_FStateMachineMoveTowardsEnemy.MoveTowardsEnemy;
							bOT_Manager.currentStateAttack = BOT_Manager.BOT_FStateMachineAttack.GetEnemyTeam;

						}

					}
					else
					{
						bOT_Manager.currentStateWander = BOT_Manager.BOT_FStateMachineWander.StartWander;
						bOT_Manager.currentStateAttack = BOT_Manager.BOT_FStateMachineAttack.GetEnemyTeam;
					}
	
					//Debug.Log("Blocked");

					Debug.DrawLine(rayCastPos.transform.position, enemy.transform.position, Color.red);

				}
			}
		}

	}

	public void MoveTowardsEnemy()
	{
		

	}

	public void StartShooting()
	{
		if (bOT_Profile.blue == true) 
		{
			transform.LookAt(target.transform);
			shootScript.Fire("water_pistol" + "_bullet", rayCastPos, 1400f, Vector2.one, PhotonNetwork.player, 25, false, -1);


		}

		if (bOT_Profile.red == true) 
		{
			transform.LookAt (target.transform);
			shootScript.Fire("water_pistol" + "_bullet", rayCastPos, 1400f, Vector2.one, PhotonNetwork.player, 25, true, -1);

		}


	}
		

}
