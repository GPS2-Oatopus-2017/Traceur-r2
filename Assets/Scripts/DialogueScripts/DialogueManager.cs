﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour 
{
	[Header("When this is true, the dialogue system starts (Loading Screen)")]
	public bool startSystem = false;

    [Header("Indices")]
	[Tooltip("BeginningScene Index count")]
	public int bsIndex = 0;
	[Tooltip("First Encounter Index count")]
	public int feIndex = 0;
	[Tooltip("Lose Scene transition dialogue count")]
	public int loseIndex = 0;
	[Tooltip("Win Scene transition dialogue count")]
    public int winIndex = 0;

    [Header("Beginning Scene Settings")]
    public Text beginningText;
	public GameObject beginningDialogue;
	public GameObject beginningPressAnyKeyBox;
    public float speed;
    public Transform target;
    [Range(0.0f, 1.0f)]
    public float delayBetweenLetters;

    [Header("Text Settings")]
    public Text dialogue;   
    public Text countDown;
    public List<string> beginningScene; // List of dialogues used at the beginning of the game.
    public List<string> firstEncounter; // List of dialogues used for every FIRST encounter with an object.
    public List<string> loseDialogue;
    public List<string> winDialogue;

	[Header("Game Objects")]
	public GameObject dialogueBox;
	public GameObject pressAnyKeyBox;
    public GameObject[] popUps; // An array of GameObjects for pop-up UIs (i.e HealthBar, Time-line).

    [Header("SoundList")]
	public AudioClipID levelBGM;
    public List<AudioClipID> beginningSound;
    public List<AudioClipID> firstEncounterSound;
    public List<AudioClipID> winSound;
    public List<AudioClipID> loseSound;
    public AudioClipID countDownSound;

    [Header("Timers")]
    public float timer; 
    public float showTimer;
    private float cdTimer; 
    public float setTime;
    private int curTimeCount = 5;
    private float b_Timer;
    public float setB_Timer;
    public float continueTimer; // Timer for win/lose dialogue;

    [Header("Booleans")]
    public bool[] objectSeen;
    public bool initTimer, initDialogue, startCD, youWin, youLose, canPress, canContinue;
	public bool checkFirstEncounter;
	public bool overrideBS;

	[Header("Last Minute Stuff")]
	public TimerScript timerScript;
	private bool sceneReady = false;

    public static DialogueManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        cdTimer = setTime;
        timer = showTimer; // Set all First Encounter timers to desired set amount.
        b_Timer = setB_Timer;

        for(int i = 0; i < objectSeen.Length; i++) // Have player ever seen these objects? No. So all booleans are set to false;
        {
            objectSeen[i] = false;
        }

        initTimer = false;      //Initiate timer is set to false.
        initDialogue = false;   //Initial dialogue once scene is loaded is only played once.
        startCD = false;        //Timer for CountDownTimer is set to false.
        youWin = false;
        youLose = false;
		canPress = false;
		beginningPressAnyKeyBox.SetActive(canPress);
		canContinue = false;
		pressAnyKeyBox.SetActive(canContinue);
		checkFirstEncounter = false;

		GameManagerScript.Instance.player.StopRunning();

		sceneReady = true;
    }

    void Update()
    {
		if(!startSystem)
		{
			if(Input.GetKeyDown(KeyCode.Return))
				startSystem = true;
			return;
		}

		//Phase 1 - Beginning Dialogue
		if(!initDialogue) //Displays Beginning Dialogue once
        {
            BeginningSceneDialogue();
		}

		//Phase 2 - Countdown Timer
		if(startCD) // Starts counting down UI after beginning dialogue is completed.
		{
			cdTimer -= Time.deltaTime;
			if(cdTimer <= curTimeCount)
			{
				CountDownTimer();
				curTimeCount--;
			}
		}

		//Phase 3 - FirstEncounter Dialogue
		if(checkFirstEncounter)
		{
			FirstEncounterDialogue(); 

			if(initTimer) //Timer only starts counting down after player sees an object for the first time
			{
				timer -= Time.deltaTime;

				if(timer <= 0) //Disable dialogue box after timer expires
				{
					dialogueBox.SetActive(false);
					timer = showTimer;
					initTimer = false;
				}
			}
		}
    }

    void CountDownTimer() //Countdown UI
    {
        //Amanda's countdown 
        switch(curTimeCount)
        {
            case 5:
                countDown.text = "T";
                popUps[2].SetActive(true);
                SoundManagerScript.Instance.PlaySFX2D(countDownSound, false);
                break;
            case 3:
                countDown.text = "3";
                break;
            case 2:
                countDown.text = "2";
                break;
            case 1:
                countDown.text = "1";
                break;
            case 0:
				startCD = false;
				popUps[0].SetActive(true);
				popUps[1].SetActive(true);
                popUps[2].SetActive(false);
                cdTimer = 0;

                GameManagerScript.Instance.player.StartRunning();
				SoundManagerScript.Instance.PlayBGM(levelBGM, false);
                timerScript.hasStarted = true;
				startCD = false;
				checkFirstEncounter = true;
                break;
        }
    }

    public void BeginningSceneDialogue() // Displays Dialogue in the beginning of the game 
    {
		if(overrideBS || MenuSettings.Instance.skipBS)
		{
			beginningDialogue.SetActive(false);

			initDialogue = true; //Set initialDialogue boolean to true.
			startCD = true; //Start counting-down.
		}
		else
		{
			if(bsIndex < beginningScene.Count)
	        {
	            beginningDialogue.SetActive(true);

	            if(sceneReady)
	            {
	                sceneReady = false;

	                StartCoroutine("TypeEffect"); // Start typing effect.
	                SoundManagerScript.Instance.PlaySFX2D(beginningSound[bsIndex], false);
	                return;
	            }
	            if(((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) && canPress == true || Input.GetMouseButtonDown(0)) && bsIndex <= beginningScene.Count && canPress == true)
	            {
	                SoundManagerScript.Instance.StopSFX2D(beginningSound[bsIndex]);
	                beginningText.text = " ";
	                StopCoroutine("TypeEffect"); // Stop ongoing coroutine.
	                bsIndex++; // Next line of dialogue.
					canPress = false;
					beginningPressAnyKeyBox.SetActive(canPress);
	                if(bsIndex < beginningScene.Count)
	                {
	                    StartCoroutine("TypeEffect"); // Start typing effect.
	                    SoundManagerScript.Instance.PlaySFX2D(beginningSound[bsIndex], false);
	                }
	            }
	        }
	        else //Once beginning dialogue is done, :
	        {
	            StopCoroutine("TypeEffect"); // Stop ongoing coroutine.

	            b_Timer -= Time.deltaTime;

	            if(b_Timer > 0)
	            {
	                beginningDialogue.transform.position = Vector3.MoveTowards(beginningDialogue.transform.position, target.position, speed * Time.deltaTime);
	            }
	            else
	            {
	                beginningDialogue.SetActive(false);

	                initDialogue = true; //Set initialDialogue boolean to true.
	                startCD = true; //Start counting-down.
	                MenuSettings.Instance.skipBS = true;
	            }
	        }
		}
    }

    public void WinSceneDialogue() // Call this function at win scene, may not be nessecary if we transition to win scene.
    {                              // DialogueManager.Instance.WinSceneDialogue();\
        if(youWin == false)
        {
            int randomWinSoundList = Random.Range(0, winSound.Count);

            SoundManagerScript.Instance.StopSFX2D(winSound[randomWinSoundList]);

            dialogueBox.SetActive(true);
            dialogue.text = winDialogue[randomWinSoundList];

            SoundManagerScript.Instance.PlayOneShotSFX2D(winSound[randomWinSoundList]);

            StartCoroutine(ContinueBool());

            youWin = true;
			timerScript.hasStarted = false;
        }
    }

    public void LoseSceneDialogue() // Call this function when player loses the game either by dying or by failing to reach end point in time, may not be nessecary if we transition to lose scene.
    {                               // DialogueManager.Instance.LoseSceneDialogue();
		if(youLose == false)
        {
           // Time.timeScale = 0;

            int randomLoseSoundList = Random.Range(0, loseSound.Count);

            SoundManagerScript.Instance.StopSFX2D(loseSound[randomLoseSoundList]);

            dialogueBox.SetActive(true);
            dialogue.text = loseDialogue[randomLoseSoundList];

            SoundManagerScript.Instance.PlayOneShotSFX2D(loseSound[randomLoseSoundList]);

            StartCoroutine(ContinueBool());

            youLose = true;
			timerScript.hasStarted = false;
        }
    }

    public void FirstEncounterDialogue() // Displays a brief run-down of the objects during player's first encounter with it.
    {
        for(int i = 0; i < objectSeen.Length; i++)
        {
            if(FirstEncounterScript.Instance.seenObj[i] == true && objectSeen[i] == false) //seenObj = Is player currently looking at an object?
            {
                SoundManagerScript.Instance.PlayOneShotSFX2D(firstEncounterSound[feIndex + i]); // Play dialogue for seen obj.

                dialogueBox.SetActive(true); // Enable dialogue box.

                dialogue.text = firstEncounter[feIndex + i]; //Displays text based on which object it is (feIndex);
                initTimer = true;  

                objectSeen[i] = true; // Mark particular object as seen.sable touch-to-continue text.
            }
        }
    }

    IEnumerator TypeEffect() // Simulate typing effect
    {
        for(int i = 1; i <= beginningScene[bsIndex].Length; i++)
        {
            beginningText.text = beginningScene[bsIndex].Substring(0, i); // Show next Text.

            yield return new WaitForSeconds(delayBetweenLetters); // Delay between each text.

            if(i >= beginningScene[bsIndex].Length) //Check if all letters are already typed
            {
                canPress = true;
				beginningPressAnyKeyBox.SetActive(canPress);
            }
        }
    }

    IEnumerator ContinueBool()
    {
        yield return new WaitForSeconds(continueTimer);
        canContinue = true;
		pressAnyKeyBox.SetActive(canContinue);
    }
}
