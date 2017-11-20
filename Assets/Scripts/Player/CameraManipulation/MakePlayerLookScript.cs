using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class MakePlayerLookScript : MonoBehaviour
{
	public Transform objectToLookAt;

	RigidbodyFirstPersonController rbController;
	RotateCamera rotCam;

	public float slowTime = 0.25f;
	public float originalTime;

	public bool lookOnce = true;
	public bool toLookAt = false;
	public int animCount;

	public float timeToLookAt = 2f;
	public float timeToLookCounter = 0f;

	void Start ()
	{
		rbController = FindObjectOfType<RigidbodyFirstPersonController> ();

		rotCam = FindObjectOfType<RotateCamera> ();

		originalTime = Time.timeScale;
	}


	void LateUpdate ()
	{
		CheckLook ();
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Player") {
			
			Debug.Log ("Player Turns BACK!");

			if (!rotCam.isRolling && lookOnce) {
				
				Time.timeScale = slowTime;
				toLookAt = true;
				lookOnce = false;
				rotCam.isLookBack = true;
				animCount = 0;
			}
		}
	}

	void CheckLook ()
	{
		if (toLookAt) {
			timeToLookCounter += Time.deltaTime;

			if (animCount == 0) {
				//Camera.main.transform.LookAt (objectToLookAt.transform.position);

				Quaternion targetRotation = Quaternion.LookRotation (objectToLookAt.transform.position - Camera.main.transform.position);

				Camera.main.transform.rotation = Quaternion.Lerp (Camera.main.transform.rotation, targetRotation, 5f * Time.deltaTime);

			} else if (animCount == 1) {
				//Camera.main.transform.rotation = rbController.transform.rotation;

				Camera.main.transform.rotation = Quaternion.Lerp (Camera.main.transform.rotation, rbController.transform.rotation, 5f * Time.deltaTime);
			}

			rotCam.isEvent = true;

			if (timeToLookCounter >= timeToLookAt) {
				
				animCount++;

				timeToLookCounter = 0f;

				if (animCount >= 2) {

					Time.timeScale = originalTime;
					
					toLookAt = false;

					rotCam.isLookBack = false;

					animCount = 0;

					rotCam.isEvent = false;
				}
			}
		}
	}
}
