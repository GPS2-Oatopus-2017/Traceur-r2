using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowIndicator : MonoBehaviour {

	public Image indicator;
	public float hideTimer;
	public float hideDelay;

	// Use this for initialization
	void Start () 
	{
		indicator = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(indicator.enabled == true)
		{
			hideTimer += 1f * Time.deltaTime;

			if(hideTimer >= hideDelay)
			{
				hideTimer = 0;
				indicator.enabled = false;
			}
		}
		else
		{
			hideTimer = 0;
		}
	}
}
