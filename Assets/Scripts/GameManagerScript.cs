using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
	//Singleton Setup
	private static GameManagerScript mInstance;
	public static GameManagerScript Instance
	{
		get { return mInstance; }
	}

	[Header("Common Used Components")]
	public PlayerCoreController player;

	void Awake()
	{
		//Singleton Setup
		if(mInstance == null) mInstance = this;
		else if(mInstance != this) Destroy(this.gameObject);

		GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
		if(playerGO) player = playerGO.GetComponent<PlayerCoreController>();
		else Debug.LogError("GameManager: Unable to find Player!");
	}

	void Update()
	{
		CheckWinLoseConditions();
	}

	[Header("Level Time")]
	public float totalTimeLevel1 = 90f;

	[Header("WinLoseConditions")]
	public TimerScript timerScript;

	void CheckWinLoseConditions () 
	{
      //  if(PlayerStatusScript.Instance.health > 0) return; //After character health reaches "0" change scene to [LoseScene]
		if(player.status.currentHealth <= 0)
        {
            DialogueManager.Instance.LoseSceneDialogue();

            if(((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0)))
            {
                Time.timeScale = 1.0f;
                GetComponent<ChangeSceneScript>().ChangeScenes(1);
            }
        }
		//else if(timerScript.totalTimeLevel1 > 0) return;// After count-down timer reaches "0" change scene to [LoseScene]
		//else
        if(timerScript.totalTimeLevel1 <= 0)
		{
			DialogueManager.Instance.LoseSceneDialogue();

			player.status.currentHealth = 0;

			if(((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0)))
			{
				Time.timeScale = 1.0f;
				GetComponent<ChangeSceneScript>().ChangeScenes(1);
			}
		}
	}
}
