using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Bot manager handles the bot behavour
public class BOT_Manager : Photon.PunBehaviour 
{
	//WANDER	
	public enum BOT_FStateMachineWander
	{

		Stop, StartWander, Wander, StoppedWandering

	}
	///ATTACK
	public enum BOT_FStateMachineAttack
	{

		LookForEnemy, GetEnemyTeam, Stop

	}

	public enum BOT_FStateMachineMoveTowardsEnemy
	{

		MoveTowardsEnemy, Stop, Stopped

	}

	Animator anim;
	Vector2 speedTriggerThings;

	[Header("Wander State")]
	public BOT_FStateMachineWander currentStateWander;

	[Header("Attack State")]
	public BOT_FStateMachineAttack currentStateAttack;
	public BOT_FStateMachineMoveTowardsEnemy currentMoveState;

	//Allof 
	BOT_Wander botWander;
	BOT_Attack bOT_Attack;
	LocalSettings localSettings;

	public Vector3 targetDestination;

	NavMeshAgent navMeshAgent;

	bool started;
	//float hackytimer = 3.2f;

	public bool hasStarted = false;

	void Start()
	{
		anim = GetComponent<Animator> ();


		botWander = GetComponent<BOT_Wander>();
		bOT_Attack = GetComponent<BOT_Attack>();

		navMeshAgent = GetComponent<NavMeshAgent>();

		currentStateWander = BOT_FStateMachineWander.StartWander;
		currentStateAttack = BOT_FStateMachineAttack.LookForEnemy;
		currentMoveState = BOT_FStateMachineMoveTowardsEnemy.Stopped;
		localSettings = GameObject.Find("LocalSettings").GetComponent<LocalSettings>();
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!PhotonNetwork.isMasterClient)
		{
			return;
		}

		//if (!started) {
		//	hackytimer -= Time.deltaTime;
		//	if (hackytimer <= 0f) {
				//currentStateWander = BOT_FStateMachineWander.Wander;
				//StartWander();
		//		Debug.LogError ("aaa");
		//		started = true;
		//	}
		//}

		anim.SetFloat("InputV", speedTriggerThings.y);
		anim.SetFloat ("InputH", speedTriggerThings.x);

		///WANDER STATE
		switch(currentStateWander)
		{

			case BOT_FStateMachineWander.Stop:
			StopWandering();
			break;

			case BOT_FStateMachineWander.StartWander:
			StartWander();
			break;

			case BOT_FStateMachineWander.Wander:
			break;

			case BOT_FStateMachineWander.StoppedWandering:
			StoppedWandering();
			break;

		}

		///ATTACK STATE
		switch(currentStateAttack)
		{
			case BOT_FStateMachineAttack.LookForEnemy:
			StartAttackState();
			break;

			case BOT_FStateMachineAttack.GetEnemyTeam:
			break;

			case BOT_FStateMachineAttack.Stop:
			break;
		}

		switch(currentMoveState)
		{
			case BOT_FStateMachineMoveTowardsEnemy.MoveTowardsEnemy:
			MoveToTheEnemy();
			break;

			case BOT_FStateMachineMoveTowardsEnemy.Stop:
			navMeshAgent.isStopped = true;
			currentMoveState = BOT_FStateMachineMoveTowardsEnemy.Stopped;
			break;

			case BOT_FStateMachineMoveTowardsEnemy.Stopped:
			break;

		}
		
	}

	///
	///WANDER 
	///

	void StartWander()
	{

		if (localSettings.botsCanStart == true && hasStarted == false) 
		{
			currentStateWander = BOT_FStateMachineWander.Wander;
			botWander.curState = BOT_Wander.WanderState.Wander;
			hasStarted = true;
		} 
		else 
		{

			return;

		}

	}

	//Stop Everything
	void StopWandering()
	{

		botWander.StopWander();
		currentStateWander = BOT_FStateMachineWander.StoppedWandering;

	}

	//Stopped Wandering
	void StoppedWandering()
	{
	}

	///
	/// END OF WANDER
	///

	///
	///ATTACK
	///

	void MoveToTheEnemy()
	{

		//navMeshAgent.destination = targetDestination;

	}

	void StartAttackState()
	{

		if (localSettings.botsCanStart == true)
		{

			bOT_Attack.currState = BOT_Attack.AttackState.GetPlayers;
			currentStateAttack = BOT_FStateMachineAttack.GetEnemyTeam;
		}

	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			
			stream.SendNext (anim.GetBool ("isFiring"));
			stream.SendNext (anim.GetBool ("isReloading"));
			stream.SendNext(speedTriggerThings);

		}else
		{



			anim.SetBool("isFiring", (bool)stream.ReceiveNext());
			anim.SetBool("isReloading", (bool)stream.ReceiveNext());
			speedTriggerThings = (Vector2)stream.ReceiveNext ();

		}
	}
}
