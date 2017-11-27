using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManagerScript : MonoBehaviour
{
	private static ScoreManagerScript mInstance;
	public static ScoreManagerScript Instance
	{
		get { return mInstance; }
	}

	private TimerScript timerScript;
	private PlayerStatusScript status;
	private ReputationManagerScript repScript;
	private Transform playerTrans;

	void Awake ()
	{
		if(mInstance == null) mInstance = this;
		else if(mInstance != this) Destroy(this.gameObject);
	}

	void Start()
	{
		timerScript = TimerScript.Instance;
		status = GameManagerScript.Instance.player.status;
		repScript = ReputationManagerScript.Instance;
		playerTrans = GameManagerScript.Instance.player.transform;
	}

	public int totalScore_int = 0;
	private float totalScore = 0.0f;
	[HideInInspector]
	public int finalScore = 0;
	public float precisionScore = 0.0f;

	void Update()
	{
		float time = timerScript.timeLevel1;
		float health = status.currentHealth;
		float reputation = repScript.currentRep;

		totalScore = (time * health * (reputation + 1)) + precisionScore;
		totalScore_int = Mathf.RoundToInt(totalScore);
	}

	public void MarkPrecision()
	{
		Vector3 waypointCenter = WaypointManagerScript.Instance.touchedNodes [0].transform.position;
		Vector3 playerCenter = playerTrans.position;
		float swipeLocation = Vector3.Distance(waypointCenter,playerCenter);

		if (swipeLocation > 4.0f)
		{
			precisionScore += 20.0f;
		}
		else if (swipeLocation < 4.0f && swipeLocation > 1.0f)
		{
			precisionScore += 30.0f;
		}
		else if (swipeLocation <= 1.0f)
		{
			precisionScore += 50.0f;
		}
	}

	public void MarkFinalScore()
	{
		finalScore = totalScore_int;
	}
}
