using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreScript : MonoBehaviour {

	private static PlayerScoreScript mInstance;
	public static PlayerScoreScript Instance
	{
		get { return mInstance; }
	}
		
	public GameObject curWaypoint,playerCollider, waypointCollider;
	public Vector3 waypointCenter, playerCenter;
	public float swipeLocation, playerDirection, time, health, reputation;

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
		

	public void calculateDistance()
	{
		waypointCenter = curWaypoint.transform.position;
		playerCenter = gameObject.transform.position;
		playerDirection = gameObject.GetComponent<PlayerCoreController>().rigidController.rotAngle;

		if (playerDirection < 5 && playerDirection > 355 || playerDirection > 175 && playerDirection < 185 )
		{
			swipeLocation = calculateNearest(playerCenter.z,waypointCenter.z);
		}
		if (playerDirection < 95 && playerDirection > 85 || playerDirection > 265 && playerDirection < 275)
		{
			swipeLocation = calculateNearest(playerCenter.x,waypointCenter.x);
		}

		inputScore(swipeLocation);
	}

	float calculateNearest(float player, float waypoint)
	{
		float result;

		if (waypoint > 0 && player > 0)
		{
			result = waypoint - player;	
		}
		else if (waypoint > 0 && player< 0)
		{
			result = waypoint - Mathf.Abs(player);
		}
		else if (waypoint < 0 && player > 0)
		{
			result = Mathf.Abs(waypoint) - player;
		}
		else if (waypoint < 0 && player < 0)
		{
			result = Mathf.Abs(waypoint) - Mathf.Abs(player);
		}
		else 
		{
			result = 0;
		}
		return result;	
	}

	void inputScore(float score)
	{
		waypointScore.Add(score);
	}

	public void calculateFinalScore()
	{
		time = GameManagerScript.Instance.totalTimeLevel1;
		health = PlayerStatusScript.Instance.currentHealth;
	}
}
