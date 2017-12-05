using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StepOnMeToWin : MonoBehaviour
{
    private bool isEntered = false;

    void Update()
    {
        if(isEntered)
		{
			GameManagerScript.Instance.player.StopRunning();
			GameManagerScript.Instance.player.hasWon = true;
			DialogueManager.Instance.WinSceneDialogue();
			ScoreManagerScript.Instance.MarkFinalScore();

            if(DialogueManager.Instance.canContinue)
            {
                if(((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0)))
                {
                    Time.timeScale = 1.0f;
					SceneDataHolder.Instance.nextScene = GameManagerScript.Instance.winScene;
					ChangeSceneScript css = GameObject.FindWithTag("GameController").GetComponent<ChangeSceneScript>();
					css.ChangeScenes(0);
                    MenuSettings.Instance.skipBS = false;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            isEntered = true;
        }
    }
}
