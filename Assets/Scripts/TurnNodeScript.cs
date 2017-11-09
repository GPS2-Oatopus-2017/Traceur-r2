using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class TurnNodeScript : MonoBehaviour
{

	public bool canForward = false;
	public bool canLeft = false;
	public bool canRight = false;

	UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController controller;
	Rigidbody rb;
	public NodeDetectScript nodeDetect;

	void Awake ()
	{
		controller = FindObjectOfType<RigidbodyFirstPersonController> ();
		rb = FindObjectOfType<Rigidbody> ();
	}

	void Start ()
	{
		
	}

	void Update ()
	{
		
	}

	//Player enters the effector//
	//When no swipes are made//
	//Player will stand on the node until action is taken//

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Player" && this.tag == "Effector") {
			//other.transform.position = new Vector3 (this.transform.position.x, other.transform.position.y, this.transform.position.z);
			canLeft = true;
			canRight = true;
			if (nodeDetect.toLeft && canLeft && !nodeDetect.toRight) {
				rb.constraints = RigidbodyConstraints.FreezePosition;
				controller.rotAngle -= 90.0f;
				rb.constraints = ~RigidbodyConstraints.FreezePosition;
				canLeft = false;
				canRight = false;
			} else if (nodeDetect.toRight && canRight && !nodeDetect.toLeft) {
				rb.constraints = RigidbodyConstraints.FreezePosition;
				controller.rotAngle += 90.0f;
				rb.constraints = ~RigidbodyConstraints.FreezePosition;
				canRight = false;
				canLeft = false;
			}
		}
	}

	void OnTriggerStay (Collider other)
	{
		if (other.tag == "Player" && this.tag == "Effector") {
			if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Left || Input.GetKeyDown (KeyCode.A)) {
				if (canLeft) {
					rb.constraints = RigidbodyConstraints.FreezePosition;
					controller.rotAngle -= 90.0f;
					rb.constraints = ~RigidbodyConstraints.FreezePosition;
					canLeft = false;
					canRight = false;
				}
			} else if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Right || Input.GetKeyDown (KeyCode.D)) {
				if (canRight) {
					rb.constraints = RigidbodyConstraints.FreezePosition;
					controller.rotAngle += 90.0f;
					rb.constraints = ~RigidbodyConstraints.FreezePosition;
					canRight = false;
					canLeft = false;
				}
			}
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.tag == "Player" && this.tag == "Effector") {
			
		}
	}
}
