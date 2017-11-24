using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour {

	private static TimerScript mInstance;

	public static TimerScript Instance
	{
		get {return mInstance;}
	}

	public float totalTimeLevel1;
	public Image timerBar;
	public bool hasStarted = false;
	public Text timerText;

	void Awake ()
	{
		if(mInstance == null) mInstance = this;
		else if(mInstance != this) Destroy(this.gameObject);
	}

	// Use this for initialization
	void Start () 
	{
		totalTimeLevel1 = GameManagerScript.Instance.totalTimeLevel1;
		timerBar = GetComponentInChildren<Image>();
		timerText.text = totalTimeLevel1.ToString();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(hasStarted)
		{
			totalTimeLevel1 -= Time.deltaTime;
		}
		timerBar.fillAmount = totalTimeLevel1 / GameManagerScript.Instance.totalTimeLevel1 * 1;
		float timer = Mathf.Round(totalTimeLevel1);
		timerText.text = timer.ToString();
	}
}
