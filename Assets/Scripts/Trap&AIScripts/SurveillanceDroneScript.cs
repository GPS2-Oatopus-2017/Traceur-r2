using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveillanceDroneScript : MonoBehaviour {

	public EnemyData surveillance_Droid;
	public GameObject player;
	public Vector3 chasingPosition;

	//public float movementSpeed = 14.0f;
	public float turnSpeed = 8.0f;

	//public float alertDistance = 18.0f;
	//public float safeDistance = 50.0f;

	public float hoverForce = 90.0f;
	public float hoverHeight = 3.5f;

	private Rigidbody surveillanceDroneRigidbody;

	public bool hasBeenDetected;
	public bool isSpawned; // If TRUE, it is a spawned SD, if FALSE, it is a SD in the map *use for player detection*
	private bool hasCalculatedPlayerPosition;

	public int currentPoint = 0; 

	public float distanceOfPlayer;

	void Awake()
	{
		player = GameObject.FindWithTag("Player");
		surveillanceDroneRigidbody = GetComponent<Rigidbody>();
	}


	void Start()
	{
		float randNum = Random.Range(3,6);
		hoverHeight = randNum;
		currentPoint = SpawnManagerScript.Instance.currentSpawnIndex;
		hasBeenDetected = false;
		hasCalculatedPlayerPosition = false;
	}


	void Update()
	{
		droneStartType();
		surveillanceDroneChaseFunctions();
		surveillanceDroneMainFunctions();

		distanceOfPlayer = Vector3.Distance(transform.position, player.transform.position);
	}


	void FixedUpdate()
	{
		droneHoveringFunction();
	}


	void droneStartType()
	{
		if(hasBeenDetected == false && isSpawned == false) // Previously is if(!hasBeenDeteced) -> Added in !isSpawned to check if it is pregenerated or spawned
		{
			playerDetection();
		}

		if(isSpawned == true)
		{
			//if(Vector3.Distance(transform.position, player.transform.position) <= alertDistance && WaypointManagerScript.Instance.tracePlayerNodes.Count > 0)
			if(Vector3.Distance(transform.position, player.transform.position) <= surveillance_Droid.alertDistance && WaypointManagerScript.Instance.tracePlayerNodes.Count > 0)
			{
				hasBeenDetected = true;
			}

			//if(ReputationManagerScript.Instance.currentRep == 0 && Vector3.Distance(transform.position, player.transform.position) >= safeDistance) // Can possibly be changed to be despawned when out of Player's sight
			if(ReputationManagerScript.Instance.currentRep == 0 && Vector3.Distance(transform.position, player.transform.position) >= surveillance_Droid.safeDistance)
			{
				PoolManagerScript.Instance.Despawn(this.gameObject);
				TimelineScript.Instance.DestroyEnemyIcon(this.gameObject.name, surveillance_Droid.spawnHDAmount);
			}
		}
	}


	void playerDetection()
	{
		//if(Vector3.Distance(transform.position, player.transform.position) <= alertDistance && WaypointManagerScript.Instance.tracePlayerNodes.Count > 0)
		if(Vector3.Distance(transform.position, player.transform.position) <= surveillance_Droid.alertDistance && WaypointManagerScript.Instance.tracePlayerNodes.Count > 0)
		{
			hasBeenDetected = true;

			if(hasCalculatedPlayerPosition == false) // For SDs That Are Already In the Map 
			{
				SpawnManagerScript.Instance.CalculateSpawnPoint();
				currentPoint = SpawnManagerScript.Instance.currentSpawnIndex + 1;

				hasCalculatedPlayerPosition = true;
			}

			// SpawnFunction
			//SpawnManagerScript.Instance.CalculateSpawnPoint();
			//currentPoint = SpawnManagerScript.Instance.currentSpawnIndex + 1;
			SpawnManagerScript.Instance.Spawn("Hunting_Droid");
			//PoolManagerScript.Instance.Spawn("Hunting_Droid",SpawnManagerScript.Instance.spawnPoint,Quaternion.identity);

			if(ReputationManagerScript.Instance.currentRep == 0)
			{
				ReputationManagerScript.Instance.currentRep += 1;
			}
		}
	}


	void surveillanceDroneChaseFunctions()
	{
		if(Vector2.Distance(new Vector2(chasingPosition.x, chasingPosition.z), new Vector2(transform.position.x, transform.position.z)) <= 0.1f)
		{
			if(currentPoint < WaypointManagerScript.Instance.tracePlayerNodes.Count)
			{
				currentPoint++;
			}
		}

		Transform chasingTrans = player.transform;

		if(currentPoint < WaypointManagerScript.Instance.tracePlayerNodes.Count)
		{
			chasingTrans = WaypointManagerScript.Instance.tracePlayerNodes[currentPoint].transform;
		}

		chasingPosition = chasingTrans.position;
		chasingPosition.y = transform.position.y;
	}


	void surveillanceDroneMainFunctions()
	{
		transform.LookAt(chasingPosition);

		if(hasBeenDetected == true)
		{
			//if(Vector3.Distance(transform.position, player.transform.position) >= safeDistance)
			if(Vector3.Distance(transform.position, player.transform.position) >= surveillance_Droid.safeDistance)
			{
				hasBeenDetected = false;

				//Debug.Log("Surveillance Drone No Longer Following Player (More Than safeDistance)");
			}
			else
			{
				//transform.position += transform.forward * movementSpeed * Time.deltaTime;
				transform.position += transform.forward * surveillance_Droid.movementSpeed * Time.deltaTime;
			}
		}
		else
		{
			surveillanceDroneRigidbody.velocity = surveillanceDroneRigidbody.velocity * 0.9f;
		}
	}


	void droneHoveringFunction()
	{
		Ray hoverRay = new Ray (transform.position, -transform.up);
		RaycastHit hoverHit;

		if(Physics.Raycast(hoverRay, out hoverHit, hoverHeight))
		{
			float propotionalHeight = (hoverHeight - hoverHit.distance) / hoverHeight;
			Vector3 appliedHoverForce = Vector3.up * propotionalHeight * hoverForce;
			surveillanceDroneRigidbody.AddForce(appliedHoverForce, ForceMode.Acceleration);
		}
	}
}
