﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface Iinteractable 
{
	void Interated();
}

public class SwitchScript : MonoBehaviour, Iinteractable
{
	public void Interated()
	{
		TurnLever ();
	}
		
	public GameObject otherSwitch;
	public GameObject fence;
	GameObject lever;
	GameObject otherLever;
	GameObject fenceBar;
	public bool isOn;

	void Start ()
	{
		isOn = true;
		fence.SetActive (true);
		lever = this.transform.GetChild(1).gameObject;
		otherLever = otherSwitch.transform.GetChild(1).gameObject;
		fenceBar = fence.transform.GetChild(1).gameObject;
	}


	void Update ()
	{
		//checkIfTappedAndSwitchMainFunction ();
	}


	public void TurnLever ()
	{
		if (isOn == false) {
			//lever.transform.Rotate (new Vector3 (1.0f, 0f, 0f), 90.0f);
			lever.transform.localEulerAngles = new Vector3(-90,0,0);
			//otherLever.transform.Rotate (new Vector3 (1.0f, 0f, 0f), 90.0f);
			isOn = true;
			fenceBar.SetActive (true);
		} else if (isOn == true) {
			//lever.transform.Rotate (new Vector3 (1.0f, 0f, 0f), -90.0f);
			lever.transform.localEulerAngles = new Vector3(0,0,0);
			//otherLever.transform.Rotate (new Vector3 (1.0f, 0f, 0f), -90.0f);
			isOn = false;
			fenceBar.SetActive (false);
		}
	}


	void checkIfTappedAndSwitchMainFunction ()
	{
		if ((Input.touchCount > 0) && (Input.GetTouch (0).phase == TouchPhase.Began)) {
			Ray raycast = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);
			RaycastHit raycastHit;

			if (Physics.Raycast (raycast, out raycastHit)) {
				if (raycastHit.collider.CompareTag ("TrapSwitch")) {
					TurnLever ();
				}
			}
		}

		//* For Testing Only -- Mouse Input
		if (Input.GetMouseButtonDown (0)) {
			//Debug.Log("Hi");
			Ray raycast = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit raycastHit;

			if (Physics.Raycast (raycast, out raycastHit)) {
				if (raycastHit.collider.CompareTag ("TrapSwitch")) {
					TurnLever ();
				}
			}
		}
		//* To Be Deleted When Testing Is Completed
	}
}
