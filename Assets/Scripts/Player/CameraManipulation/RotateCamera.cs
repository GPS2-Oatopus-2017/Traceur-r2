using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour, IPlayerComponent
{
	private PlayerCoreController m_Player;

	public void SetPlayer (PlayerCoreController m_Player)
	{
		this.m_Player = m_Player;
	}

	public float rotationAmount = 360f;
	public float rotationSpeed = 360f;
	public float rotationCounter = 0f;

	public bool isRolling = false;
	public bool isEvent = false;
	public bool isLookBack = false;

	void Awake ()
	{
		
	}

	void Start ()
	{
		rotationCounter = rotationAmount;
	}

	void Update ()
	{
		if(!GameManagerScript.Instance.player.status.isAlive) return;

		CheckSlide ();
	}

	void CheckSlide ()
	{
		if(!m_Player.status.isAlive)
		{
			rotationCounter = rotationAmount;
			isRolling = false;
			m_Player.rigidController.cam.transform.localRotation = Quaternion.identity;
			return;
		}

		// Causes rotation within 1 second. ---//
		float rotation = rotationSpeed * Time.deltaTime;

		if (isRolling)
		{
			// Player movement is actually dependent on camera position, this problem needs to be fixed. ---//
			m_Player.rigidController.cam.transform.Rotate (Vector3.right * rotation, Space.Self);

			if (rotationCounter > rotation)
			{
				rotationCounter -= rotation;
			}
			else
			{
				m_Player.rigidController.cam.transform.localRotation = Quaternion.Euler (0.0f, 0.0f, 0.0f);

				rotationCounter = rotationAmount;

				isRolling = false;
			}
		}
	}
}
