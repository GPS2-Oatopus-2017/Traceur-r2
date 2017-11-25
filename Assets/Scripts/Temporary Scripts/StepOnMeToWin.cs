﻿using System.Collections;
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
            DialogueManager.Instance.WinSceneDialogue();
			ScoreManagerScript.Instance.MarkFinalScore();
            if(((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0)))
            {
                Time.timeScale = 1.0f;
                GameObject.FindWithTag("GameController").GetComponent<ChangeSceneScript>().ChangeScenes(0);
                MenuSettings.Instance.skipBS = false;
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
