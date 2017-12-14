using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapMeScript : MonoBehaviour {

	public bool isSwitch;
	public SwitchScript swscript;
	public MotionSensorScript msScript;
	GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if(isSwitch)
		{
			if(swscript.isOn == false)
				gameObject.SetActive(false);
		}
		else
		{
			if(msScript.isTapped || msScript.isActive == false)
				gameObject.SetActive(false);
		}
		transform.LookAt(player.transform);
	}

}
