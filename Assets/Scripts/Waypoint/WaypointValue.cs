using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointValue : MonoBehaviour {

	//!!!
	//Created this script cuz i cannot touch JPs codes.
	//Inherit this script in WaypointNodeScript
	//Also attach this script to all waypoint nodes
	//!!!

	[Header("Mainpath has high value of 10, alternative path has lower value")]
	public int value;

	[Header("Drag all connected nodes into this list")]
	public List<WaypointNodeScript> connectedNodeList = new List<WaypointNodeScript>();
}
