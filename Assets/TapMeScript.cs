using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapMeScript : MonoBehaviour {

	public bool isSwitch;
	public GameObject obj;
	public SwitchScript swscript;
	public MotionSensorScript msScript;
	public SlowDownTimeScript slowscript;
	GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		obj = transform.GetChild(0).gameObject;
		if(swscript != null)
			obj.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(isSwitch)
		{
			if(slowscript.nearSwitch && swscript.isOn)
			{
				obj.SetActive(true);
			}
			else if(swscript.isOn == false)
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
