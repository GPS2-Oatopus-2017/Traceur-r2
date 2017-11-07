using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour 
{
    [Header("Variables")]
    public int bsIndex = 0; // BeginningScene Index count.
    public int feIndex = 0; // First Encounter Index count.
    public int loseIndex = 0; // Lose Scene transition dialogue count.
    public int winIndex = 0; // Win Scene transition dialogue count.

    [Header("Beginning Scene Settings")]
    public Image startingDialogue;
    public float fadeTime;
    Color colorToFadeTo;
    public Text beginningText;
    public GameObject beginningDialogue;
    public float speed;
    public Transform target;

    [Range(0.0f, 1.0f)]
    public float nextLetter;

    [Header("Text Settings")]
    public Text dialogue;   
    public Text countDown;
    public List<string> beginningScene; // List of dialogues used at the beginning of the game.
    public List<string> firstEncounter; // List of dialogues used for every FIRST encounter with an object.
    public List<string> loseDialogue;
    public List<string> winDialogue;

    [Header("Game Objects")]
    public GameObject dialogueBox;
    public GameObject ttcText; // ttc = touch-to-continue
    public GameObject[] popUps; // An array of GameObjects for pop-up UIs (i.e HealthBar, Time-line).

    [Header("SoundList")]

    [Header("Timers")]
    public float timer; 
    public float showTimer;
    public float cdTimer; 
    public float setTime;

    [Header("Booleans")]
    public bool[] objectSeen;
    public bool initTimer, initDialogue, startCD;

	[Header("Last Minute Stuff")]
	public TimerScript timerScript;

    public static DialogueManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartCoroutine("TypeEffect");

        cdTimer = setTime;
        timer = showTimer; // Set all First Encounter timers to desired set amount.

//        for(int i = 0; i < timer.Length; i++)
//        {
//            timer[i] = showTimer; // Set all First Encounter timers to desired set amount.
//        }

        for(int i = 0; i < objectSeen.Length; i++) // Have player ever seen these objects? No. So all booleans are set to false;
        {
            objectSeen[i] = false;
        }

        initTimer = false;      //Initiate timer is set to false.
        initDialogue = false;   //Initial dialogue once scene is loaded is only played once.
        startCD = false;        //Timer for CountDownTimer is set to false.

		GameManagerScript.Instance.player.StopRunning();
		SoundManagerScript.Instance.StopBGM(AudioClipID.BGM_MAIN_MENU);
    }

    void Update()
    {
        if(initTimer == true) //Timer only starts counting down after player sees an object for the first time
        {
            timer -= Time.deltaTime;
        }

        if(startCD == true) // Starts counting down UI after beginning dialogue is completed.
        {
            cdTimer -= Time.deltaTime;
            CountDownTimer();
        }

        if(timer <= 0) //Disable dialogue box after timer expires
        {
            dialogueBox.SetActive(false);
            timer = showTimer;
            initTimer = false;
        }

        if(!initDialogue) //Displays Beginning Dialogue once
        {
            BeginningSceneDialogue();
        }
            
        FirstEncounterDialogue(); 
    }

    public void BeginningSceneDialogue() // Displays Dialogue in the beginning of the game 
    {
        beginningDialogue.SetActive(true);

        if(((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0)) && bsIndex <= beginningScene.Count )
        {
            beginningText.text = " ";
            StopCoroutine("TypeEffect"); // Stop ongoing coroutine.
            beginningScene.RemoveAt(bsIndex);
            StartCoroutine("TypeEffect"); // Start typing effect.
        }

        if(bsIndex >= beginningScene.Count) //Once beginning dialogue is done, :
        {
            /*colorToFadeTo = new Color(1f, 1f, 1f, 0f);
            startingDialogue.CrossFadeColor(colorToFadeTo, fadeTime, true, true);
            fadeTime -= Time.deltaTime;*/

            beginningDialogue.transform.position = Vector3.MoveTowards(beginningDialogue.transform.position, target.position, speed * Time.deltaTime);

            if(beginningDialogue.transform.position.y == target.transform.position.y) // After beginning scene has faded, start the game.
            {
                beginningDialogue.SetActive(false);

                initDialogue = true; //Set initialDialogue boolean to true.
                startCD = true; //Start counting-down.
            }
        }
    }

    public void WinSceneDialogue() // Call this function at win scene, may not be nessecary if we transition to win scene.
    {                              // DialogueManager.Instance.WinSceneDialogue();
        Time.timeScale = 0;
        dialogueBox.SetActive(true);
        dialogue.text = winDialogue[winIndex];

        if(((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0)) && winIndex <= winDialogue.Count )
        {
            winIndex++;
        }
    }

    public void LoseSceneDialogue() // Call this function when player loses the game either by dying or by failing to reach end point in time, may not be nessecary if we transition to lose scene.
    {                               // DialogueManager.Instance.LoseSceneDialogue();
        Time.timeScale = 0;
        dialogueBox.SetActive(true);
        dialogue.text = loseDialogue[loseIndex];

        if(((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0)) && loseIndex <= loseDialogue.Count )
        {
            loseIndex++;
        }
    }

    public void FirstEncounterDialogue() // Displays a brief run-down of the objects during player's first encounter with it.
    {

        for(int i = 0; i < objectSeen.Length; i++)
        {
            if(FirstEncounterScript.Instance.seenObj[i] == true && objectSeen[i] == false) //seenObj = Is player currently looking at an object?
            {
                dialogueBox.SetActive(true); // Enable dialogue box.
                dialogue.text = firstEncounter[feIndex + i]; //Displays text based on which enemy it is (feIndex);
                initTimer = true;  

                objectSeen[i] = true; // Mark particular object as seen.
                ttcText.SetActive(false); // Disable touch-to-continue text.
            }
        }
    }

    void CountDownTimer() //Countdown UI
    {
        popUps[2].SetActive(true);
   
        if(cdTimer <= 4 && cdTimer > 3)
            countDown.text = "!";
        else if(cdTimer <= 3 && cdTimer > 2)
            countDown.text = "3";
        else if(cdTimer <= 2 && cdTimer > 1)
            countDown.text = "2";
        else if(cdTimer <= 1 && cdTimer > 0)
            countDown.text = "1";
        else if(cdTimer <= 0)
        {
            popUps[2].SetActive(false);
            startCD = false;
            cdTimer = 0;

			GameManagerScript.Instance.player.StartRunning();
			SoundManagerScript.Instance.PlayBGM(AudioClipID.BGM_TUTO);
			timerScript.hasStarted = true;
        }
    }

    IEnumerator TypeEffect() // Simulate typing effect
    {
        foreach(char letter in beginningScene[bsIndex].ToCharArray()) // Get each character from the dialogues written in Inspector.
        {
            //Can put sound effect for text typing here


            beginningText.text += letter; // Show next Text.

            yield return new WaitForSeconds(nextLetter); // Delay between each text.
        }
    }
}
