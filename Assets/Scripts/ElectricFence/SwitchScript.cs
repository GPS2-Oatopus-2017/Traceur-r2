using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScript : MonoBehaviour
{

	public GameObject lever;
	public GameObject otherLever;
	public GameObject fence;
	public bool isOn;

	void Start ()
	{
		isOn = true;
		fence.SetActive (true);
	}


	void Update ()
	{
		checkIfTappedAndSwitchMainFunction ();
	}


	public void TurnLever ()
	{
		if (isOn == false) {
			lever.transform.Rotate (new Vector3 (1.0f, 0f, 0f), 90.0f);
			otherLever.transform.Rotate (new Vector3 (1.0f, 0f, 0f), 90.0f);
			isOn = true;
			fence.SetActive (true);
		} else if (isOn == true) {
			lever.transform.Rotate (new Vector3 (1.0f, 0f, 0f), -90.0f);
			otherLever.transform.Rotate (new Vector3 (1.0f, 0f, 0f), -90.0f);
			isOn = false;
			fence.SetActive (false);
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
