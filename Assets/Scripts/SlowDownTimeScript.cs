using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class SlowDownTimeScript : MonoBehaviour
{

	public float slowTime = 0.25f;

	public float originalTime;

	public bool nearSwitch = false;

	public GameObject switchToLook;

	public float lookDuration = 2f;
	public float lookCounter = 0f;

	RigidbodyFirstPersonController rbController;

	RotateCamera rollCamera;

	void Start ()
	{
		originalTime = Time.timeScale;

		rbController = FindObjectOfType<RigidbodyFirstPersonController> ();

		rollCamera = FindObjectOfType<RotateCamera> ();
	}

	void Update ()
	{
		
	}

	void FixedUpdate ()
	{
		CheckLookSwitch ();
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Player" && this.gameObject.layer == 12) {

			Time.timeScale = slowTime;

			//Debug.Log ("ENTER Slow Motion");

			nearSwitch = true;

			rollCamera.isEvent = true;

		}
	}

	void OnTriggerStay (Collider other)
	{
		if (other.gameObject.tag == "Player" && this.gameObject.layer == 12) {

			Time.timeScale = slowTime;

			//Debug.Log ("IN Slow Motion");

		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.gameObject.tag == "Player" && this.gameObject.layer == 12) {

			Time.timeScale = originalTime;

			//Debug.Log ("OUT Slow Motion");

		}
	}

	void CheckLookSwitch ()
	{
		if (nearSwitch) {

//			Debug.Log ("LOOKING AT OBJECT: " + switchToLook); !!

			lookCounter += Time.deltaTime;

			//Vector3 switchPosition = new Vector3 (Camera.main.transform.position.x, switchToLook.transform.position.y, Camera.main.transform.position.z);

			Camera.main.transform.LookAt (switchToLook.transform.position);

			if (lookCounter >= lookDuration) {

				//Quaternion.Lerp (Camera.main.transform.rotation, rbController.transform.rotation, 1f);

				Camera.main.transform.rotation = rbController.transform.rotation;

				lookCounter = 0f;

				nearSwitch = false;

				rollCamera.isEvent = false;
			}
		}
	}
}
