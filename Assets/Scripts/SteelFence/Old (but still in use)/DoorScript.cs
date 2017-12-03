using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
	private HingeJoint doorHinge;
	private float hingeSpeed;

	private float motorTimer;
	public float motorDuration = 0.5f;

	private float tapTimer;
	public float tapDuration = 1.0f;

	public bool isOpened = false;
	public bool isTappedByPlayer = false;

	public float tapMaxDistance = 50.0f;

	// Use this for initialization
	void Start ()
	{
		doorHinge = GetComponent<HingeJoint>();
		hingeSpeed = doorHinge.motor.targetVelocity;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(doorHinge.axis.y > 0) // If the hinge is rotated by Y-Axis
		{
			isOpened = transform.rotation.eulerAngles.y > (doorHinge.limits.max - doorHinge.limits.min) / 2.0f;
		}

		if(motorTimer < motorDuration)
		{
			motorTimer += Time.deltaTime;
		}
		else
		{
			doorHinge.useMotor = false;
			if(isOpened) CloseDoor(); //Auto Close
		}

		if(!isTappedByPlayer)
		{
			if((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0))
			{
//				Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

				RaycastHit hit;
				if(Physics.Raycast(ray, out hit, tapMaxDistance))
				{
					if(hit.collider.gameObject == this.gameObject)
					{
						Debug.Log("Tapped Door");
						tapTimer = 0.0f;
						isTappedByPlayer = true;
					}
				}
			}
		}
		else
		{
			if(SwipeScript.Instance.GetSwipe() == SwipeDirection.Left)
			{
				OpenDoor();
				tapTimer = 0.0f;
				isTappedByPlayer = false;
			}
			else if(SwipeScript.Instance.GetSwipe() == SwipeDirection.Right)
			{
				CloseDoor();
				tapTimer = 0.0f;
				isTappedByPlayer = false;
			}
			else
			{
				if(tapTimer < tapDuration)
				{
					tapTimer += Time.deltaTime;
				}
				else
				{
					tapTimer = 0.0f;
					isTappedByPlayer = false;
					Debug.Log("Door expired");
				}
			}
		}
	}

	[ContextMenu("Open Door")]
	void OpenDoor()
	{
		if(!doorHinge.useMotor && !isOpened)
		{
			SetHingeSpeed(hingeSpeed);

			doorHinge.useMotor = true;

			motorTimer = 0.0f;
		}
	}

	[ContextMenu("Close Door")]
	void CloseDoor()
	{
		if(!doorHinge.useMotor && isOpened)
		{
			SetHingeSpeed(-hingeSpeed);

			doorHinge.useMotor = true;

			motorTimer = 0.0f;
		}
	}

	void SetHingeSpeed(float speed)
	{
		JointMotor newMotor = doorHinge.motor;
		newMotor.targetVelocity = speed;
		doorHinge.motor = newMotor;
	}

	void OnCollisionEnter(Collision col) //Apply knockback and speed reduction upon collision
	{
		if(col.collider.gameObject.tag == "Enemy")
		{
			PoolManagerScript.Instance.Despawn(col.collider.gameObject);

			if(col.collider.gameObject.GetComponent<SurveillanceDroneScript>())
				ReputationManagerScript.Instance.deadSD++;
			else if(col.collider.gameObject.GetComponent<HuntingDroneScript>())
				ReputationManagerScript.Instance.deadHD++;

			Debug.Log("enemy despwan");
		}
	}
}
