using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IPlayerComponent
{
	void SetPlayer (PlayerCoreController m_Player);
}

[RequireComponent (typeof(RigidbodyFirstPersonController))]
[RequireComponent (typeof(PlayerAnimationController))]
public class PlayerCoreController : MonoBehaviour
{
	public RigidbodyFirstPersonController rigidController;
	public PlayerAnimationController animController;
	public PlayerStatusScript status;
	public PlayerInteractScript interact;
	public FallThenRollScript fallThenRoll;
	public RotateCamera rotateCamera;

	void Awake ()
	{
		rigidController = GetComponent<RigidbodyFirstPersonController> ();
		animController = GetComponent<PlayerAnimationController> ();
		status = GetComponent<PlayerStatusScript> ();
		interact = GetComponent<PlayerInteractScript>();
		fallThenRoll = GetComponent<FallThenRollScript>();
		rotateCamera = GetComponent<RotateCamera>();

		if (rigidController)
			rigidController.SetPlayer (this);
		if (animController)
			animController.SetPlayer (this);
		if (status)
			status.SetPlayer (this);
		if (interact)
			interact.SetPlayer (this);
		if (fallThenRoll)
			fallThenRoll.SetPlayer (this);
		if (rotateCamera)
			rotateCamera.SetPlayer (this);
	}

	// Use this for initialization
	void Start ()
	{
		
	}

	// Update is called once per frame
	void Update ()
	{
		
	}

	//Cross-object functions
	//...
	public void RotateTowards(Vector3 targetPos)
	{
		Vector3 direction = targetPos - transform.position;
		float angle = Quaternion.LookRotation(direction).eulerAngles.y;
		rigidController.rotAngle = angle;
	}
	
    public void ToggleRunning(bool isRunning)
    {
        rigidController.movementSettings.m_Running = isRunning;
	}

	public void ToggleDead(bool isDead)
	{
		rigidController.movementSettings.m_Dead = isDead;
	}

	public void StartRunning()
	{
		rigidController.movementSettings.isAutoRun = true;
	}

	public void StopRunning()
	{
		rigidController.movementSettings.isAutoRun = false;
	}

	public void KillPlayer()
	{
		rigidController.cam.transform.rotation = Quaternion.identity;
		animController.PlayDeathAnim();
	}

//	public void ToggleisAlive()
//	{
//		animController.SetisAlive();
//	}
}
