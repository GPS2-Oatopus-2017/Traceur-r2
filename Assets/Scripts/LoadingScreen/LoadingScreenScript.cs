using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenScript : MonoBehaviour
{
	private Animator anim;

	public GameObject textGO;

	public bool screenPressed;
	public bool loadFinished;

	void Awake ()
	{
		anim = GetComponent<Animator>();
		screenPressed = false;
		StartCoroutine(LoadAsync());
	}

	IEnumerator LoadAsync()
	{
		AsyncOperation async = SceneManager.LoadSceneAsync(SceneDataHolder.Instance.nextScene, LoadSceneMode.Additive);

		while (!async.isDone)
		{
			yield return null;
		}

		loadFinished = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		anim.SetBool("LoadFinished", screenPressed);
		if(loadFinished)
		{
			if(Input.GetMouseButtonDown(0))
			{
				screenPressed = true;
				DialogueManager.Instance.startSystem = true;
			}

			textGO.SetActive(!screenPressed);
		}
	}

	public void Unload()
	{
		SceneManager.UnloadSceneAsync("LoadingScreen");
	}
}
