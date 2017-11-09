using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntingDroneScript : MonoBehaviour {

	public EnemyData hunting_Drone;
	public GameObject player;
	public GameObject enemyAttackIndicator;
	public Vector3 chasingPosition;
	private Vector3 target;
	public float targetOffset;
	public float hoverForce = 90.0f;
	public float hoverHeight = 3.5f;
	private Rigidbody huntingDroneRigidbody;
	public GameObject bullet;
	public Transform droneGunHardPointUp;
	public Transform droneGunHardPointDown;
	public float fireIndication = 0.5f;
	private float nextFire;
	private int randUpDown;

	public bool isIndicated = false;
	public bool isWithinRange;

	public int currentPoint = 0;

	//public float distanceOfPlayer;


	void Awake()
	{
		player = GameObject.FindWithTag("Player");
		huntingDroneRigidbody = GetComponent<Rigidbody>();
	}


	void Start()
	{
		SpawnManagerScript.Instance.CalculateSpawnPoint();
		currentPoint = SpawnManagerScript.Instance.currentSpawnIndex + 1;
		target = player.transform.position + player.transform.forward * targetOffset;

		float randNum = Random.Range(3,6);
		hoverHeight = randNum;

		nextFire = hunting_Drone.attackSpeed;
	}


	void Update()
	{
		huntngDroneChaseFunctions();
		huntingDroneMainFunctions();

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


	void huntngDroneChaseFunctions()
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


	void huntingDroneMainFunctions()
	{
		nextFire -= Time.deltaTime;
		transform.LookAt(chasingPosition);

		if(Vector3.Distance(transform.position, player.transform.position) >= hunting_Drone.safeDistance)
		{
			isWithinRange = false;
			huntingDroneRigidbody.velocity = huntingDroneRigidbody.velocity * 0.9f;

			//Debug.Log("Hunting Drone No Longer Chasing Player (More Than safeDistance)");
		}
		else
		{
			isWithinRange = true;
			transform.position += transform.forward * hunting_Drone.movementSpeed * Time.deltaTime;
		}

		if(isWithinRange == true)
		{
			if(nextFire <= fireIndication)
			{
				if(!isIndicated)
				{
					isIndicated = true;

					randUpDown = Random.Range(0,2);

					if(randUpDown == 0)
					{
						target = droneGunHardPointUp.position + (player.transform.forward * targetOffset);
						GameObject atkIndicator = Instantiate(enemyAttackIndicator, target, droneGunHardPointUp.rotation);
					}
					else 
					{
						target = droneGunHardPointDown.position + (player.transform.forward * targetOffset);
						GameObject atkIndicator = Instantiate(enemyAttackIndicator, target, droneGunHardPointDown.rotation);
					}
				}
			}

			if(nextFire <= 0)
			{
				nextFire = hunting_Drone.attackSpeed;

				if(randUpDown == 0)
				{
					GameObject newBullet = Instantiate(bullet, droneGunHardPointUp.position, droneGunHardPointUp.rotation); // Shoot from TOP HARDPOINT
					BulletScript bulletScript = newBullet.GetComponent<BulletScript>();
					bulletScript.fromTopHardpoint = true;
					newBullet = null;
				}
				else
				{
					GameObject newBullet = Instantiate(bullet, droneGunHardPointDown.position, droneGunHardPointDown.rotation); // Shoot from BOTTOM HARDPOINT
					BulletScript bulletScript = newBullet.GetComponent<BulletScript>();
					bulletScript.fromTopHardpoint = false;
					newBullet = null;
				}
				isIndicated = false;
			}
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
			huntingDroneRigidbody.AddForce(appliedHoverForce, ForceMode.Acceleration);
		}
	}
}
