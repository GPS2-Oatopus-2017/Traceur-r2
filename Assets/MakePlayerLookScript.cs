using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class MakePlayerLookScript : MonoBehaviour
{
	public Transform objectToLookAt;

	RigidbodyFirstPersonController rbController;

	public bool toLookAt = false;

	public float timeToLookAt = 2f;
	public float timeToLookCounter = 0f;

	void Start ()
	{
		rbController = FindObjectOfType<RigidbodyFirstPersonController> ();
	}


	void FixedUpdate ()
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

			if (timeToLookCounter >= timeToLookAt) {
				
				Camera.main.transform.rotation = rbController.transform.rotation;
				
				toLookAt = false;

				timeToLookCounter = 0f;
			}
		}
	}
}
