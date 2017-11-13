using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteelFenceScript : MonoBehaviour
{

	public bool canSteelDoorUp = true;
	public bool canSteelDoorDown = false;

	public bool isActivated = false;

	public float timeToMove = 1f;
	public float timeCounter = 0f;

	public float openSpeed = 5f;

	void Start ()
	{
		
	}

	void Update ()
	{
		CheckMoveFence ();
	}

	void CheckMoveFence ()
	{
		if (isActivated) {
			
			timeCounter += Time.deltaTime;

			if (canSteelDoorUp) {
				
				transform.Translate (Vector3.up * openSpeed * Time.deltaTime, Space.Self);

			} else if (canSteelDoorDown) {
				
				transform.Translate (Vector3.down * openSpeed * Time.deltaTime, Space.Self);
			}

			if (timeCounter >= timeToMove) {
				
				timeCounter = 0f;

				isActivated = false;

				if (canSteelDoorUp) {
					
					canSteelDoorUp = false;
					canSteelDoorDown = true;

				} else if (canSteelDoorDown) {
					
					canSteelDoorUp = true;
					canSteelDoorDown = false;
				}
			}
		}
	}
}
