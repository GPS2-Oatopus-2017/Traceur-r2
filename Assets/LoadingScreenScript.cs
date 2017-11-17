using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenScript : MonoBehaviour
{
	private Animator anim;

	public GameObject textGO;

	public bool loadFinished;
	public bool screenPressed;
	public string sceneName;

	void Awake ()
	{
		anim = GetComponent<Animator>();
		loadFinished = false;
		StartCoroutine(LoadAsync());
	}

	IEnumerator LoadAsync()
	{
		AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

		while (!async.isDone)
		{
			yield return null;
		}

		screenPressed = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		anim.SetBool("LoadFinished", loadFinished);
		if(screenPressed)
		{
			if(Input.GetMouseButtonDown(0))
			{
				loadFinished = true;
				DialogueManager.Instance.startSystem = true;
			}

			textGO.SetActive(!loadFinished);
		}
	}

	public void Unload()
	{
		SceneManager.UnloadSceneAsync("LoadingScreen");
	}
}
