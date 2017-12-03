using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDownTimeScript : MonoBehaviour
{
	public float slowTime = 0.25f;

	public float originalTime;

	public bool nearSwitch = false;

	public GameObject switchToLook;

	public float lookDuration = 2f;
	public float lookCounter = 0f;

	public int animCount;

	private RigidbodyFirstPersonController rbController;
	private RotateCamera rollCamera;

	void Start ()
	{
		originalTime = Time.timeScale;

		PlayerCoreController player = GameManagerScript.Instance.player;
		rbController = player.rigidController;
		rollCamera = player.rotateCamera;
	}

	void Update ()
	{
		
	}

	void LateUpdate ()
	{
		if(!GameManagerScript.Instance.player.status.isAlive) return;
		CheckLookSwitch ();
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Player" && this.gameObject.layer == 12 && !rollCamera.isRolling)
		{
			Time.timeScale = slowTime;

			//Debug.Log ("ENTER Slow Motion");

			nearSwitch = true;

			rollCamera.isEvent = true;
		}
	}

	//	void OnTriggerStay (Collider other)
	//	{
	//		if (other.gameObject.tag == "Player" && this.gameObject.layer == 12) {
	//
	//			Time.timeScale = slowTime;
	//
	//			//Debug.Log ("IN Slow Motion");
	//
	//		}
	//	}
	//
	//	void OnTriggerExit (Collider other)
	//	{
	//		if (other.gameObject.tag == "Player" && this.gameObject.layer == 12) {
	//
	//			Time.timeScale = originalTime;
	//
	//			//Debug.Log ("OUT Slow Motion");
	//
	//		}
	//	}

	void CheckLookSwitch ()
	{
		if (nearSwitch) {

//			Debug.Log ("LOOKING AT OBJECT: " + switchToLook); !!

			lookCounter += Time.deltaTime;

			if (animCount == 0) {
				//Vector3 switchPosition = new Vector3 (Camera.main.transform.position.x, switchToLook.transform.position.y, Camera.main.transform.position.z);

				//Camera.main.transform.LookAt (switchToLook.transform.position);

				Quaternion targetRotation = Quaternion.LookRotation (switchToLook.transform.position - Camera.main.transform.position);

				Camera.main.transform.rotation = Quaternion.Lerp (Camera.main.transform.rotation, targetRotation, 5f * Time.deltaTime);

			} else if (animCount == 1) {
				
				//Quaternion.Lerp (Camera.main.transform.rotation, rbController.transform.rotation, 1f);

				Camera.main.transform.rotation = Quaternion.Lerp (Camera.main.transform.rotation, rbController.transform.rotation, 5f * Time.deltaTime);
			}

			if (lookCounter >= lookDuration) {
				
				animCount++;

				lookCounter = 0f;

				Time.timeScale = originalTime;

				if (animCount >= 2) {
					
					animCount = 0;

					nearSwitch = false;

					rollCamera.isEvent = false;
				}
			}
		}
	}
}
