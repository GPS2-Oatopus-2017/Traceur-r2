using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneScript : MonoBehaviour 
{
    public string[] switchScenes;

    public static ChangeSceneScript Instance;

    void Awake()
    {
        Instance = this;
    }

    public void ChangeScenes(int scene)
    {
        Time.timeScale = 1;
		SceneManager.LoadScene(switchScenes[scene], LoadSceneMode.Single);
    }
}
