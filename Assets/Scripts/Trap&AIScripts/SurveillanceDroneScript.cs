using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveillanceDroneScript : MonoBehaviour {

	public EnemyData surveillance_Droid;
	public PlayerCoreController player;
	public Vector3 chasingPosition;
	public float turnSpeed = 8.0f;
	public float hoverForce = 90.0f;
	public float hoverHeight = 3.5f;

	private Rigidbody surveillanceDroneRigidbody;

	public bool hasBeenDetected;
	public bool isSpawned; // If TRUE, it is a spawned SD, if FALSE, it is a SD in the map - *Used for player detection*
	private bool hasCalculatedPlayerPosition;

	public int currentPoint = 0; 
	private bool slowDown;

	//variable for walking separately
	public float offset;
	public bool isHorizontal;
	//public float distanceOfPlayer;

	void OnEnable()
	{
		if(SpawnManagerScript.Instance.isHorizontal == true)
		{
			offset = transform.position.z - SpawnManagerScript.Instance.spawnPoint.z;
			isHorizontal = true;
		}
		else if(SpawnManagerScript.Instance.isHorizontal == false)
		{
			offset = transform.position.x - SpawnManagerScript.Instance.spawnPoint.x;
			isHorizontal = false;
		}
	}

	void Awake()
	{
		player = GameManagerScript.Instance.player;
		surveillanceDroneRigidbody = GetComponent<Rigidbody>();
	}


	void Start()
	{
		float randNum = Random.Range(3,6);
		hoverHeight = randNum;
		currentPoint = SpawnManagerScript.Instance.currentSpawnIndex;
		hasBeenDetected = false;
		hasCalculatedPlayerPosition = false;
		slowDown = false;
	}


	void Update()
	{
		droneStartType();
		surveillanceDroneChaseFunctions();
		surveillanceDroneMainFunctions();

		if(ReputationManagerScript.Instance.currentRep == 0)
		{
			PoolManagerScript.Instance.Despawn(this.gameObject);
			TimelineScript.Instance.DestroyEnemyIcon(this.gameObject.name, 1);
		}

		//distanceOfPlayer = Vector3.Distance(transform.position, player.transform.position);
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
			if(Vector3.Distance(transform.position, player.transform.position) <= surveillance_Droid.alertDistance && WaypointManagerScript.Instance.tracePlayerNodes.Count > 0)
			{
				hasBeenDetected = true;
			}
				
			if(ReputationManagerScript.Instance.currentRep == 0 && Vector3.Distance(transform.position, player.transform.position) >= surveillance_Droid.safeDistance) // Can possibly be changed to be despawned when out of Player's sight
			{
				PoolManagerScript.Instance.Despawn(this.gameObject);
				TimelineScript.Instance.DestroyEnemyIcon(this.gameObject.name, surveillance_Droid.spawnHDAmount);
			}
		}
	}


	void playerDetection()
	{
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
			SpawnManagerScript.Instance.Spawn("Hunting_Droid");

			if(ReputationManagerScript.Instance.currentRep == 0)
			{
				ReputationManagerScript.Instance.currentRep += 1;
			}
			SoundManagerScript.Instance.PlayOneShotSFX3D(AudioClipID.SFX_DRONE_ALERT, gameObject);
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

		if(isHorizontal)
		{
			chasingPosition.z += offset;
		}
		else if(!isHorizontal)
		{
			chasingPosition.x += offset;
		}
	}


	void surveillanceDroneMainFunctions()
	{
		transform.LookAt(chasingPosition);

		if(hasBeenDetected == true)
		{
			if(Vector3.Distance(transform.position, player.transform.position) <= 2.0f)
			{
				surveillanceDroneRigidbody.velocity = surveillanceDroneRigidbody.velocity * 0.9f;
				slowDown = true;
			}
			else
			{
				slowDown = false;
			}

			if(Vector3.Distance(transform.position, player.transform.position) >= surveillance_Droid.safeDistance)
			{
				hasBeenDetected = false;

				//Debug.Log("Surveillance Drone No Longer Following Player (More Than safeDistance)");
			}
			else
			{
				if(slowDown == false)
				{
					transform.position += transform.forward * surveillance_Droid.movementSpeed * Time.deltaTime;
				}
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
		SoundManagerScript.Instance.PlaySFX3D(AudioClipID.SFX_DRONE_HOVER, gameObject, true);
	}
}
