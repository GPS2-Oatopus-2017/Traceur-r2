using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class RotateCamera : MonoBehaviour
{

	RigidbodyFirstPersonController rbController;

	public float rotationAmount = 360f;
	public float rotationSpeed = 360f;
	public float rotationCounter = 0f;

	public bool isRolling = false;
	public bool isEvent = false;
	public bool isAbleToRoll = false;

	public float eventBraceTime = 1f;
	public float eventCounter = 0f;

	void Awake ()
	{
		rbController = FindObjectOfType<RigidbodyFirstPersonController> ();
	}

	void Start ()
	{
		rotationCounter = rotationAmount;
	}

	void Update ()
	{
		//CheckRollBrace ();

		CheckSlide ();
	}

	void CheckRollBrace ()
	{
		if (!isEvent) {

			eventCounter += Time.deltaTime;

			if (eventCounter >= eventBraceTime) {

				isAbleToRoll = true;

				eventCounter = 0f;

			}
		} else if (isEvent) {

			isAbleToRoll = false;

		}
	}

	void CheckSlide ()
	{

		// Causes rotation within 1 second. ---//
		float rotation = rotationSpeed * Time.deltaTime;

		if (!isEvent && !isRolling && rbController.canSlide && (SwipeScript.Instance.GetSwipe () == SwipeDirection.Down || Input.GetKeyDown (KeyCode.S))) {
			
			isRolling = true;

		}

		if (isRolling) {

			// Player movement is actually dependent on camera position, this problem needs to be fixed. ---//
			rbController.cam.transform.Rotate (Vector3.right * rotation, Space.Self);

			//rotationCounter += Time.deltaTime;

			if (rotationCounter > rotation) {

				rotationCounter -= rotation;

			} else {

				rbController.cam.transform.localRotation = Quaternion.Euler (0.0f, 0.0f, 0.0f);

				rotationCounter = rotationAmount;

				isRolling = false;
			}
		} 
//		else if (!isRolling || !isEvent) {
//
//			rotationCounter = rotationAmount;
//			rbController.cam.transform.localRotation = Quaternion.Euler (0.0f, 0.0f, 0.0f);
//
//		}
	}
}
