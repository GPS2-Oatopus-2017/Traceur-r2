using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour, IPlayerComponent
{
	private PlayerCoreController m_Player;
	public void SetPlayer(PlayerCoreController m_Player)
	{
		this.m_Player = m_Player;
	}

	private Animator m_Animator;

	// Use this for initialization
	void Start () 
	{
		m_Animator = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update () 
	{
		m_Animator.SetBool ("Running", m_Player.rigidController.Velocity.magnitude > float.Epsilon);
		m_Animator.SetBool ("Jumping", m_Player.rigidController.Jumping);
		m_Animator.SetFloat ("VerticalVelocity", m_Player.rigidController.Velocity.y);
	}

	public void PlayDeathAnim()
	{
		m_Animator.SetTrigger("Dies");
		m_Animator.SetBool("isAlive",false);
	}

	public void SetisAlive()
	{
		m_Animator.SetBool("isAlive",true);
	}
}
