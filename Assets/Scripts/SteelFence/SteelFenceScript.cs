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
	public bool hasPlayedFenceAudio = false;

	public float timeToMove = 1f;
	public float timeCounter = 0f;

	public float openSpeed = 5f;

	public bool isOpen = false;

	private RotateCamera rotCam;

	void Start ()
	{
		rotCam = GameManagerScript.Instance.player.rotateCamera;
		hasPlayedFenceAudio = false;
	}

	void FixedUpdate ()
	{
		NewSteelDoorCheck ();
		CheckMoveFence ();
	}

	void CheckDirectionInteraction ()
	{
		Direction playerDir = WaypointManagerScript.Instance.playerDirection;
		if (playerDir == Direction.West || playerDir == Direction.North)
		{
			if(rotCam.isLookBack)
			{
				canSteelDoorLeft = isOpen;
				canSteelDoorRight = !isOpen;
			}
			else
			{
				canSteelDoorLeft = !isOpen;
				canSteelDoorRight = isOpen;
			}
		}
		else if (playerDir == Direction.East || playerDir == Direction.South)
		{
			if(rotCam.isLookBack)
			{
				canSteelDoorLeft = !isOpen;
				canSteelDoorRight = isOpen;
			}
			else
			{
				canSteelDoorLeft = isOpen;
				canSteelDoorRight = !isOpen;
			}
		}
	}

	void CheckMoveFence ()
	{
		if (isActivated)
		{
			if(!hasPlayedFenceAudio)
			{
				SoundManagerScript.Instance.PlayOneShotSFX3D(AudioClipID.SFX_FENCE, this.gameObject);
				hasPlayedFenceAudio = true;
			}

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

			if (timeCounter >= timeToMove)
			{
				timeCounter = 0f;

				isActivated = false;
				hasPlayedFenceAudio = false;

				isOpen = !isOpen;
			}
		}
		else
		{
			hasPlayedFenceAudio = false;
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