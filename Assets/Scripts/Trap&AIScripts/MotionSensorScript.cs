using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionSensorScript : MonoBehaviour, Iinteractable 
{
	public void Interacted()
	{
		if(isActive == true)
		{
			isActive = false;
		}

		//Debug.Log("MD Touched!");
	}

	public TrapData motionSensor_Data;

	public GameObject player;
	public bool isActive;
	public Renderer rend;

	//public float distanceOfPlayer;


	void Awake()
	{
		player = GameObject.FindWithTag("Player");
	}


	void Start() 
	{
		isActive = true;

		//Temporary colours until animation or finalised textures are given
		rend = GetComponent<Renderer>();
		rend.material.color = Color.red;
	}


	void Update()
	{
		motionDetectorMainFunctions();

		//distanceOfPlayer = Vector3.Distance(transform.position, player.transform.position);
	}


	void motionDetectorMainFunctions()
	{
		if(isActive ==  true)
        {
            if(Vector3.Distance(transform.position, player.transform.position) <= motionSensor_Data.alertDistance && isActive == true)
            {
                isActive = false;
                rend.material.color = Color.green;

                if(ReputationManagerScript.Instance.currentRep == 0)
                {
                    ReputationManagerScript.Instance.currentRep += 1;
                }

                SpawnManagerScript.Instance.CalculateSpawnPoint();
                SpawnManagerScript.Instance.SpawnMultiple("Hunting_Droid",motionSensor_Data.spawnHDCount);
                //PoolManagerScript.Instance.SpawnMuliple("Hunting_Droid",SpawnManagerScript.Instance.spawnPoint,Quaternion.identity,2,0,3.5f,SpawnManagerScript.Instance.isHorizontal);
            }
        }
	}
}
