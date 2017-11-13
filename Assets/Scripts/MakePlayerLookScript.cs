using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class MakePlayerLookScript : MonoBehaviour
{
	public Transform objectToLookAt;

	RigidbodyFirstPersonController rbController;
	RotateCamera rotCam;

	public bool toLookAt = false;

	public float timeToLookAt = 2f;
	public float timeToLookCounter = 0f;

	void Start ()
	{
		rbController = FindObjectOfType<RigidbodyFirstPersonController> ();

		rotCam = FindObjectOfType<RotateCamera> ();
	}


	void LateUpdate ()
	{
		CheckLook ();
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Player") {

			Debug.Log ("Player Turns BACK!");

			toLookAt = true;
		}
	}

	void CheckLook ()
	{
		if (toLookAt) {
			
			timeToLookCounter += Time.deltaTime;

			Camera.main.transform.LookAt (objectToLookAt.transform.position);

			//Quaternion targetRotation = Quaternion.LookRotation (objectToLookAt.transform.position - gameObject.transform.position);

			//transform.rotation = Quaternion.Lerp (gameObject.transform.rotation, targetRotation, 1000f * Time.deltaTime);

			rotCam.isEvent = true;

			if (timeToLookCounter >= timeToLookAt) {
				
				Camera.main.transform.rotation = rbController.transform.rotation;
				
				toLookAt = false;

				timeToLookCounter = 0f;

				rotCam.isEvent = false;
			}
		}
	}
}
