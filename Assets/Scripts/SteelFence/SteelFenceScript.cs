using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteelFenceScript : MonoBehaviour
{
	public bool canSteelDoorUp = false;
	public bool canSteelDoorDown = false;
	public bool canSteelDoorLeft = false;
	public bool canSteelDoorRight = false;

	public bool isActivated = false;

	public float timeToMove = 1f;
	public float timeCounter = 0f;

	public float openSpeed = 5f;

	public bool isOpen = false;

	private RotateCamera rotCam;
	private RigidbodyFirstPersonController rbController;

	void Start ()
	{
		rotCam = GameManagerScript.Instance.player.rotateCamera;
		rbController = FindObjectOfType<RigidbodyFirstPersonController> ();
	}

	void FixedUpdate ()
	{
		NewSteelDoorCheck ();
		CheckMoveFence ();
	}

	void CheckDirectionInteraction ()
	{
		if (!rotCam.isLookBack) {
//			transform.rotation = Quaternion.Euler (transform.rotation.x, 0f, transform.rotation.z);
			if (WaypointManagerScript.Instance.playerDirection == Direction.West || WaypointManagerScript.Instance.playerDirection == Direction.North) {
				if (!isOpen) {
					canSteelDoorLeft = true;
					canSteelDoorRight = false;
				} else if (isOpen) {
					canSteelDoorLeft = false;
					canSteelDoorRight = true;
				}
			} else if (WaypointManagerScript.Instance.playerDirection == Direction.East || WaypointManagerScript.Instance.playerDirection == Direction.South) {
				if (!isOpen) {
					canSteelDoorLeft = false;
					canSteelDoorRight = true;
				} else if (isOpen) {
					canSteelDoorLeft = true;
					canSteelDoorRight = false;
				}
			}
		} else if (rotCam.isLookBack) {
//			transform.rotation = Quaternion.Euler (transform.rotation.x, 180f, transform.rotation.z);
			if (WaypointManagerScript.Instance.playerDirection == Direction.West || WaypointManagerScript.Instance.playerDirection == Direction.North) {
				if (!isOpen) {
					canSteelDoorLeft = false;
					canSteelDoorRight = true;
				} else if (isOpen) {
					canSteelDoorLeft = true;
					canSteelDoorRight = false;
				}
			} else if (WaypointManagerScript.Instance.playerDirection == Direction.East || WaypointManagerScript.Instance.playerDirection == Direction.South) {
				if (!isOpen) {
					canSteelDoorLeft = true;
					canSteelDoorRight = false;
				} else if (isOpen) {
					canSteelDoorLeft = false;
					canSteelDoorRight = true;
				}
			}
		}
	}

	void CheckMoveFence ()
	{
		if (isActivated) {
			
			timeCounter += Time.deltaTime;

			//Vector3 playerPosition = new Vector3 (rbController.transform.position.x, transform.position.y, rbController.transform.position.z);

			//transform.LookAt (playerPosition);

			if (canSteelDoorUp) {
				transform.Translate (Vector3.up * openSpeed * Time.deltaTime, Space.Self);

			} else if (canSteelDoorDown) {
				transform.Translate (Vector3.down * openSpeed * Time.deltaTime, Space.Self);

			} else if (canSteelDoorLeft) {
				transform.Translate (Vector3.left * openSpeed * Time.deltaTime, Space.Self);

			} else if (canSteelDoorRight) {
				transform.Translate (Vector3.right * openSpeed * Time.deltaTime, Space.Self);
			}

			if (timeCounter >= timeToMove) {
				
				timeCounter = 0f;

				isActivated = false;

				if (canSteelDoorUp) {
					canSteelDoorUp = false;
					canSteelDoorDown = true;

				} else if (canSteelDoorDown) {
					canSteelDoorUp = true;
					canSteelDoorDown = false;

				} else if (canSteelDoorLeft) {
					if (!isOpen) {
						isOpen = true;
					} else if (isOpen) {
						isOpen = false;
					}
				} else if (canSteelDoorRight) {
					if (!isOpen) {
						isOpen = true;
					} else if (isOpen) {
						isOpen = false;
					}
				}
			}
		}
	}

	//West & North swipes left
	//East & South swipes right

	void NewSteelDoorCheck ()
	{
		if (!isOpen) {
			canSteelDoorLeft = true;
			canSteelDoorRight = false;
		} else if (isOpen) {
			canSteelDoorLeft = false;
			canSteelDoorRight = true;
		}
	}
}