using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreScript : MonoBehaviour {

	private static PlayerScoreScript mInstance;
	public static PlayerScoreScript Instance
	{
		get { return mInstance; }
	}
	
	public GameObject curWaypoint,playerCollider;
	Vector3 waypointCenter, playerCenter;
	float swipeLocation, time, health, reputation,finalScore;

	public List<float> waypointScore = new List<float>();

	void Awake()
	{
		if(mInstance == null) mInstance = this;
		else if(mInstance != this) Destroy(this.gameObject);
	}


	// Use this for initialization
	void Start () 
	{
		FindPlayer();

	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void FindPlayer()
	{
		playerCollider = gameObject;

	}

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.tag == "Waypoint")
		{
			curWaypoint = col.gameObject;
		}
	}

	void OnTriggerStay (Collider col)
	{
		if (col.gameObject.tag == "Waypoint")
		{
			waypointCenter = curWaypoint.transform.position;
			playerCenter = gameObject.transform.position;
		}
	}

	public void calculateDistance()
	{
		swipeLocation = Vector3.Distance(waypointCenter,playerCenter);

		inputScore(swipeLocation);
		swipeLocation = 0;
	}

	void inputScore(float score)
	{
		waypointScore.Add(score);
	}

	public float calculateFinalScore()
	{
		time = GameManagerScript.Instance.totalTimeLevel1;
		health = PlayerStatusScript.Instance.currentHealth;
		reputation = SpawnManagerScript.Instance.reputation;

		finalScore = time * health * reputation; 

		for (int i = 0; i < waypointScore.Count;i++)
		{
			if (waypointScore[i] > 4)
			{
				finalScore += 20;
			}
			else if (waypointScore[i] < 4 && waypointScore[i] > 1)
			{
				finalScore += 30;
			}
			else if (waypointScore[i] <= 1)
			{
				finalScore += 50;
			}
		}

		return finalScore;
	}
}
