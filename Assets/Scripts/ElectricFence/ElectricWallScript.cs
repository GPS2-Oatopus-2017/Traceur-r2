using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricWallScript : MonoBehaviour 
{
	public TrapData electricFence_Data;
    public float slowTimer;
    public float speedReducedValue;
	public PlayerCoreController player;
	public GameObject cylinder;
    public bool playerIsSlowed;
	public bool isActived;
	bool isPlayed;
	public GameObject electricEffectPrefab;

	void Start() 
	{
		player = GameManagerScript.Instance.player;
		cylinder = this.transform.GetChild(1).gameObject;
		isActived = true;
		slowTimer = electricFence_Data.slowDuration; // Set Countdown timer to the duration player is slowed
        playerIsSlowed = false; // Boolean indicating if player is slowed
		isPlayed = false;
	}

	void Update() 
	{
        if(playerIsSlowed) // If player is slowed, do the following
        {
			if(!isPlayed)
			{
				GameManagerScript.Instance.player.animController.PlayShockedAnim();
				isPlayed = true;
			}
            slowTimer -= Time.deltaTime; // Countdown timer

			if(player.GetComponent<PlayerCoreController>() != null)
			{
				player.GetComponent<PlayerCoreController>().ToggleRunning(false); //Player is slowed
			}
				
            if(slowTimer <= 0) // After timer reaches 0, slow debuff will be lifted
            {
               // slowTimer = slowDuration;
				slowTimer = electricFence_Data.slowDuration;
                playerIsSlowed = false;
                player.GetComponent<PlayerCoreController>().ToggleRunning(true);
                Debug.Log("Speed slow debuff is lifted!");
            }
        }
	}


    void OnTriggerEnter(Collider other) //Apply knockback and speed reduction upon collision
	{
		if(isActived)
		{
			if(other.gameObject.tag == "Player")
			{
				playerIsSlowed = true;
				isPlayed = false;
				Transform camTrans = player.rigidController.cam.transform;
				Instantiate(electricEffectPrefab, camTrans.position, camTrans.rotation, camTrans);
				Debug.Log("Player speed is reduced by " + speedReducedValue);
			}

			if(other.gameObject.tag == "Enemy")
			{
				if(other.gameObject.GetComponent<SurveillanceDroneScript>())
				{
					ReputationManagerScript.Instance.deadSD++;
				}
				else if(other.gameObject.GetComponent<HuntingDroneScript>())
				{
					ReputationManagerScript.Instance.deadHD++;
				}

				Debug.Log("Enemy Despawned");
				PoolManagerScript.Instance.Despawn(other.gameObject);
			}
			SoundManagerScript.Instance.PlayOneShotSFX3D(AudioClipID.SFX_FENCE_ACTIVE, cylinder);
		}
    }
}
