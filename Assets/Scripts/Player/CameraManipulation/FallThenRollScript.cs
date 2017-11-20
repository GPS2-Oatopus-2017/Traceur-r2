using UnityEngine;
using System.Collections;

[System.Serializable]
public class FallThenRollScript : MonoBehaviour, IPlayerComponent
{
	private PlayerCoreController m_Player;

	public void SetPlayer (PlayerCoreController m_Player)
	{
		this.m_Player = m_Player;
	}

	public bool toRoll = false;

	public Vector3 playerGroundPos;
	public Vector3 playerAirPos;

	public float fallDistanceToRoll = 5f;

	void Start ()
	{
		
	}

	void Update ()
	{
		
	}

	void FixedUpdate ()
	{
		CheckVectorFall ();
	}

	void CheckVectorFall ()
	{
		if (m_Player.rigidController.Grounded) {
			
			playerGroundPos = m_Player.rigidController.transform.position;
		}

		if (!m_Player.rigidController.Grounded && !toRoll && !m_Player.rotateCamera.isEvent)
		{
			playerAirPos = m_Player.rigidController.transform.position;

			if (Mathf.Abs (playerAirPos.y) >= Mathf.Abs (playerGroundPos.y + fallDistanceToRoll))
			{
				toRoll = true;
				m_Player.rotateCamera.isEvent = true;
			}
		}

		if (m_Player.rigidController.Grounded && toRoll)
		{
			toRoll = false;

			m_Player.rotateCamera.isEvent = false;

			m_Player.rotateCamera.isRolling = true;

			m_Player.rigidController.isSliding = true;
		}
	}
}

