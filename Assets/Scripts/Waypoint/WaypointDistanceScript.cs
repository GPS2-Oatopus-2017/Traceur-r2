using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointDistanceScript : MonoBehaviour {

	//!!!
	//Attach this script to waypoint manager and drag all waypoints into the list
	//Drag first node to currNode
	//!!!

	public static WaypointDistanceScript Instance;

	public Transform player;

	public List<WaypointNodeScript> calculatedWaypoint = new List<WaypointNodeScript>();

	public WaypointNodeScript currNode;
	public WaypointNodeScript nextNode;

	public float totalDistance = 0f;
	public float playerDistanceTraveled = 0f;

	//Calculations for curr and next node
	private float currToNextDistance = 0f;
	private float playerCumulatedDistanceTraveled = 0f;

	void Awake()
	{
		Instance = this;
	}

	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindWithTag("Player").transform;

		RecalculateWaypoint(currNode);

		ReorderWaypoint();

		nextNode = currNode.nextNode;
		currToNextDistance = Vector3.Distance(currNode.transform.position, nextNode.transform.position);

		RecalculateDistance();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(nextNode != null)
			UpdatePlayerDistanceTraveled();
	}

	void UpdatePlayerDistanceTraveled()
	{
		float playerToNextDistance = Vector3.Distance(player.position, nextNode.transform.position);

		if(playerToNextDistance < -2)
		{
			RecalculateWaypoint(currNode);
		}
		else
		{
			playerDistanceTraveled = (currToNextDistance - playerToNextDistance) + playerCumulatedDistanceTraveled;
		}
	}

	public void ResetNodes(WaypointNodeScript newNode)
	{
		if(newNode == currNode)
			return;

		if(newNode != nextNode)
		{
			for(int i = 0; i < calculatedWaypoint.Count; i++)
			{
				if(calculatedWaypoint[i] == currNode)
				{
					calculatedWaypoint.Insert(i + 1, newNode);
					currToNextDistance = Vector3.Distance(calculatedWaypoint[i].transform.position, calculatedWaypoint[i + 1].transform.position);
					break;
				}
			}

			RecalculateWaypoint(newNode);
			ReorderWaypoint();
			RecalculateDistance();
		}

		playerCumulatedDistanceTraveled += currToNextDistance;

		currNode = newNode;
		nextNode = newNode.nextNode;

		if(nextNode == null)
			return;

		currToNextDistance = Vector3.Distance(currNode.transform.position, nextNode.transform.position);
	}

	//For taking alternative path
	void RecalculateWaypoint(WaypointNodeScript mCurrNode)
	{
		///Remove all nodes behind current nodes
		if(calculatedWaypoint.Count == 0)
		{
			calculatedWaypoint.Add(mCurrNode);
		}
		else
		{
			for(int i = calculatedWaypoint.Count - 1; i > -1 ; i--)
			{
				if(calculatedWaypoint[i] == mCurrNode)
				{
					break;
				}
				else
				{
					calculatedWaypoint.RemoveAt(i);
				}
			}
		}
		///

		///Find and assign new next nodes
		int tempNodeIndex = 0;
		int tempValue = 0;

		if(mCurrNode.connectedNodeList.Count > 1)
		{
			for(int i = 0; i < mCurrNode.connectedNodeList.Count; i++)
			{
				if(mCurrNode.connectedNodeList[i].value > tempValue)
				{
					if(calculatedWaypoint.Count > 2)
					{
						if(mCurrNode.connectedNodeList[i] != calculatedWaypoint[calculatedWaypoint.Count - 2])
						{
							tempNodeIndex = i;
							tempValue = mCurrNode.connectedNodeList[i].value;
						}
					}
					else
					{
						tempNodeIndex = i;
						tempValue = mCurrNode.connectedNodeList[i].value;
					}

				}
			}
		}
		else
		{
			return;
		}

		calculatedWaypoint.Add(mCurrNode.connectedNodeList[tempNodeIndex]);

		RecalculateWaypoint(mCurrNode.connectedNodeList[tempNodeIndex]);
		///
	}

	//Reorder next nodes for new list
	void ReorderWaypoint()
	{
		for(int i = 0; i < calculatedWaypoint.Count - 1; i++)
		{
			calculatedWaypoint[i].nextNode = calculatedWaypoint[i + 1];
		}
	}

	//Recalculate total distance for new list
	void RecalculateDistance()
	{
		totalDistance = 0f;

		for(int i = 0; i < calculatedWaypoint.Count - 1; i++)
		{
			float mDistance = Vector3.Distance(calculatedWaypoint[i].transform.position, calculatedWaypoint[i].nextNode.transform.position);
			totalDistance += mDistance;
		}
	}
}
