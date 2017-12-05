using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnIndicator : MonoBehaviour {

	public Image leftIndicator;
	public Image rightIndicator;
	public bool canTurnLeft = false;
	public bool canTurnRight = false;
	public float hideDelay;
	private float hideTimer;

	// Use this for initialization
	void Start () 
	{
		hideTimer = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(leftIndicator.enabled == true || rightIndicator.enabled == true)
		{
			hideTimer += 1f * Time.deltaTime;

			if(hideTimer >= hideDelay)
			{
				hideTimer = 0;
				leftIndicator.enabled = false;
				rightIndicator.enabled = false;
			}
		}
		else
		{
			hideTimer = 0;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			if(canTurnLeft)
			{
				if(leftIndicator.enabled == false)
				{
					leftIndicator.enabled = true;
				}
			}
			else if(canTurnRight)
			{
				if(rightIndicator.enabled == false)
				{
					rightIndicator.enabled = true;
				}
			}
		}
	}
}
