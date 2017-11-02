using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

[System.Serializable]
public class CheckForWallScript : MonoBehaviour
{

	public Rigidbody playerRb;

	RigidbodyFirstPersonController rbController;

	public bool isStun = false;
	public bool isKnockingBack = false;

	public float stunDuration = 1f;
	public float stunCounter = 0.0f;

	public float originalSpeed;

	public float knockbackSpeed = 0.02f;
	public float knockbackCounter = 0.0f;

	public float knockbackDistance = 10f;

	public float knockbackForce = 100f;
	//public float upwardForce = 10f;

	public float knockbackTime = 0.5f;
	public float knockbackCountdown;

	public Vector3 endPos;
	public Vector3 startPos;

	void Start ()
	{
		rbController = FindObjectOfType<RigidbodyFirstPersonController> ();

		originalSpeed = rbController.movementSettings.ForwardSpeed;
	}


	void Update ()
	{
		CheckKnockBack ();

		CheckStun ();
	}

	void OnTriggerStay (Collider other)
	{
		if (other.gameObject.layer == 10 && this.gameObject.layer == 11 || other.gameObject.tag == "Pushable") {
	
			Debug.Log ("Hit The Wall");
	
			//playerRb.AddForce (Vector3.back * 1000f * Time.deltaTime, ForceMode.Impulse);
	
			//playerRb.AddRelativeForce (Vector3.back * knockbackForce, ForceMode.VelocityChange);

			//playerRb.velocity += Vector3.back * knockbackForce;

			//playerRb.velocity = -(transform.forward * knockbackForce) + (transform.up * upwardForce);

			//playerRb.velocity = (transform.up * upwardForce);

			//playerRb.velocity = -(transform.forward * knockbackForce);

			//endPos = rbController.transform.position + new Vector3 (-knockbackDistance, 0f, 0f);

			//startPos = rbController.transform.position;

			//endPos = rbController.transform.position + transform.forward * -knockbackDistance;

			isKnockingBack = true;
		}
	}


	void CheckKnockBack ()
	{
		if (isKnockingBack) {

			/*
			knockbackCounter += knockbackSpeed;

			rbController.transform.position = Vector3.Lerp (startPos, endPos, knockbackCounter);

			if (rbController.transform.position == endPos) {

				isKnockingBack = false;

				knockbackCounter = 0.0f;

				isStun = true;

			}
			*/

			if (knockbackCountdown <= knockbackTime) {
				
				rbController.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezePositionY;

				knockbackCountdown += Time.deltaTime;

				playerRb.velocity = -(transform.forward * knockbackForce);

			} else {

				knockbackCountdown = 0f;

				isKnockingBack = false;

				isStun = true;

			}
		}
	}


	void CheckStun ()
	{
		if (isStun) {

			rbController.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;

			if (stunDuration >= stunCounter) {

				stunCounter += Time.deltaTime;

				rbController.movementSettings.ForwardSpeed = 0f;

			} else {

				stunCounter = 0f;

				rbController.movementSettings.ForwardSpeed = originalSpeed;

				isStun = false;

				rbController.GetComponent<Rigidbody> ().constraints = ~RigidbodyConstraints.FreezeAll;

			}

		}
	}

	/*
	void OnTriggerStay (Collider other)
	{
		if (other.gameObject.layer == 10 && this.gameObject.layer == 11) {

			Debug.Log ("Hit The Wall");

			//playerRb.AddForce (Vector3.back * 1000f * Time.deltaTime, ForceMode.Impulse);

			playerRb.AddRelativeForce (Vector3.back * knockBackForce, ForceMode.Force);

		}
	}
	*/
}
