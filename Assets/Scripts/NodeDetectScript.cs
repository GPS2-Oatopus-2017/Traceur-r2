using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class NodeDetectScript : MonoBehaviour
{

	public bool toForward = false;
	public bool toLeft = false;
	public bool toRight = false;

	UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController controller;
	Rigidbody rb;

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

	//Player enters the detector//
	//Swipes can be made when inside detector//
	//Will auto turn when player hits effector//

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Player" && this.tag == "Detector") {
			toLeft = false;
			toRight = false;
		}
	}

	void OnTriggerStay (Collider other)
	{
		if (other.tag == "Player" && this.tag == "Detector") {
			if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Left || Input.GetKeyDown (KeyCode.A)) {
				toLeft = true;
				toRight = false;
			} else if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Right || Input.GetKeyDown (KeyCode.D)) {
				toRight = true;
				toLeft = false;
			}
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.tag == "Player" && this.tag == "Detector") {
			
		}
	}
}
