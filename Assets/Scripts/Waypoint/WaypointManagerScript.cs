using System.Collections;
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

	public EventType curEvent;
	public bool hasConfirmedEvent = true;

	//The player
	public Direction playerDirection;
	public WaypointNodeScript pointingNode;

	//Nodes that the player touches
	public List<WaypointNodeScript> tracePlayerNodes = new List<WaypointNodeScript> ();
	public List<WaypointNodeScript> touchedNodes = new List<WaypointNodeScript> ();

	public PlayerCoreController player;

	public bool isInProximity {
		get {
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
		GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
		if(playerGO) player = playerGO.GetComponent<PlayerCoreController>();
		else Debug.LogError("GameManager: Unable to find Player!");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (isInProximity) {
			if (!hasConfirmedEvent) {  
				if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Left || Input.GetKeyDown (KeyCode.A)) {
					if (player.interact.isUsingSteelDoor) {
						curEvent = EventType.None;
					} else {
						curEvent = EventType.SwipeLeft;
						hasConfirmedEvent = true;
					}
					
				} else if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Right || Input.GetKeyDown (KeyCode.D)) {

					if (player.interact.isUsingSteelDoor) {
						curEvent = EventType.None;
					} else {
						curEvent = EventType.SwipeRight;
						hasConfirmedEvent = true;
					}

				} else {
					curEvent = EventType.MoveForward;
					hasConfirmedEvent = false;
				}
			}
		} else {
			hasConfirmedEvent = false;
			curEvent = EventType.None;
		}

		if (curEvent != EventType.None) {
			switch (curEvent) {
			case EventType.SwipeLeft:
				if (hasConfirmedEvent) {
					pointingNode = touchedNodes [0].data.leftNode ((int)playerDirection);
					if (pointingNode) {
						playerDirection = (Direction)(((int)playerDirection + 3) % (int)Direction.Total);
						ScoreManagerScript.Instance.MarkPrecision();
					} else {
						pointingNode = touchedNodes [0].data.forwardNode ((int)playerDirection);
						hasConfirmedEvent = false;
					}
					curEvent = EventType.None;
				}
				break;
			case EventType.SwipeRight:
				if (hasConfirmedEvent) {
					pointingNode = touchedNodes [0].data.rightNode ((int)playerDirection);
					if (pointingNode) {
						playerDirection = (Direction)(((int)playerDirection + 1) % (int)Direction.Total);
						ScoreManagerScript.Instance.MarkPrecision();
					} else {
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
			GameManagerScript.Instance.player.RotateTowards (pointingNode.transform.position);
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
