using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractScript : MonoBehaviour, IPlayerComponent
{
	private PlayerCoreController m_Player;

	public static PlayerInteractScript Instance;

	public void SetPlayer (PlayerCoreController m_Player)
	{
		this.m_Player = m_Player;
	}

	public float rayDistance = 10f;

	public bool toOpenDoor;
	public GameObject currentDoor = null;

	public bool isUsingObject = false;
	public bool isUsingSteelDoor = false;

	public bool runDeactivateTimer;
	public float deactivateTimer = 0f;

	public float pushDistance = 5f;

	void Start()
	{
		runDeactivateTimer = false;
	}

	void Update ()
	{
		CheckPushable ();
	}

	void LateUpdate ()
	{
		Interaction ();
	}

	//When left mouse button is pressed, shoot out a ray cast from screen to pointer.//
	//If the player is within the radius of the object, it will go towards it.//

//	GameObject objectToStore;
	SteelFenceScript curSteelFence;

	void Interaction ()
	{
		#if UNITY_ANDROID && !UNITY_EDITOR
		if(Input.touchCount > 0)
		{
			if(Input.GetTouch (0).phase == TouchPhase.Began)
			{
				deactivateTimer = 0f;
				isUsingObject = false;
				isUsingSteelDoor = false;
				runDeactivateTimer = false;
				
				Ray raycast = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);
				RaycastHit raycastHit;
				
				if(Physics.Raycast(raycast, out raycastHit, rayDistance))
				{
					if(raycastHit.collider.CompareTag ("InteractableObjects")) 
					{
						Iinteractable interact = raycastHit.collider.GetComponent<Iinteractable>();
						interact.Interacted ();
					}

					if (raycastHit.collider.CompareTag ("Pushable"))
					{
//						objectToStore = raycastHit.collider.gameObject;

						Debug.Log ("Pushing Object Detected");

						isUsingObject = true;
					}

					if (raycastHit.collider.CompareTag ("SteelFence"))
					{
//						objectToStore = raycastHit.collider.gameObject;
						curSteelFence = raycastHit.collider.GetComponent<SteelFenceScript>();

						Debug.Log ("Steel Door Detected");
						
						if(curSteelFence)
							isUsingSteelDoor = true;
						else
							isUsingSteelDoor = false;
					}
				}
			}
			else if(Input.GetTouch (0).phase == TouchPhase.Ended || Input.GetTouch (0).phase == TouchPhase.Canceled)
			{
				runDeactivateTimer = true;
			}
		}
		#elif UNITY_EDITOR
		if (Input.GetMouseButtonDown (0))
		{
			deactivateTimer = 0f;
			isUsingObject = false;
			isUsingSteelDoor = false;
			runDeactivateTimer = false;

			Ray raycast = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit raycastHit;

			Debug.DrawRay (raycast.origin, raycast.direction * rayDistance, Color.white, 10.0f, true);
			if (Physics.Raycast (raycast, out raycastHit, rayDistance))
			{
				if (raycastHit.collider.CompareTag ("InteractableObjects"))
				{
					Iinteractable interact = raycastHit.collider.GetComponent<Iinteractable> ();
					interact.Interacted ();
				}

				if (raycastHit.collider.CompareTag ("Pushable"))
				{
//					objectToStore = raycastHit.collider.gameObject;

					Debug.Log ("Pushing Object Detected");

					isUsingObject = true;
				}

				if (raycastHit.collider.CompareTag ("SteelFence"))
				{
//					objectToStore = raycastHit.collider.gameObject;
					curSteelFence = raycastHit.collider.GetComponent<SteelFenceScript>();

					Debug.Log ("Steel Fence Detected");

					if(curSteelFence)
						isUsingSteelDoor = true;
					else
						isUsingSteelDoor = false;
				}
			}
		}
		else if(Input.GetMouseButtonUp(0))
		{
			runDeactivateTimer = true;
		}
		#endif
	}

	void CheckPushable ()
	{
		if(runDeactivateTimer)
		{
			deactivateTimer += Time.deltaTime;

			if (deactivateTimer >= 0.5f)
			{
				deactivateTimer = 0f;
				isUsingObject = false;
				isUsingSteelDoor = false;
				runDeactivateTimer = false;
			}
		}

		// Terrence's Object Function
//		if (isUsingObject)
//		{
//			activateCounter += Time.deltaTime;
//
//			Vector3 playerPosition = new Vector3 (this.transform.position.x, objectToStore.transform.position.y, this.transform.position.z);
//
//			objectToStore.transform.LookAt (playerPosition);
//
//			if (activateCounter >= activateObjectTimer) {
//				
//				activateCounter = 0f;
//				isUsingObject = false;
//			}
//
//			if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Left) {
//				objectToStore.transform.Translate (-Vector3.left * pushDistance, Space.Self);
//
//				//Vector3.Lerp (objectToStore.transform.position, new Vector3 (objectToStore.transform.position.x, objectToStore.transform.position.y + 10f, objectToStore.transform.position.x), 1f);
//
//				activateCounter = 0f;
//				isUsingObject = false;
//			} 
//
//			if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Right) {
//				objectToStore.transform.Translate (-Vector3.right * pushDistance, Space.Self);
//
//				//Vector3.Lerp (objectToStore.transform.position, new Vector3 (objectToStore.transform.position.x, objectToStore.transform.position.y + 10f, objectToStore.transform.position.x), 1f);
//
//				activateCounter = 0f;
//				isUsingObject = false;
//			}
//		}

		//Edited Steel Fence Function
		if (isUsingSteelDoor)
		{
//			if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Up && curSteelFence.canSteelDoorUp)
//			{
//				curSteelFence.isActivated = true;
//				activateCounter = 0f;
//				isUsingSteelDoor = false;
//
//				//objectToStore.transform.Translate (Vector3.up * pushDistance, Space.Self);
//
//				//Vector3.Lerp (objectToStore.transform.position, new Vector3 (objectToStore.transform.position.x, objectToStore.transform.position.y + 10f, objectToStore.transform.position.x), 1f);
//
//				//steelFence.canSteelDoorUp = false;
//				//steelFence.canSteelDoorDown = true;
//			}
//
//			if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Down && curSteelFence.canSteelDoorDown)
//			{
//				curSteelFence.isActivated = true;
//				activateCounter = 0f;
//				isUsingSteelDoor = false;
//			}

			bool isActivated = false;

//			float playerEulerAngleY = transform.eulerAngles.y;
			Direction playerDir = WaypointManagerScript.Instance.playerDirection;
			float steelFenceEulerAngleY = curSteelFence.transform.eulerAngles.y;
			Direction steelFenceDir = Direction.North;
			bool isLookBack = m_Player.rotateCamera.isLookBack;

			bool isNormal = false;
			bool isInversed = false;

			//More accurate player direction
//			if (playerEulerAngleY >= 350f || playerEulerAngleY <= 10f) // North // Smart Choice
//				playerDir = Direction.North;
//			else if (playerEulerAngleY >= 170f && playerEulerAngleY <= 190f) // South
//				playerDir = Direction.South;
//			else if (playerEulerAngleY >= 80f && playerEulerAngleY <= 100f) // East
//				playerDir = Direction.East;
//			else if (playerEulerAngleY >= 260f && playerEulerAngleY <= 280f) // West
//				playerDir = Direction.West;

			//Get direction of steel fence
			if (steelFenceEulerAngleY >= 350f || steelFenceEulerAngleY <= 10f) // North // Smart Choice
				steelFenceDir = Direction.North;
			else if (steelFenceEulerAngleY >= 170f && steelFenceEulerAngleY <= 190f) // South
				steelFenceDir = Direction.South;
			else if (steelFenceEulerAngleY >= 80f && steelFenceEulerAngleY <= 100f) // East
				steelFenceDir = Direction.East;
			else if (steelFenceEulerAngleY >= 260f && steelFenceEulerAngleY <= 280f) // West
				steelFenceDir = Direction.West;

			//Check directions
			if(playerDir == steelFenceDir)
			{
				if(!isLookBack)
				{
					isNormal = true;
				}
				else
				{
					isInversed = true;
				}
			}
			else if(playerDir == (Direction)(((int)steelFenceDir + 2) % (int)Direction.Total))
			{
				if(!isLookBack)
				{
					isInversed = true;
				}
				else
				{
					isNormal = true;
				}
			}

			//Normal
			if(isNormal)
			{
				if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Left && curSteelFence.canSteelDoorLeft)
				{
					isActivated = true;
				}
				else if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Right && curSteelFence.canSteelDoorRight)
				{
					isActivated = true;
				}
			}
			//Inversed
			else if(isInversed)
			{
				if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Left && curSteelFence.canSteelDoorRight)
				{
					isActivated = true;
				}
				else if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Right && curSteelFence.canSteelDoorLeft)
				{
					isActivated = true;
				}
			}

			if(isActivated)
			{
				curSteelFence.isActivated = true;
				deactivateTimer = 0f;
				isUsingSteelDoor = false;
				runDeactivateTimer = false;
			}
		
			//----------------------------------OLDEN----------------------------------//
			//----------------------------------STUFF----------------------------------//
			//----------------------------------BELOW----------------------------------//

			/*
//			if (TerrenceSwipeScript.instance.IsSwiping (_SwipeDirection.LEFT) && steelFence.canSteelDoorLeft) {
				if (WaypointManagerScript.Instance.playerDirection == Direction.West) {
				
					if (GameManagerScript.Instance.player.rigidController.transform.eulerAngles.y >= 80f
					    && GameManagerScript.Instance.player.rigidController.transform.eulerAngles.y <= 100f ||
					    GameManagerScript.Instance.player.rigidController.transform.eulerAngles.y >= 260f
					    && GameManagerScript.Instance.player.rigidController.transform.eulerAngles.y <= 280f) {

						if (steelFence.transform.eulerAngles.y == 90f || steelFence.transform.eulerAngles.y == 270f) {

							if (!GameManagerScript.Instance.player.rotateCamera.isLookBack) {
								if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Left && steelFence.canSteelDoorLeft) {

									steelFence.isActivated = true;
									activateCounter = 0f;
									isUsingSteelDoor = false;
								}
								//if (TerrenceSwipeScript.instance.IsSwiping (_SwipeDirection.RIGHT) && steelFence.canSteelDoorRight) {
								if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Right && !steelFence.canSteelDoorLeft) {

									steelFence.isActivated = true;
									activateCounter = 0f;
									isUsingSteelDoor = false;
								}
							} else if (GameManagerScript.Instance.player.rotateCamera.isLookBack) {
								if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Left && !steelFence.canSteelDoorLeft) {

									steelFence.isActivated = true;
									activateCounter = 0f;
									isUsingSteelDoor = false;
								}
								//if (TerrenceSwipeScript.instance.IsSwiping (_SwipeDirection.RIGHT) && steelFence.canSteelDoorRight) {
								if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Right && steelFence.canSteelDoorLeft) {

									steelFence.isActivated = true;
									activateCounter = 0f;
									isUsingSteelDoor = false;
								}
							}
						}
					}
				} else if (WaypointManagerScript.Instance.playerDirection == Direction.East) {

					if (GameManagerScript.Instance.player.rigidController.transform.eulerAngles.y >= 80f
					    && GameManagerScript.Instance.player.rigidController.transform.eulerAngles.y <= 100f ||
					    GameManagerScript.Instance.player.rigidController.transform.eulerAngles.y >= 260f
					    && GameManagerScript.Instance.player.rigidController.transform.eulerAngles.y <= 280f) {

						if (steelFence.transform.eulerAngles.y == 90f || steelFence.transform.eulerAngles.y == 270f) {

							if (!GameManagerScript.Instance.player.rotateCamera.isLookBack) {
								if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Right && steelFence.canSteelDoorLeft) {

									steelFence.isActivated = true;
									activateCounter = 0f;
									isUsingSteelDoor = false;
								}
								//if (TerrenceSwipeScript.instance.IsSwiping (_SwipeDirection.RIGHT) && steelFence.canSteelDoorRight) {
								if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Left && !steelFence.canSteelDoorLeft) {

									steelFence.isActivated = true;
									activateCounter = 0f;
									isUsingSteelDoor = false;
								}
							} else if (GameManagerScript.Instance.player.rotateCamera.isLookBack) {
								if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Right && !steelFence.canSteelDoorLeft) {

									steelFence.isActivated = true;
									activateCounter = 0f;
									isUsingSteelDoor = false;
								}
								//if (TerrenceSwipeScript.instance.IsSwiping (_SwipeDirection.RIGHT) && steelFence.canSteelDoorRight) {
								if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Left && steelFence.canSteelDoorLeft) {

									steelFence.isActivated = true;
									activateCounter = 0f;
									isUsingSteelDoor = false;
								}
							}
						}
					}
				} else if (WaypointManagerScript.Instance.playerDirection == Direction.North) {

					if (GameManagerScript.Instance.player.rigidController.transform.eulerAngles.y >= 350f
					    || GameManagerScript.Instance.player.rigidController.transform.eulerAngles.y <= 10f ||
					    GameManagerScript.Instance.player.rigidController.transform.eulerAngles.y >= 170f
					    && GameManagerScript.Instance.player.rigidController.transform.eulerAngles.y <= 190f) {

						if (steelFence.transform.eulerAngles.y == 0f || steelFence.transform.eulerAngles.y == 180f) {

							if (!GameManagerScript.Instance.player.rotateCamera.isLookBack) {
								if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Left && steelFence.canSteelDoorLeft) {

									steelFence.isActivated = true;
									activateCounter = 0f;
									isUsingSteelDoor = false;
								}
								//if (TerrenceSwipeScript.instance.IsSwiping (_SwipeDirection.RIGHT) && steelFence.canSteelDoorRight) {
								if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Right && !steelFence.canSteelDoorLeft) {

									steelFence.isActivated = true;
									activateCounter = 0f;
									isUsingSteelDoor = false;
								}
							} else if (GameManagerScript.Instance.player.rotateCamera.isLookBack) {
								if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Left && !steelFence.canSteelDoorLeft) {

									steelFence.isActivated = true;
									activateCounter = 0f;
									isUsingSteelDoor = false;
								}
								//if (TerrenceSwipeScript.instance.IsSwiping (_SwipeDirection.RIGHT) && steelFence.canSteelDoorRight) {
								if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Right && steelFence.canSteelDoorLeft) {

									steelFence.isActivated = true;
									activateCounter = 0f;
									isUsingSteelDoor = false;
								}
							}
						}
					}
				} else if (WaypointManagerScript.Instance.playerDirection == Direction.South) {

					if (GameManagerScript.Instance.player.rigidController.transform.eulerAngles.y >= 350f
					    || GameManagerScript.Instance.player.rigidController.transform.eulerAngles.y <= 10f ||
					    GameManagerScript.Instance.player.rigidController.transform.eulerAngles.y >= 170f
					    && GameManagerScript.Instance.player.rigidController.transform.eulerAngles.y <= 190f) {

						if (steelFence.transform.eulerAngles.y == 0f || steelFence.transform.eulerAngles.y == 180f) {

							if (!GameManagerScript.Instance.player.rotateCamera.isLookBack) {
								if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Right && steelFence.canSteelDoorLeft) {

									steelFence.isActivated = true;
									activateCounter = 0f;
									isUsingSteelDoor = false;
								}
								//if (TerrenceSwipeScript.instance.IsSwiping (_SwipeDirection.RIGHT) && steelFence.canSteelDoorRight) {
								if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Left && !steelFence.canSteelDoorLeft) {

									steelFence.isActivated = true;
									activateCounter = 0f;
									isUsingSteelDoor = false;
								}
							} else if (GameManagerScript.Instance.player.rotateCamera.isLookBack) {
								if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Right && !steelFence.canSteelDoorLeft) {

									steelFence.isActivated = true;
									activateCounter = 0f;
									isUsingSteelDoor = false;
								}
								//if (TerrenceSwipeScript.instance.IsSwiping (_SwipeDirection.RIGHT) && steelFence.canSteelDoorRight) {
								if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Left && steelFence.canSteelDoorLeft) {

									steelFence.isActivated = true;
									activateCounter = 0f;
									isUsingSteelDoor = false;
								}
							}
						}
					}
				}
			}
			*/
		}
	}
}
