using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

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

	void Awake ()
	{
		rigidController = GetComponent<RigidbodyFirstPersonController> ();
		animController = GetComponent<PlayerAnimationController> ();

		if (rigidController)
			rigidController.SetPlayer (this);
		if (animController)
			animController.SetPlayer (this);
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

	public void StartRunning()
	{
		rigidController.movementSettings.isAutoRun = true;
	}

	public void StopRunning()
	{
		rigidController.movementSettings.isAutoRun = false;
	}
}
