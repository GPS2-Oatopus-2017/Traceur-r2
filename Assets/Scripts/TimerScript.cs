using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
	private static TimerScript mInstance;

	public static TimerScript Instance
	{
		get {return mInstance;}
	}

	public float timeLevel1;
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
		timeLevel1 = totalTimeLevel1;
		timerBar = GetComponentInChildren<Image>();
		timerText.text = StringExtension.ToStringLite(Mathf.RoundToInt(timeLevel1));
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(hasStarted)
		{
			timeLevel1 -= Time.deltaTime;
		}

		if(timeLevel1 <= 0)
			timeLevel1 = 0;
		
		timerBar.fillAmount = timeLevel1 / totalTimeLevel1 * 1;
		timerText.text = StringExtension.ToStringLite(Mathf.RoundToInt(timeLevel1));
	}
}
