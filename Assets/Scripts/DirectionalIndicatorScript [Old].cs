using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectionalIndicatorScript : MonoBehaviour {

	public GameObject model;
	public bool canTurnLeft = false;
	public bool canTurnRight = false;
	public Image leftIndicator;
	public Image rightIndicator;

	// Use this for initialization
	void Start () 
	{
		model = gameObject.transform.GetChild(0).gameObject;
	}
	
	// Update is called once per frame
	void Update () 	
	{
		
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			if(!model.activeInHierarchy)
			{
				model.SetActive(true);
			}

			if(canTurnLeft)
			{
				if(leftIndicator.enabled == false)
					leftIndicator.enabled = true;
			}

			if(canTurnRight)
			{
				if(rightIndicator.enabled == false)
					rightIndicator.enabled = true;
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.tag == "Player")
		{
			if(model.activeInHierarchy)
			{
				model.SetActive(false);
			}
		}
	}
}
