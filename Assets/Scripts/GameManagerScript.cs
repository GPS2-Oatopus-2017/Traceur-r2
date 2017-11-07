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

	public PlayerCoreController player;
	public float totalTimeLevel1 = 90f;

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

	[Header("WinLoseConditions")]
	public TimerScript timerScript;

	void CheckWinLoseConditions () 
	{
		if(timerScript.totalTimeLevel1 > 0) return;// After count-down timer reaches "0" change scene to [LoseScene]
		if(player.status.health > 0) return; //After character health reaches "0" change scene to [LoseScene]

		DialogueManager.Instance.LoseSceneDialogue();

        if(((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0)))
        {
			GetComponent<ChangeSceneScript>().ChangeScenes(1);
		}
	}
}
