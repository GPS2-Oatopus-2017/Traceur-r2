using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepOnMeToWin : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
            DialogueManager.Instance.WinSceneDialogue();

            if(DialogueManager.Instance.winIndex >= DialogueManager.Instance.winDialogue.Count)
            {
                GetComponent<ChangeSceneScript>().ChangeScenes(0);
            }
		}
	}
}
