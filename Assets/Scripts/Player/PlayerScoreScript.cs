using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreScript : MonoBehaviour {

	public GameObject curWaypoint,playerCollider, waypointCollider;
	public Vector3 waypointCenter, playerCenter;
	public float swipeLocation, playerDirection;
	public bool startCalculating;

	// Use this for initialization
	void Start () 
	{
		FindPlayer();

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (startCalculating)
		{
			calculateDistance();
		}

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
			startCalculating = true;
		}
	}

	void OnTriggerExit (Collider col)
	{
		if (col.gameObject.tag == "Waypoint")
		{
			startCalculating = false;
		}
	}

	void calculateDistance()
	{
		waypointCenter = curWaypoint.transform.position;
		playerCenter = gameObject.transform.position;
		playerDirection = gameObject.GetComponent<PlayerCoreController>().rigidController.rotAngle;

		if(GameObject.Find("SwipeControlManager").GetComponent<SwipeScript>().swipeDirection != SwipeDirection.None)
		{
			Debug.Log ("start calculating");
			if (playerDirection < 10 && playerDirection > 350 || playerDirection > 170 && playerDirection < 190)
			{
				swipeLocation = calculateNearest(playerCenter.z,waypointCenter.z);
				startCalculating = false;	
			}
			if (playerDirection < 100 && playerDirection > 80 || playerDirection > 260 && playerDirection < 280)
			{
				swipeLocation = calculateNearest(playerCenter.x,waypointCenter.x);
				startCalculating = false;	
			}

		}
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
}
