using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Characters.FirstPerson
{
	[RequireComponent (typeof(Rigidbody))]
	[RequireComponent (typeof(CapsuleCollider))]
	public class RigidbodyFirstPersonController : MonoBehaviour, IPlayerComponent
	{
		private PlayerCoreController m_Player;

		public void SetPlayer (PlayerCoreController m_Player)
		{
			this.m_Player = m_Player;
		}

		[System.Serializable]
		public class MovementSettings
		{
			public float ForwardSpeed = 8.0f;
			// Speed when walking forward
			public float BackwardSpeed = 4.0f;
			// Speed when walking backwards
			public float StrafeSpeed = 4.0f;
			// Speed when walking sideways
			public float RunMultiplier = 2.0f;
			// Speed when sprinting
			public KeyCode RunKey = KeyCode.LeftShift;
			public float JumpForce = 30f;
			public AnimationCurve SlopeCurveModifier = new AnimationCurve (new Keyframe (-90.0f, 1.0f), new Keyframe (0.0f, 1.0f), new Keyframe (90.0f, 0.0f));
			[HideInInspector] public float CurrentTargetSpeed = 8f;

			//#if !MOBILE_INPUT
			public bool m_Running;
			public bool m_Dead;
			//#endif
			public bool isAutoRun = false;

			public void UpdateDesiredTargetSpeed (Vector2 input)
			{
				if (input == Vector2.zero)
					return;
				if (input.x > 0 || input.x < 0) {
					//strafe
					CurrentTargetSpeed = StrafeSpeed;
				}
				if (input.y < 0) {
					//backwards
					CurrentTargetSpeed = BackwardSpeed;
				}
				if (input.y > 0) {
					//forwards
					//handled last as if strafing and moving forward at the same time forwards speed should take precedence
					CurrentTargetSpeed = ForwardSpeed;
				}

				if (m_Running) {
					CurrentTargetSpeed *= RunMultiplier; 
				}

				if (m_Dead) {
					CurrentTargetSpeed *= 0;
				}
					
//				CurrentTargetSpeed *= RunMultiplier;
//				m_Running = true;

				//#if !MOBILE_INPUT
				//	            if (Input.GetKey(RunKey))
				//	            {
				//		            CurrentTargetSpeed *= RunMultiplier;
				//		            m_Running = true;
				//	            }
				//	            else
				//	            {
				//		            m_Running = false;
				//	            }
				//#endif
			}

			//#if !MOBILE_INPUT
			public bool Running {
				get { return m_Running; }
			}
			//#endif
		}


		[System.Serializable]
		public class AdvancedSettings
		{
			public float groundCheckDistance = 0.01f;
			// distance for checking if the controller is grounded ( 0.01f seems to work best for this )
			public float stickToGroundHelperDistance = 0.5f;
			// stops the character
			public float slowDownRate = 20f;
			// rate at which the controller comes to a stop when there is no input
			public bool airControl;
			// can the user control the direction that is being moved in the air
			[Tooltip ("set it to 0.1 or more if you get stuck in wall")]
			public float shellOffset;
			//reduce the radius by that ratio to avoid getting stuck in wall (a value of 0.1f is nice)
		}


		public Camera cam;
		public MovementSettings movementSettings = new MovementSettings ();
		//	public MouseLook mouseLook = new MouseLook ();
		public AdvancedSettings advancedSettings = new AdvancedSettings ();

		private Rigidbody m_RigidBody;
		private CapsuleCollider m_Capsule;
		private float m_YRotation;
		private Vector3 m_GroundContactNormal;
		private bool m_Jump, m_PreviouslyGrounded, m_Jumping, m_IsGrounded;


		public Vector3 Velocity {
			get { return m_RigidBody.velocity; }
		}

		public bool Grounded {
			get { return m_IsGrounded; }
		}

		public bool Jumping {
			get { return m_Jumping; }
		}

		public bool Running {
			get {
				// #if !MOBILE_INPUT
				return movementSettings.Running;
				//#else
				//	            return false;
				//#endif
			}
		}

		RotateCamera rotCamera;

		private void Start ()
		{
			m_RigidBody = GetComponent<Rigidbody> ();
			m_Capsule = GetComponent<CapsuleCollider> ();
			rotCamera = FindObjectOfType<RotateCamera> ();
			//mouseLook.Init (transform, cam.transform);
		}

		private void Update ()
		{
			RotateView ();

			if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Up && !m_Jump || Input.GetKeyDown (KeyCode.Space) && !m_Jump) {
				if (!isSliding) {
					m_Jump = true;
				}
			}

			if ((SwipeScript.Instance.GetSwipe () == SwipeDirection.Down || Input.GetKeyDown (KeyCode.S))) {
				if (m_IsGrounded && canSlide && !isSliding && !rotCamera.isEvent) {

					rotCamera.isRolling = true;
					isSliding = true;
					slideTimer = 0.0f;
				}
			}

			Sliding ();

			// Cooldown for sliding. //
			if (!canSlide) {
				
				slideCooldownCounter += Time.deltaTime;

				if (slideCooldownCounter >= slideCooldown) {
					
					slideCooldownCounter = 0f;

					canSlide = true;
				}
			}
		}




		private void FixedUpdate ()
		{
			GroundCheck ();
			Vector2 input = GetInput ();

			if ((Mathf.Abs (input.x) > float.Epsilon || Mathf.Abs (input.y) > float.Epsilon) && (advancedSettings.airControl || m_IsGrounded)) {
				// always move along the camera forward as it is the direction that it being aimed at
				Vector3 desiredMove = transform.forward * input.y;// + cam.transform.right*input.x;
				desiredMove = Vector3.ProjectOnPlane (desiredMove, m_GroundContactNormal).normalized;

				desiredMove.x = desiredMove.x * movementSettings.CurrentTargetSpeed;
				desiredMove.z = desiredMove.z * movementSettings.CurrentTargetSpeed;
				desiredMove.y = desiredMove.y * movementSettings.CurrentTargetSpeed;

				if (m_RigidBody.velocity.sqrMagnitude <
				    (movementSettings.CurrentTargetSpeed * movementSettings.CurrentTargetSpeed)) {
					m_RigidBody.AddForce (desiredMove * SlopeMultiplier (), ForceMode.Impulse);
				}
			}

			if (m_IsGrounded) {
				m_RigidBody.drag = 5f;

				if (m_Jump) {
					m_RigidBody.drag = 0f;
					m_RigidBody.velocity = new Vector3 (m_RigidBody.velocity.x, 0f, m_RigidBody.velocity.z);
					m_RigidBody.AddForce (new Vector3 (0f, movementSettings.JumpForce, 0f), ForceMode.Impulse);
					m_Jumping = true;
				}

				if (!m_Jumping && Mathf.Abs (input.x) < float.Epsilon && Mathf.Abs (input.y) < float.Epsilon && m_RigidBody.velocity.magnitude < 1f) {
					m_RigidBody.Sleep ();
				}
			} else {
				m_RigidBody.drag = 0f;
				if (m_PreviouslyGrounded && !m_Jumping) {
					StickToGroundHelper ();
				}
			}
			m_Jump = false;
		}


		private float SlopeMultiplier ()
		{
			float angle = Vector3.Angle (m_GroundContactNormal, Vector3.up);
			return movementSettings.SlopeCurveModifier.Evaluate (angle);
		}


		private void StickToGroundHelper ()
		{
			RaycastHit hitInfo;
			if (Physics.SphereCast (transform.position, m_Capsule.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo,
				    ((m_Capsule.height / 2f) - m_Capsule.radius) +
				    advancedSettings.stickToGroundHelperDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore)) {
				if (Mathf.Abs (Vector3.Angle (hitInfo.normal, Vector3.up)) < 85f) {
					m_RigidBody.velocity = Vector3.ProjectOnPlane (m_RigidBody.velocity, hitInfo.normal);
				}
			}
		}


		private Vector2 GetInput ()
		{
			Vector2 input;

			// Sabotaged to make auto-run
			if (movementSettings.isAutoRun) {
				input = Vector2.up;
			} else {
				input = Vector2.zero;
//				input = new Vector2
//				{
//					x = CrossPlatformInputManager.GetAxis ("Horizontal"),
//					y = CrossPlatformInputManager.GetAxis ("Vertical")
//				};
			}
			movementSettings.UpdateDesiredTargetSpeed (input);
			return input;
		}

		[Header ("Rot Angle")]
		public float rotAngle = 0.0f;

		private void RotateView ()
		{
			//avoids the mouse looking if the game is effectively paused
			if (Mathf.Abs (Time.timeScale) < float.Epsilon)
				return;

			// get the rotation before it's changed
			float oldYRotation = transform.eulerAngles.y;

			//		//Calculate new angle when turned left or right
			//		float newYRotation = oldYRotation + (CrossPlatformInputManager.GetAxis ("Horizontal") * 100 * Time.deltaTime);


//			if(SwipeScript.Instance.GetSwipe() == SwipeDirection.Left || Input.GetKeyDown (KeyCode.A))
//			{
//				rotAngle -= 90.0f;
//			}
//			else if(SwipeScript.Instance.GetSwipe() == SwipeDirection.Right || Input.GetKeyDown (KeyCode.D))
//			{
//				rotAngle += 90.0f;
//			}

			//Rotate to desired direction after swiping
			float newYRotation = Mathf.LerpAngle (oldYRotation, rotAngle, 10 * Time.deltaTime);
	
			//Apply new angle to the gameobject
			transform.rotation = Quaternion.Euler (0, newYRotation, 0);

			//      mouseLook.LookRotation (transform, cam.transform);

			if (m_IsGrounded || advancedSettings.airControl) {
				// Rotate the rigidbody velocity to match the new direction that the character is looking
				Quaternion velRotation = Quaternion.AngleAxis (transform.eulerAngles.y - oldYRotation, Vector3.up);
				m_RigidBody.velocity = velRotation * m_RigidBody.velocity;
			}
		}

		[Header ("Sliding")]
		public bool isSliding = false;
		public float slideDuration = 1.0f;
		private float slideTimer = 0.0f;
		public bool doOnce = true;

		public bool canSlide = true;
		public float slideCooldown = 2f;
		public float slideCooldownCounter = 0f;

		private void Sliding ()
		{
			if (!isSliding)
				return;
			
			//avoids the mouse looking if the game is effectively paused
			if (Mathf.Abs (Time.timeScale) < float.Epsilon)
				return;

			// get the rotation before it's changed
			float oldXRotation = Camera.main.transform.eulerAngles.x;
			//float newXRotation = 0.0f;
			float rotateTime = Time.deltaTime / 1f;

			slideTimer += Time.deltaTime;

			if (slideTimer >= slideDuration) {
				
				isSliding = false;

				canSlide = false;

				slideTimer = 0.0f;

				if (!doOnce) {

					//this.transform.Rotate (new Vector3 (transform.rotation.x + 90.0f, transform.rotation.y, transform.rotation.z));
					//newXRotation = Mathf.LerpAngle (oldXRotation, 0.0f, 10 * Time.deltaTime);

					m_Capsule.radius += 0.3f;
					m_Capsule.height += 1.0f;
					doOnce = true;
				}

			} else if (slideTimer >= 0.0f) {
				
				//this.transform.Rotate (new Vector3 (transform.rotation.x - 90.0f, transform.rotation.y, transform.rotation.z));
				//newXRotation = Mathf.LerpAngle (oldXRotation, -30.0f, 20 * Time.deltaTime);
				//transform.eulerAngles = Vector3.Lerp (new Vector3 (0f, 0f, 0f), new Vector3 (360f, 0f, 0f), 20 * Time.deltaTime);
				//newXRotation = Mathf.LerpUnclamped (oldXRotation, 360.0f, 20 * Time.deltaTime);

				//cam.transform.rotation = Quaternion.Lerp (cam.transform.rotation, Quaternion.Euler (cam.transform.eulerAngles.x + 360f, cam.transform.eulerAngles.y, cam.transform.eulerAngles.z), 20 * Time.deltaTime);
				//cam.transform.RotateAround (Vector3.zero, Vector3.back, 360f * Time.deltaTime / 1);
				//cam.transform.Rotate (new Vector3 (360f, 0f, 0f), Space.Self);

				if (doOnce) {
					
					m_Capsule.radius -= 0.3f;
					m_Capsule.height -= 1.0f;
					doOnce = false;

				}
			}

			//Apply new angle to the gameobject
			//Camera.main.transform.rotation = Quaternion.Euler (newXRotation, Camera.main.transform.eulerAngles.y, Camera.main.transform.eulerAngles.z);

			//      mouseLook.LookRotation (transform, cam.transform);

			/*
			if (m_IsGrounded || advancedSettings.airControl)
			{
				// Rotate the rigidbody velocity to match the new direction that the character is looking
				Quaternion velRotation = Quaternion.AngleAxis (transform.eulerAngles.x - oldXRotation, Vector3.right);
				m_RigidBody.velocity = velRotation * m_RigidBody.velocity;
			}
			*/

		}

		/// sphere cast down just beyond the bottom of the capsule to see if the capsule is colliding round the bottom
		private void GroundCheck ()
		{
			m_PreviouslyGrounded = m_IsGrounded;
			RaycastHit hitInfo;
			if (Physics.SphereCast (transform.position, m_Capsule.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo,
				    ((m_Capsule.height / 2f) - m_Capsule.radius) + advancedSettings.groundCheckDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore)) {
				m_IsGrounded = true;
				m_GroundContactNormal = hitInfo.normal;
			} else {
				m_IsGrounded = false;
				m_GroundContactNormal = Vector3.up;
			}
			if (!m_PreviouslyGrounded && m_IsGrounded && m_Jumping) {
				m_Jumping = false;
			}
		}
	}
}