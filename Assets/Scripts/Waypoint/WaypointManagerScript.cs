using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManagerScript : MonoBehaviour
{
	//Singleton Setup
	private static WaypointManagerScript mInstance;
	public static WaypointManagerScript Instance
	{
		get { return mInstance; }
	}

	public enum EventType
	{
		None = -1,
		MoveForward = 0,
		SwipeLeft,
		SwipeRight
	}
	public EventType curEvent;
	public bool hasConfirmedEvent = true;

	//The player
	public Direction playerDirection;
	public WaypointNodeScript pointingNode;

	//Nodes that the player touches
	public List<WaypointNodeScript> tracePlayerNodes = new List<WaypointNodeScript>();
	public List<WaypointNodeScript> touchedNodes = new List<WaypointNodeScript>();
	public bool isInProximity
	{
		get
		{
			return touchedNodes.Count > 0;
		}
	}

	void Awake()
	{
		//Singleton Setup
		if(mInstance == null) mInstance = this;
		else if(mInstance != this) Destroy(this.gameObject);
	}

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
    {
        if (isInProximity)
        {
            if (!hasConfirmedEvent)
            {  
                if (SwipeScript.Instance.GetSwipe() == SwipeDirection.Left || Input.GetKeyDown(KeyCode.A))
                {
                    curEvent = EventType.SwipeLeft;
                    hasConfirmedEvent = true;
					PlayerScoreScript.Instance.calculateDistance();
					
                }
                else if (SwipeScript.Instance.GetSwipe() == SwipeDirection.Right || Input.GetKeyDown(KeyCode.D))
                {
                    curEvent = EventType.SwipeRight;
                    hasConfirmedEvent = true;
					PlayerScoreScript.Instance.calculateDistance();
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

		if(curEvent != EventType.None)
		{
            switch (curEvent)
            {
                case EventType.SwipeLeft:
					if(hasConfirmedEvent)
					{
						pointingNode = touchedNodes[0].data.leftNode((int)playerDirection);
						playerDirection = (Direction)(((int)playerDirection + 3) % (int)Direction.Total);
	                    curEvent = EventType.None;
					}
                    break;
				case EventType.SwipeRight:
					if(hasConfirmedEvent)
					{
						pointingNode = touchedNodes[0].data.rightNode((int)playerDirection);
						playerDirection = (Direction)(((int)playerDirection + 1) % (int)Direction.Total);
	                    curEvent = EventType.None;
					}
                    break;
                case EventType.MoveForward:
					pointingNode = touchedNodes[0].data.forwardNode((int)playerDirection);
                    break;
            }
        }

		if(pointingNode)
			GameManagerScript.Instance.player.RotateTowards(pointingNode.transform.position);
    }

	public void RegisterNode(WaypointNodeScript node)
	{
		tracePlayerNodes.Add(node);
		touchedNodes.Add(node);

//		for(int i = 0; i < DistanceCalculation.Instance.calculationNodeList.Count; i++)
//		{
//			if(DistanceCalculation.Instance.calculationNodeList[i].node == node.transform)
//				DistanceCalculation.Instance.ResetCalculation(i);
//		}
	}

	public void UnregisterNode(WaypointNodeScript node)
	{
		touchedNodes.Remove(node);
	}
}
