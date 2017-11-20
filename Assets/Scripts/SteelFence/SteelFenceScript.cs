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

	void Start ()
	{
		rotCam = GameManagerScript.Instance.player.rotateCamera;
	}

	void Update ()
	{
		CheckDirectionInteraction ();
		CheckMoveFence ();
	}

	void CheckDirectionInteraction ()
	{
		if (!rotCam.isLookBack)
		{
//			transform.rotation = Quaternion.Euler (transform.rotation.x, 0f, transform.rotation.z);

			if (isOpen) {
				canSteelDoorLeft = false;
				canSteelDoorRight = true;
			} else if (!isOpen) {
				canSteelDoorLeft = true;
				canSteelDoorRight = false;
			}
		} else if (rotCam.isLookBack) {

//			transform.rotation = Quaternion.Euler (transform.rotation.x, 180f, transform.rotation.z);

			if (isOpen) {
				canSteelDoorLeft = true;
				canSteelDoorRight = false;
			} else if (!isOpen) {
				canSteelDoorLeft = false;
				canSteelDoorRight = true;
			}
		}

//		// Face the opposite direction of player. //
//		if (WaypointManagerScript.Instance.playerDirection == Direction.North)
//		{
//			if (!rotCam.isLookBack)
//			{
////				transform.rotation = Quaternion.Euler (transform.rotation.x, 0f, transform.rotation.z);
//
//				if (isOpen) {
//					canSteelDoorLeft = false;
//					canSteelDoorRight = true;
//				} else if (!isOpen) {
//					canSteelDoorLeft = true;
//					canSteelDoorRight = false;
//				}
//			} else if (rotCam.isLookBack) {
//
////				transform.rotation = Quaternion.Euler (transform.rotation.x, 180f, transform.rotation.z);
//
//				if (isOpen) {
//					canSteelDoorLeft = true;
//					canSteelDoorRight = false;
//				} else if (!isOpen) {
//					canSteelDoorLeft = false;
//					canSteelDoorRight = true;
//				}
//			}
//
//		} else if (WaypointManagerScript.Instance.playerDirection == Direction.South) {
//
//			if (!rotCam.isLookBack) {
//
////				transform.rotation = Quaternion.Euler (transform.rotation.x, 180f, transform.rotation.z);
//
//				if (isOpen) {
//					canSteelDoorLeft = true;
//					canSteelDoorRight = false;
//				} else if (!isOpen) {
//					canSteelDoorLeft = false;
//					canSteelDoorRight = true;
//				}
//			} else if (rotCam.isLookBack) {
////				transform.rotation = Quaternion.Euler (transform.rotation.x, 0f, transform.rotation.z);
//
//				if (isOpen) {
//					canSteelDoorLeft = false;
//					canSteelDoorRight = true;
//				} else if (!isOpen) {
//					canSteelDoorLeft = true;
//					canSteelDoorRight = false;
//				}
//			}
//
//		} else if (WaypointManagerScript.Instance.playerDirection == Direction.West) {
//				
//			if (!rotCam.isLookBack) {
//
////				transform.rotation = Quaternion.Euler (transform.rotation.x, 270f, transform.rotation.z);
//
//				if (isOpen) {
//					canSteelDoorLeft = false;
//					canSteelDoorRight = true;
//				} else if (!isOpen) {
//					canSteelDoorLeft = true;
//					canSteelDoorRight = false;
//				}
//			} else if (rotCam.isLookBack) {
//				
////				transform.rotation = Quaternion.Euler (transform.rotation.x, 90f, transform.rotation.z);
//
//				if (isOpen) {
//					canSteelDoorLeft = true;
//					canSteelDoorRight = false;
//				} else if (!isOpen) {
//					canSteelDoorLeft = false;
//					canSteelDoorRight = true;
//				}
//			}
//
//		} else if (WaypointManagerScript.Instance.playerDirection == Direction.East) {
//
//			if (!rotCam.isLookBack) {
////				transform.rotation = Quaternion.Euler (transform.rotation.x, 90f, transform.rotation.z);
//
//				if (isOpen) {
//					canSteelDoorLeft = true;
//					canSteelDoorRight = false;
//				} else if (!isOpen) {
//					canSteelDoorLeft = false;
//					canSteelDoorRight = true;
//
//				} else if (rotCam.isLookBack) {
//
////					transform.rotation = Quaternion.Euler (transform.rotation.x, 270f, transform.rotation.z);
//
//					if (isOpen) {
//						canSteelDoorLeft = false;
//						canSteelDoorRight = true;
//					} else if (!isOpen) {
//						canSteelDoorLeft = true;
//						canSteelDoorRight = false;
//					}
//				}
//			}
//		}
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
					canSteelDoorLeft = false;
					canSteelDoorRight = false;

				} else if (canSteelDoorDown) {
					
					canSteelDoorUp = true;
					canSteelDoorDown = false;
					canSteelDoorLeft = false;
					canSteelDoorRight = false;

				} else if (canSteelDoorLeft) {
					if (!isOpen) {
						isOpen = true;
					} else {
						isOpen = false;
					}
				} else if (canSteelDoorRight) {
					if (!isOpen) {
						isOpen = true;
					} else {
						isOpen = false;
					}
				}
			}
		}
	}
}
