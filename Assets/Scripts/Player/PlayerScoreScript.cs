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

	void calculateDistance()
	{
		waypointCenter = curWaypoint.transform.position;
		playerCenter = gameObject.transform.position;
		playerDirection = gameObject.GetComponent<PlayerCoreController>().rigidController.rotAngle;

		if(Input.anyKey || Input.touchCount > 0)
		{
			if (playerDirection < 10 && playerDirection > 350 || playerDirection > 170 && playerDirection < 190)
			{
				swipeLocation = waypointCenter.z - playerCenter.z;
				startCalculating = false;	
			}
			else if (playerDirection < 100 && playerDirection > 80 || playerDirection > 260 && playerDirection < 280)
			{
				swipeLocation = waypointCenter.x - playerCenter.x;
				startCalculating = false;	
			}

		}
	}
}
