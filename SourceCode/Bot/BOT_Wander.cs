using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BOT_Wander : MonoBehaviour 
{

	public enum WanderState
	{

		Stop,
		Wander,
		CheckPos,
		ShouldITakeABreak,
		TakeABreak,
		PickNewDestination

	}

	[Header("Wander State")]
	public WanderState curState;

	[Header("Bot Targets")]
	public Vector3 targetDestination;
	public Vector3 tempTarDes;
	Vector3 tempDes;

	[Header("Break Timer Things")]
	float randomTime = 0f;
	public float timer = 0f;

	//Agent stuffs
	NavMeshAgent navMeshAgent;
	NavMeshPath path;

	[Header("Target Color")]
	public Color color = new Color(0.2F, 0.3F, 0.4F, 0.5F);

	Animator anim;

	// Use this for initialization
	void Start () 
	{
		
		navMeshAgent = GetComponent<NavMeshAgent>();

		anim = GetComponent<Animator> ();

		//curState = WanderState.CheckPos;
		
	}

	private void Update() 
	{

		switch(curState)
		{

			case WanderState.PickNewDestination:
			GetOurNewDestination();
			break;

			case WanderState.Wander:
			StartWandering();
			break;

			case WanderState.CheckPos:
			CheckPos();
			break;

			case WanderState.ShouldITakeABreak:
			ShouldITakeABreak();
			break;

			case WanderState.TakeABreak:
			TakeABreak();
			break;

			case WanderState.Stop:
			break;

		}
	}

	public void GetOurNewDestination()
	{

			path = new NavMeshPath();
			navMeshAgent.CalculatePath(targetDestination, path);

			Debug.Log(navMeshAgent.CalculatePath(targetDestination, path));

			if(path.status == NavMeshPathStatus.PathPartial || path.status == NavMeshPathStatus.PathInvalid )
			{
	
				curState = WanderState.CheckPos;

			}
			else
			{

				navMeshAgent.SetPath(path);

			//	GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
       		//	cube.transform.position = targetDestination;

//				cube.GetComponent<MeshRenderer>().material.color = color;
				curState = WanderState.Wander;
			}

			 Debug.Log(navMeshAgent.CalculatePath(targetDestination, path));

	}

	public void CheckPos()
	{

		tempTarDes = RandomNavMeshPos(transform.position, 25f, -1);
		targetDestination = tempTarDes;
		//This should fix the Infinite bug
		Vector3 infVec3 = new Vector3 (99999, 99999, 99999);

		if (targetDestination.x > infVec3.x)
		{

			curState = WanderState.CheckPos;

		}
		else
		curState = WanderState.PickNewDestination;

	}

	//Start our Wander 
	public void StartWandering()
	{


		 if (!navMeshAgent.pathPending )
 			{
    		 if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
    	 	{
         		if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
         		{
             		curState = WanderState.ShouldITakeABreak;
        		 }
     	}
 }
 else
 			{

				navMeshAgent.destination = targetDestination;

			}

	}

	//Get the distance between our BOT and Target
	public float distanceBetweenBOTandTarget()
	{

		float dist = Vector3.Distance(transform.position, targetDestination);

		return dist;

	}

	//Get Random Pos
	//Use layer mask -1 for all masks
	public Vector3 RandomNavMeshPos(Vector3 origin, float distance, int layermask)
	{

		Vector3 randomPos = Random.insideUnitSphere * distance;
		randomPos += origin;

		NavMeshHit navHit;
		NavMesh.SamplePosition(randomPos, out navHit, distance, layermask);

		return navHit.position;

	}

	//See if I should take a break
	public void ShouldITakeABreak()
	{

		float value = Random.Range(0f, 1f);

		if (value > 0.75f)
		{

			randomTime = Random.Range(1f, 3f);
			timer = randomTime;
			curState = WanderState.TakeABreak;

		}
		else
		curState = WanderState.CheckPos;

	}

	//This is where we take our break 
	public void TakeABreak()
	{

		timer -= Time.deltaTime;

		if (timer <= 0f)
		{

			curState = WanderState.CheckPos;

		}

	}

	public void StopWander()
	{

		//destination = null;
		Debug.Log("" + transform.name);
		//navMeshAgent.isStopped = true;
		curState = WanderState.Stop;

	}

}
