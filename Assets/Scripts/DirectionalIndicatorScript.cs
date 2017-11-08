using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalIndicatorScript : MonoBehaviour {

	public GameObject model;

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
