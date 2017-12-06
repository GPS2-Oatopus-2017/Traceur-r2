using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionSensorScript : MonoBehaviour, Iinteractable 
{
	public enum MotionDetectorState
	{
		Normal,
		Alerted,
		Deactivated
	}

	public void Interacted()
	{
		if(isActive == true)
		{
			SoundManagerScript.Instance.StopSFX3D(AudioClipID.SFX_MD_BEEP, this.gameObject);
			SoundManagerScript.Instance.PlayOneShotSFX3D(AudioClipID.SFX_MD_DEACTIVATED, this.gameObject);
			isActive = false;
			isTapped = true;
			motionDetectorCurrentMaterial.material  = deactivatedState;
			lRend.colorGradient = deactivateColor;
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

	//for light change
	private LineRenderer lRend;
	public Gradient normalColor;
	public Gradient alertColor;
	public Gradient deactivateColor;

	void Awake()
	{
		//player = GameManagerScript.Instance.player;
	}


	void Start() 
	{
		player = GameManagerScript.Instance.player;
		isActive = true;
		isTapped = false;
		lRend = GetComponent<LineRenderer>();
		motionDetectorCurrentMaterial = GetComponent<Renderer>();
		motionDetectorCurrentMaterial.material = normalState;
		SoundManagerScript.Instance.PlaySFX3D(AudioClipID.SFX_MD_BEEP, gameObject, true);
		lRend.colorGradient = normalColor; //light 
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
				lRend.colorGradient = alertColor;
                if(ReputationManagerScript.Instance.currentRep <= 0 )
                {
                   	ReputationManagerScript.Instance.currentRep = 1;
				}
				ReputationManagerScript.Instance.resetCounter = 0;

                SpawnManagerScript.Instance.CalculateSpawnPoint();
                SpawnManagerScript.Instance.SpawnMultiple("Hunting_Droid",motionSensor_Data.spawnHDCount);
				SoundManagerScript.Instance.PlayOneShotSFX3D(AudioClipID.SFX_MD_ACIVATED, gameObject);
            }
        }
	}
}
