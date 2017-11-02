using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionSensorScript : MonoBehaviour {

	public TrapData motionSensor_Data;

	public GameObject player;
	public bool isActive;
	public Material mat;
	//public float alertDistance = 12;
	public float distanceOfPlayer;

	void Awake()
	{
		player = GameObject.FindWithTag("Player");
	}


	void Start() 
	{
		isActive = true;
	}


	void Update()
	{
		checkIfTapped();
		motionDetectorMainFunctions();

		distanceOfPlayer = Vector3.Distance(transform.position, player.transform.position);
	}


	void checkIfTapped()
	{
		if(isActive == true)
		{
			if((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
			{
				Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
				RaycastHit raycastHit;

				if(Physics.Raycast(raycast, out raycastHit))
				{
					if(raycastHit.collider.CompareTag("TrapTag"))
					{
						isActive = false;
					}
				}
			}

			//* For Testing Only -- Mouse Input
			if(Input.GetMouseButtonDown(0))
			{
				Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit raycastHit;

				if(Physics.Raycast(raycast, out raycastHit))
				{
					if(raycastHit.collider.CompareTag("TrapTag"))
					{
						isActive = false;
					}
				}
			}
			//* To Be Deleted When Testing Is Completed
		}
	}


	void motionDetectorMainFunctions()
	{
		if(Vector3.Distance(transform.position, player.transform.position) <= motionSensor_Data.alertDistance && isActive == true)
		{
			isActive = false;

			if(ReputationManagerScript.Instance.currentRep == 0)
			{
				ReputationManagerScript.Instance.currentRep += 1;
			}

			SpawnManagerScript.Instance.CalculateSpawnPoint();
			SpawnManagerScript.Instance.SpawnMultiple("Hunting_Droid",motionSensor_Data.spawnHDCount);
			//PoolManagerScript.Instance.SpawnMuliple("Hunting_Droid",SpawnManagerScript.Instance.spawnPoint,Quaternion.identity,2,0,3.5f,SpawnManagerScript.Instance.isHorizontal);
		}
		else if(isActive == false)
		{
			GetComponent<MeshRenderer>().material = mat;
		}
	}
}
