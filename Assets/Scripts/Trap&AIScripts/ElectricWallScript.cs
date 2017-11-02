using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricWallScript : MonoBehaviour 
{
    [Header("Electric Wall Settings")]
	public TrapData electricFence_Data;
  //  public float slowDuration;
    public float slowTimer;
    public float speedReducedValue;
    public GameObject player;
    public bool playerIsSlowed;

	void Start() 
	{
		player = GameObject.FindWithTag("Player");
      //  slowTimer = slowDuration; // Set Countdown timer to the duration player is slowed
		slowTimer = electricFence_Data.slowDuration;
        playerIsSlowed = false; // Boolean indicating if player is slowed
	}


	void Update() 
	{
        if(playerIsSlowed) // If player is slowed, do the following
        {
            slowTimer -= Time.deltaTime; // Countdown timer
            player.GetComponent<PlayerCoreController>().ToggleRunning(false); //Player is slowed

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
		if(other.gameObject.tag == "Player")
		{
			playerIsSlowed = true;

			Debug.Log("Player speed is reduced by " + speedReducedValue);
		}

		if(other.gameObject.tag == "Enemy")
		{
			PoolManagerScript.Instance.Despawn(other.gameObject);

			if(other.gameObject.GetComponent<SurveillanceDroneScript>())
			{
				ReputationManagerScript.Instance.deadSD++;
			}
			else if(other.gameObject.GetComponent<HuntingDroneScript>())
			{
				ReputationManagerScript.Instance.deadHD++;
			}

			Debug.Log("enemy despwan");
		}
    }
}
