﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManagerScript : MonoBehaviour
{
	//Singleton Setup
	private static WaypointManagerScript mInstance;

	public static WaypointManagerScript Instance {
		get { return mInstance; }
	}

	public enum EventType
	{
		None = -1,
		MoveForward = 0,
		SwipeLeft,
		SwipeRight
	}

	private PlayerCoreController player;

	public EventType curEvent;
	public bool hasConfirmedEvent = true;

	//The player
	public Direction playerDirection;
	public WaypointNodeScript pointingNode;

	//Nodes that the player touches
	public List<WaypointNodeScript> tracePlayerNodes = new List<WaypointNodeScript> ();
	public List<WaypointNodeScript> touchedNodes = new List<WaypointNodeScript> ();

	[Header("Settings")] [Range(0.0f, 1.0f)]
	public float nearestPathSensitivity = 0.1f;

	[Header("Delay")]
	public float delayBetweenTurns = 0.5f;
	public bool isDelaying = false;
	private float delayTimer = 0.0f;

	public bool isInProximity
	{
		get
		{
			return touchedNodes.Count > 0;
		}
	}

	void Awake ()
	{
		//Singleton Setup
		if (mInstance == null)
			mInstance = this;
		else if (mInstance != this)
			Destroy (this.gameObject);
	}

	// Use this for initialization
	void Start ()
	{
		player = GameManagerScript.Instance.player;
		isDelaying = false;

		//Update player's direction
		float pRot = player.transform.eulerAngles.y;
		if(pRot >= 350.0f || pRot <= 10.0f ) playerDirection = Direction.North;
		else if(pRot >= 80.0f  && pRot <= 100.0f) playerDirection = Direction.East;
		else if(pRot >= 170.0f && pRot <= 190.0f) playerDirection = Direction.South;
		else if(pRot >= 260.0f && pRot <= 280.0f) playerDirection = Direction.West;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(isDelaying)
		{
			delayTimer += Time.deltaTime;
			if(delayTimer >= delayBetweenTurns)
			{
				delayTimer = 0.0f;
				isDelaying = false;
			}
		}
		else
		{
			if (isInProximity)
			{
				if (!hasConfirmedEvent)
				{
					if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Left)
					{
						if (player.interact.isUsingSteelDoor)
						{
							curEvent = EventType.None;
						}
						else
						{
							curEvent = EventType.SwipeLeft;
							hasConfirmedEvent = true;
						}
						
					}
					else if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Right)
					{

						if (player.interact.isUsingSteelDoor)
						{
							curEvent = EventType.None;
						}
						else
						{
							curEvent = EventType.SwipeRight;
							hasConfirmedEvent = true;
						}

					}
					else
					{
						curEvent = EventType.MoveForward;
						hasConfirmedEvent = false;
					}
				}
			}
			else
			{
				hasConfirmedEvent = false;
				curEvent = EventType.None;
			}
		}

		if (curEvent != EventType.None)
		{
			switch (curEvent)
			{
				case EventType.SwipeLeft:
					if (hasConfirmedEvent)
					{
						pointingNode = touchedNodes [0].data.leftNode ((int)playerDirection);
						if (pointingNode)
						{
							playerDirection = (Direction)(((int)playerDirection + 3) % (int)Direction.Total);
							ScoreManagerScript.Instance.MarkPrecision();
							delayTimer = 0.0f;
							isDelaying = true;
						}
						else
						{
							pointingNode = touchedNodes [0].data.forwardNode ((int)playerDirection);
							hasConfirmedEvent = false;
						}
						curEvent = EventType.None;
					}
					break;
				case EventType.SwipeRight:
					if (hasConfirmedEvent)
					{
						pointingNode = touchedNodes [0].data.rightNode ((int)playerDirection);
						if (pointingNode)
						{
							playerDirection = (Direction)(((int)playerDirection + 1) % (int)Direction.Total);
							ScoreManagerScript.Instance.MarkPrecision();
							delayTimer = 0.0f;
							isDelaying = true;
						}
						else
						{
							pointingNode = touchedNodes [0].data.forwardNode ((int)playerDirection);
							hasConfirmedEvent = false;
						}
						curEvent = EventType.None;
					}
					break;
				case EventType.MoveForward:
					pointingNode = touchedNodes [0].data.forwardNode ((int)playerDirection);
					break;
			}
		}

		if (pointingNode)
		{
			Vector3 nearestPath = Vector3.Lerp(player.transform.position, pointingNode.transform.position, nearestPathSensitivity);
			if((int)playerDirection % 2 == 0) nearestPath.x = pointingNode.transform.position.x;
			else nearestPath.z = pointingNode.transform.position.z;
			player.RotateTowards (nearestPath);
		}
	}

	public void RegisterNode (WaypointNodeScript node)
	{
		tracePlayerNodes.Add (node);
		touchedNodes.Add (node);
	}

	public void UnregisterNode (WaypointNodeScript node)
	{
		touchedNodes.Remove (node);
	}
}
