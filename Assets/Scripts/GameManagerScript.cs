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
}
