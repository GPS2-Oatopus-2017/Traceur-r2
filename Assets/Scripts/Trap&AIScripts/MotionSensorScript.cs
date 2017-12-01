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
			isTapped = true;
			motionDetectorCurrentMaterial.material  = deactivatedState;
		}
	}

	public TrapData motionSensor_Data;

	public PlayerCoreController player;
	public bool isActive;
	public bool isTapped;

	//For Colour Material Changes
	public Renderer motionDetectorCurrentMaterial;
	public Material normalState;
	public Material activatedState;
	public Material deactivatedState;

	//public float distanceOfPlayer;


	void Awake()
	{
		player = GameManagerScript.Instance.player;
	}


	void Start() 
	{
		isActive = true;
		isTapped = false;

		motionDetectorCurrentMaterial = GetComponent<Renderer>();
		motionDetectorCurrentMaterial.material = normalState;
		SoundManagerScript.Instance.PlaySFX3D(AudioClipID.SFX_MD_BEEP, gameObject, true);
	}


	void Update()
	{
		motionDetectorMainFunctions();

		//distanceOfPlayer = Vector3.Distance(transform.position, player.transform.position);
	}


	void motionDetectorMainFunctions()
	{
		if(isActive == true)
        {
			if(Vector3.Distance(transform.position, player.transform.position) <= motionSensor_Data.alertDistance && isActive == true && isTapped == false)
            {
                isActive = false;
				motionDetectorCurrentMaterial.material  = activatedState;

                if(ReputationManagerScript.Instance.currentRep == 0)
                {
                    ReputationManagerScript.Instance.currentRep += 1;
                }

                SpawnManagerScript.Instance.CalculateSpawnPoint();
                SpawnManagerScript.Instance.SpawnMultiple("Hunting_Droid",motionSensor_Data.spawnHDCount);
				SoundManagerScript.Instance.PlayOneShotSFX3D(AudioClipID.SFX_MD_ACIVATED, gameObject);
            }
        }
	}
}
