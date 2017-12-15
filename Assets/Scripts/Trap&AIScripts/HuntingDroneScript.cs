using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntingDroneScript : MonoBehaviour {

	public EnemyData hunting_Drone;
	public PlayerCoreController player;
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
	private bool shootTop = false;
	private bool slowDown;

	public bool isIndicated = false;
	public bool isWithinRange;

	public int currentPoint = 0;

	//variable for walking separately
	public float offset;
	public bool isHorizontal;
	//public float distanceOfPlayer;

	private Transform aiTrans;


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
		huntingDroneRigidbody = GetComponent<Rigidbody>();
	}


	void Start()
	{
		SpawnManagerScript.Instance.CalculateSpawnPoint();
		currentPoint = SpawnManagerScript.Instance.currentSpawnIndex + 1;
		targetOffset = hunting_Drone.keptDistance + hunting_Drone.atkIndicatorOffset;
		target = player.transform.position + player.transform.forward * targetOffset;
		hoverHeight = 4;
		nextFire = hunting_Drone.attackSpeed;
		slowDown = false;
	}


	void Update()
	{
		if(!GameManagerScript.Instance.player.hasWon)
		{
			huntngDroneChaseFunctions();
			huntingDroneMainFunctions();
			if(!GameManagerScript.Instance.player.status.isAlive)
			{
//				transform.Translate(player.transform.position * Time.deltaTime * hunting_Drone.movementSpeed);
//				transform.Translate(Vector3.forward * Time.deltaTime * hunting_Drone.movementSpeed);
				transform.position = Vector3.MoveTowards(transform.position, player.killerPos.position, Time.deltaTime * hunting_Drone.movementSpeed);
			}
			else
			{
				if(ReputationManagerScript.Instance.currentRep == 0)
				{
					PoolManagerScript.Instance.Despawn(this.gameObject);
					TimelineScript.Instance.DestroyEnemyIcon(this.gameObject.name, 1);
				}
			}
		}
		else
		{
			PoolManagerScript.Instance.Despawn(this.gameObject);
			TimelineScript.Instance.DestroyEnemyIcon(this.gameObject.name, 1);
		}

		//distanceOfPlayer = Vector3.Distance(transform.position, player.transform.position);
	}


	void FixedUpdate()
	{
//		droneHoveringFunction();
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

		if(isHorizontal)
		{
			chasingPosition.z += offset;
		}
		else if(!isHorizontal)
		{
			chasingPosition.x += offset;
		}
	}


	void huntingDroneMainFunctions()
	{
		nextFire -= Time.deltaTime;
		transform.LookAt(chasingPosition);

		if(Vector3.Distance(transform.position, player.transform.position) <= hunting_Drone.keptDistance)
		{
			huntingDroneRigidbody.velocity = huntingDroneRigidbody.velocity * 0.9f;
			slowDown = true;
		}
		else
		{
			slowDown = false;
		}

		if(Vector3.Distance(transform.position, player.transform.position) >= hunting_Drone.safeDistance)
		{
			isWithinRange = false;
			huntingDroneRigidbody.velocity = huntingDroneRigidbody.velocity * 0.9f;

			//Debug.Log("Hunting Drone No Longer Chasing Player (More Than safeDistance)");
		}
		else
		{
			isWithinRange = true;

			if(slowDown == false)
			{
				transform.position += transform.forward * hunting_Drone.movementSpeed * Time.deltaTime;
			}
		}

		if(isWithinRange == true)
		{
			if(nextFire <= fireIndication)
			{
				if(!isIndicated)
				{
					isIndicated = true;

					SoundManagerScript.Instance.PlayOneShotSFX3D(AudioClipID.SFX_CHARGE, gameObject);
					shootTop = AIAttackTimingManagerScript.Instance.isTop;

					if(shootTop)
					{
						targetOffset = hunting_Drone.keptDistance + hunting_Drone.atkIndicatorOffset;
						target = droneGunHardPointUp.position + (player.transform.forward * targetOffset);
						if(isHorizontal) target.z = player.transform.position.z;
						else target.x = player.transform.position.x;
						aiTrans = Instantiate(enemyAttackIndicator, target, droneGunHardPointUp.rotation).transform;
//						if(AIAttackTimingManagerScript.Instance.tutorialFirstTime)
//						{
							target.y -= 1.0f;
							Instantiate(AIAttackTimingManagerScript.Instance.indicatorDown, target, droneGunHardPointUp.rotation, aiTrans);
//						}
					}
					else 
					{
						targetOffset = hunting_Drone.keptDistance + hunting_Drone.atkIndicatorOffset;
						target = droneGunHardPointDown.position + (player.transform.forward * targetOffset);
						if(isHorizontal) target.z = player.transform.position.z;
						else target.x = player.transform.position.x;
						aiTrans = Instantiate(enemyAttackIndicator, target, droneGunHardPointDown.rotation).transform;
//						if(AIAttackTimingManagerScript.Instance.tutorialFirstTime)
//						{
							target.y += 1.0f;
							Instantiate(AIAttackTimingManagerScript.Instance.indicatorUp, target, droneGunHardPointUp.rotation, aiTrans);
//						}
					}
				}
			}

			if(nextFire <= 0)
			{
				nextFire = hunting_Drone.attackSpeed;

				if(shootTop)
				{
					GameObject newBullet = Instantiate(bullet, droneGunHardPointUp.position, droneGunHardPointUp.rotation); // Shoot from TOP HARDPOINT
					BulletScript bulletScript = newBullet.GetComponent<BulletScript>();
					bulletScript.fromTopHardpoint = true;
					if(aiTrans) newBullet.transform.LookAt(aiTrans);
				}
				else
				{
					GameObject newBullet = Instantiate(bullet, droneGunHardPointDown.position, droneGunHardPointDown.rotation); // Shoot from BOTTOM HARDPOINT
					BulletScript bulletScript = newBullet.GetComponent<BulletScript>();
					bulletScript.fromTopHardpoint = false;
					if(aiTrans) newBullet.transform.LookAt(aiTrans);
				}

				isIndicated = false;
				SoundManagerScript.Instance.PlayOneShotSFX3D(AudioClipID.SFX_LASER, this.gameObject);
			}
		}
	}


//	void droneHoveringFunction()
//	{
//		Ray hoverRay = new Ray (transform.position, -transform.up);
//		RaycastHit hoverHit;
//
//		if(Physics.Raycast(hoverRay, out hoverHit, hoverHeight))
//		{
//			float propotionalHeight = (hoverHeight - hoverHit.distance) / hoverHeight;
//			Vector3 appliedHoverForce = Vector3.up * propotionalHeight * hoverForce;
//			huntingDroneRigidbody.AddForce(appliedHoverForce, ForceMode.Acceleration);
//		}
//
//		SoundManagerScript.Instance.PlaySFX3D(AudioClipID.SFX_DRONE_HOVER, this.gameObject, true);
//	}
}
