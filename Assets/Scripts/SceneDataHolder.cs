using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDataHolder : MonoBehaviour
{
	private static SceneDataHolder mInstance;

	public static SceneDataHolder Instance
	{
		get
		{
			if(!mInstance)
			{
				GameObject sceneDataGO = new GameObject();
				sceneDataGO.name = "SceneDataHolder";
				mInstance = sceneDataGO.AddComponent<SceneDataHolder>();
			}
			return mInstance;
		}
	}

	public string nextScene;

	// Use this for initialization
	void Awake () 
	{
		DontDestroyOnLoad(gameObject);
	}
}
