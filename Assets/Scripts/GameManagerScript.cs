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
	public ReputationManagerScript repScript;
	public TimerScript timerScript;

	[Header("Level Time")]
	public float totalTimeLevel1;

	void Awake()
	{
		//Singleton Setup
		if(mInstance == null) mInstance = this;
		else if(mInstance != this) Destroy(this.gameObject);

		//Search for existing components
		GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
		if(playerGO) player = playerGO.GetComponent<PlayerCoreController>();
		else Debug.LogError("GameManager: Unable to find Player!");

		GameObject repGO = GameObject.FindGameObjectWithTag("ReputationManager");
		if(repGO) repScript = repGO.GetComponent<ReputationManagerScript>();
		else Debug.LogError("GameManager: Unable to find ReputationSystem!");

		GameObject timerGO = GameObject.FindGameObjectWithTag("TimerScript");
		if(timerGO) timerScript = timerGO.GetComponent<TimerScript>();
		else Debug.LogError("GameManager: Unable to find TimerScript!");

		//Redirections
		timerScript.totalTimeLevel1 = totalTimeLevel1;
	}

	void Update()
	{
		CheckWinLoseConditions();
	}

	void CheckWinLoseConditions () 
	{
		//  if(PlayerStatusScript.Instance.health > 0) return; //After character health reaches "0" change scene to [LoseScene]
		if(player.status.currentHealth <= 0)
		{
			DialogueManager.Instance.LoseSceneDialogue();

            if(DialogueManager.Instance.canContinue)
            {
    			if(((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0)))
    			{
    				Time.timeScale = 1.0f;
    				GetComponent<ChangeSceneScript>().ChangeScenes(1);
    			}
            }
        }
		//else if(timerScript.totalTimeLevel1 > 0) return;// After count-down timer reaches "0" change scene to [LoseScene]
		//else
		if(timerScript.timeLevel1 <= 0)
		{
			DialogueManager.Instance.LoseSceneDialogue();

			player.status.currentHealth = 0;

            if(DialogueManager.Instance.canContinue)
            {
    			if(((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0)))
    			{
    				Time.timeScale = 1.0f;
    				GetComponent<ChangeSceneScript>().ChangeScenes(1);
    			}
            }
		}
	}
}
