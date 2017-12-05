using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreDataHolder : MonoBehaviour
{
	private static ScoreDataHolder mInstance;

	public static ScoreDataHolder Instance
	{
		get
		{
			if(!mInstance)
			{
				GameObject scoreDataGO = new GameObject();
				scoreDataGO.name = "ScoreDataHolder";
				mInstance = scoreDataGO.AddComponent<ScoreDataHolder>();
			}
			return mInstance;
		}
	}

	public float lastScore = 0.0f;

	// Use this for initialization
	void Awake () 
	{
		DontDestroyOnLoad(gameObject);
	}
}
