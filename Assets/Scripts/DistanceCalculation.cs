using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DistanceNode
{
	public Transform node;
	public int value;
	public DistanceNode nextNode;
	public DistanceNode nextNode2; //Alternatice node
}

public class DistanceCalculation : MonoBehaviour {

	private static DistanceCalculation mInstance;
	public static DistanceCalculation Instance
	{
		get { return mInstance; }
	}

	public Transform player;

	public List<DistanceNode> allNodeList = new List<DistanceNode>();
	public List<DistanceNode> calculationNodeList = new List<DistanceNode>();

	public DistanceNode currNode;
	public DistanceNode nextNode;

	public float characterDistanceTraveled;
	public float calculatedDistanceAwayFromEnd;
	private float cumulativeDistanceTraveled;
	public float offset;

	void Awake () 
	{
		if(mInstance == null) mInstance = this;
		else if(mInstance != this) Destroy(this.gameObject);

		player = GameObject.FindWithTag("Player").transform;

		for(int i = 0; i < allNodeList.Count; i++)
		{
			if(allNodeList[i].value == 1)
			{
				calculationNodeList.Add(allNodeList[i]);
			}
		}

		for(int i = 0; i < calculationNodeList.Count - 1; i++)
		{
			calculationNodeList[i].nextNode = calculationNodeList[i+1];

			float temp = Vector3.Distance(calculationNodeList[i].node.position, calculationNodeList[i].nextNode.node.position);
			calculatedDistanceAwayFromEnd += temp;
		}

		calculatedDistanceAwayFromEnd -= offset;

		currNode = calculationNodeList[0];
		nextNode = calculationNodeList[0].nextNode;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(nextNode != null)
		{
			float distanceBetweenNodes = Vector3.Distance(nextNode.node.position, currNode.node.position);
			float distanceFromPlayer = Vector3.Distance(player.position, nextNode.node.position);

			float temp = distanceBetweenNodes - distanceFromPlayer;
			characterDistanceTraveled = cumulativeDistanceTraveled + temp;
		}
	}

	public void ResetCalculation(int i)
	{
		cumulativeDistanceTraveled = characterDistanceTraveled;

		currNode = calculationNodeList[i];
		nextNode = currNode.nextNode;
		if(nextNode == null)
			nextNode = null;
	}
}
