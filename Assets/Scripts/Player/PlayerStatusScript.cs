﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusScript : MonoBehaviour, IPlayerComponent
{
	private PlayerCoreController m_Player;
	public void SetPlayer(PlayerCoreController m_Player)
	{
		this.m_Player = m_Player;
	}

	public int health = 3;
	public bool isAlive;
	int temp;
	public static PlayerStatusScript Instance;

	void Awake()
	{
		Instance = this;
	}

	// Use this for initialization
	void Start () 
	{
		temp = health;
		isAlive = true;
		//m_Player.animController.SetisAlive();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(isAlive)
		{
			m_Player.animController.SetisAlive();
			m_Player.ToggleDead(false);
			if(health <= 0)
			{
				isAlive = false;
			//	m_Player.ToggleisAlive(false);
				m_Player.KillPlayer();
				m_Player.ToggleDead(true);
			}

			if(health != temp)
			{
				TakeDamage();
				temp = health;
			}
		}



//		if(Input.touchCount > 0)
//		{
//			Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
//			RaycastHit hit;
//			if(Physics.Raycast(ray, out hit, 10000f))
//			{
//				Debug.Log("Hit");
//				if(hit.collider.tag == "TrapTag")
//				{
//					hit.collider.GetComponent<MotionSensorScript>().isActive = false;
//				}
//			}
//		}
	}

	void TakeDamage()
	{
		HealthBarScript.Instance.ResetHealthBar(health);
	}
		
}
