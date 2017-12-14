using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserIndicator : MonoBehaviour {

	public GameObject indicator;

	// Use this for initialization
	void Start () {
		//indicator = transform.GetChild(1).GetComponent<GameObject>();
		indicator.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Player")
		{
			indicator.SetActive(true);
		}
	}

	void OnTriggerExit(Collider col)
	{
		if(col.tag == "Player")
		{
			indicator.SetActive(false);
		}
	}
}
