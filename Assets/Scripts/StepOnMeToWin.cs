using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StepOnMeToWin : MonoBehaviour
{
    public bool isEntered = false;
    public string gameManager;

    void Update()
    {
        if(isEntered)
        {
            DialogueManager.Instance.WinSceneDialogue();

            if(DialogueManager.Instance.winIndex >= DialogueManager.Instance.winDialogue.Count)
            {
                GameObject.FindWithTag(gameManager).GetComponent<ChangeSceneScript>().ChangeScenes(0);
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
