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

	public float activateObjectTimer = 2f;
	public float activateCounter = 0f;

	public float pushDistance = 5f;

	//Temporary
	[Header("SFX Stuff")]
	public GameObject motionDetector1;
	public GameObject motionDetector2;
	public GameObject steelFence1;
	public GameObject steelFence2;

	void Update ()
	{
		CheckPushable ();
	}

	void LateUpdate()
	{
		Interaction ();
	}

	//When left mouse button is pressed, shoot out a ray cast from screen to pointer.//
	//If the player is within the radius of the object, it will go towards it.//

	void Interaction ()
	{
		#if UNITY_ANDROID && !UNITY_EDITOR
		if((Input.touchCount > 0) && (Input.GetTouch (0).phase == TouchPhase.Began)) 
		{
			Ray raycast = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);
			RaycastHit raycastHit;

//			if(Physics.SphereCast (raycast, sphereCastThickness, out raycastHit)) 
			if(Physics.Raycast(raycast, out raycastHit, rayDistance))
			{
				if(raycastHit.collider.CompareTag ("InteractableObjects")) 
				{
					Iinteractable interact = raycastHit.collider.GetComponent<Iinteractable>();
					interact.Interacted ();
				}
			}
		}
		#elif UNITY_EDITOR
		if (Input.GetMouseButtonDown (0))
		{
			Ray raycast = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit raycastHit;

			Debug.DrawRay(raycast.origin, raycast.direction * rayDistance, Color.white, 10.0f, true);
//			if(Physics.SphereCast (raycast, sphereCastThickness, out raycastHit)) 
			if (Physics.Raycast (raycast, out raycastHit, rayDistance)) {
				//Debug.Log(raycastHit.collider.tag);
//				Debug.DrawLine(raycast.origin, raycastHit.point, Color.white, 10.0f, true);
				Debug.Log(raycastHit.collider.gameObject.name);
				SoundManagerScript.Instance.PlayOneShotSFX3D(AudioClipID.SFX_MD_DEACTIVATED, motionDetector1);
				SoundManagerScript.Instance.PlayOneShotSFX3D(AudioClipID.SFX_MD_DEACTIVATED, motionDetector2);
				if (raycastHit.collider.CompareTag ("InteractableObjects")) {
					Iinteractable interact = raycastHit.collider.GetComponent<Iinteractable> ();
					interact.Interacted ();
				}
			}
		}
		#endif
	}

	GameObject objectToStore;

	void CheckPushable ()
	{
		if (Input.GetMouseButtonDown (0)/* || Input.touchCount > 0*/) {
			Ray mouseRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast (mouseRay, out hit, rayDistance)) {
				if (hit.transform.tag == "Pushable" && !isUsingObject) {
					objectToStore = hit.transform.gameObject;

					Debug.Log ("Pushing Object Detected");

					isUsingObject = true;
				}

				if (hit.transform.tag == "SteelFence" && !isUsingSteelDoor) {
					objectToStore = hit.transform.gameObject;

					Debug.Log ("Steel Door Detected");

					isUsingSteelDoor = true;
				}
			}
		}

		if (isUsingObject) {
			activateCounter += Time.deltaTime;

			Vector3 playerPosition = new Vector3 (this.transform.position.x, objectToStore.transform.position.y, this.transform.position.z);

			objectToStore.transform.LookAt (playerPosition);

			if (activateCounter >= activateObjectTimer) {
				
				activateCounter = 0f;
				isUsingObject = false;
			}

//			if (TerrenceSwipeScript.instance.IsSwiping (_SwipeDirection.LEFT)) {
			if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Left) {
				objectToStore.transform.Translate (-Vector3.left * pushDistance, Space.Self);

				//Vector3.Lerp (objectToStore.transform.position, new Vector3 (objectToStore.transform.position.x, objectToStore.transform.position.y + 10f, objectToStore.transform.position.x), 1f);

				activateCounter = 0f;
				isUsingObject = false;
			} 

//			if (TerrenceSwipeScript.instance.IsSwiping (_SwipeDirection.RIGHT)) {
			if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Right) {
				objectToStore.transform.Translate (-Vector3.right * pushDistance, Space.Self);

				//Vector3.Lerp (objectToStore.transform.position, new Vector3 (objectToStore.transform.position.x, objectToStore.transform.position.y + 10f, objectToStore.transform.position.x), 1f);

				activateCounter = 0f;
				isUsingObject = false;
			}
		}

		//Debug.Log ("Current Rotation = " + GameManagerScript.Instance.player.rigidController.transform.eulerAngles.y);

		if (isUsingSteelDoor) {
			SteelFenceScript steelFence = objectToStore.GetComponent<SteelFenceScript> ();

			activateCounter += Time.deltaTime;

			//Vector3 playerPosition = new Vector3 (player.transform.position.x, objectToStore.transform.position.y, player.transform.position.z);

			//objectToStore.transform.LookAt (playerPosition);

			//gameObject.transform.LookAt (objectToStore.transform.position);

			if (activateCounter >= activateObjectTimer) {
				
				activateCounter = 0f;
				isUsingSteelDoor = false;
			}

//			if (TerrenceSwipeScript.instance.IsSwiping (_SwipeDirection.UP) && steelFence.canSteelDoorUp) {
			if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Up && steelFence.canSteelDoorUp) {

				steelFence.isActivated = true;
				activateCounter = 0f;
				isUsingSteelDoor = false;

				//objectToStore.transform.Translate (Vector3.up * pushDistance, Space.Self);

				//Vector3.Lerp (objectToStore.transform.position, new Vector3 (objectToStore.transform.position.x, objectToStore.transform.position.y + 10f, objectToStore.transform.position.x), 1f);

				//steelFence.canSteelDoorUp = false;
				//steelFence.canSteelDoorDown = true;
			}

//			if (TerrenceSwipeScript.instance.IsSwiping (_SwipeDirection.DOWN) && steelFence.canSteelDoorDown) {
			if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Down && !steelFence.canSteelDoorUp) {

				steelFence.isActivated = true;
				activateCounter = 0f;
				isUsingSteelDoor = false;
			}

			if (GameManagerScript.Instance.player.rigidController.transform.eulerAngles.y >= 80f
			    && GameManagerScript.Instance.player.rigidController.transform.eulerAngles.y <= 100f) {

				if (steelFence.transform.eulerAngles.y == 90f) {
					
					if (!GameManagerScript.Instance.player.rotateCamera.isLookBack) {
						if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Left && steelFence.canSteelDoorLeft) {

							steelFence.isActivated = true;
							activateCounter = 0f;
							isUsingSteelDoor = false;
						}
						if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Right && steelFence.canSteelDoorRight) {

							steelFence.isActivated = true;
							activateCounter = 0f;
							isUsingSteelDoor = false;
						}
					} else if (GameManagerScript.Instance.player.rotateCamera.isLookBack) {
						if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Left && steelFence.canSteelDoorRight) {

							steelFence.isActivated = true;
							activateCounter = 0f;
							isUsingSteelDoor = false;
						}
						if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Right && steelFence.canSteelDoorLeft) {

							steelFence.isActivated = true;
							activateCounter = 0f;
							isUsingSteelDoor = false;
						}
					}
				} else if (steelFence.transform.eulerAngles.y == 270f) {

					if (!GameManagerScript.Instance.player.rotateCamera.isLookBack) {
						if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Right && steelFence.canSteelDoorLeft) {

							steelFence.isActivated = true;
							activateCounter = 0f;
							isUsingSteelDoor = false;
						}
						if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Left && steelFence.canSteelDoorRight) {

							steelFence.isActivated = true;
							activateCounter = 0f;
							isUsingSteelDoor = false;
						}
					} else if (GameManagerScript.Instance.player.rotateCamera.isLookBack) {
						if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Right && steelFence.canSteelDoorRight) {

							steelFence.isActivated = true;
							activateCounter = 0f;
							isUsingSteelDoor = false;
						}
						if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Left && steelFence.canSteelDoorLeft) {

							steelFence.isActivated = true;
							activateCounter = 0f;
							isUsingSteelDoor = false;
						}
					}
				}
			} else if (GameManagerScript.Instance.player.rigidController.transform.eulerAngles.y >= 260
			           && GameManagerScript.Instance.player.rigidController.transform.eulerAngles.y <= 280f) {

				if (steelFence.transform.eulerAngles.y == 270f) {

					if (!GameManagerScript.Instance.player.rotateCamera.isLookBack) {
						if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Left && steelFence.canSteelDoorLeft) {

							steelFence.isActivated = true;
							activateCounter = 0f;
							isUsingSteelDoor = false;
						}
						if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Right && steelFence.canSteelDoorRight) {

							steelFence.isActivated = true;
							activateCounter = 0f;
							isUsingSteelDoor = false;
						}
					} else if (GameManagerScript.Instance.player.rotateCamera.isLookBack) {
						if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Left && steelFence.canSteelDoorRight) {

							steelFence.isActivated = true;
							activateCounter = 0f;
							isUsingSteelDoor = false;
						}
						if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Right && steelFence.canSteelDoorLeft) {

							steelFence.isActivated = true;
							activateCounter = 0f;
							isUsingSteelDoor = false;
						}
					}
				} else if (steelFence.transform.eulerAngles.y == 90f) {

					if (!GameManagerScript.Instance.player.rotateCamera.isLookBack) {
						if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Right && steelFence.canSteelDoorLeft) {

							steelFence.isActivated = true;
							activateCounter = 0f;
							isUsingSteelDoor = false;
						}
						if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Left && steelFence.canSteelDoorRight) {

							steelFence.isActivated = true;
							activateCounter = 0f;
							isUsingSteelDoor = false;
						}
					} else if (GameManagerScript.Instance.player.rotateCamera.isLookBack) {
						if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Right && steelFence.canSteelDoorRight) {

							steelFence.isActivated = true;
							activateCounter = 0f;
							isUsingSteelDoor = false;
						}
						if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Left && steelFence.canSteelDoorLeft) {

							steelFence.isActivated = true;
							activateCounter = 0f;
							isUsingSteelDoor = false;
						}
					}
				}
			} else if (GameManagerScript.Instance.player.rigidController.transform.eulerAngles.y >= 350
			           || GameManagerScript.Instance.player.rigidController.transform.eulerAngles.y <= 10f) {

				if (steelFence.transform.eulerAngles.y == 0) {

					if (!GameManagerScript.Instance.player.rotateCamera.isLookBack) {
						if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Left && steelFence.canSteelDoorLeft) {

							steelFence.isActivated = true;
							activateCounter = 0f;
							isUsingSteelDoor = false;
						}
						if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Right && steelFence.canSteelDoorRight) {

							steelFence.isActivated = true;
							activateCounter = 0f;
							isUsingSteelDoor = false;
						}
					} else if (GameManagerScript.Instance.player.rotateCamera.isLookBack) {
						if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Left && steelFence.canSteelDoorRight) {

							steelFence.isActivated = true;
							activateCounter = 0f;
							isUsingSteelDoor = false;
						}
						if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Right && steelFence.canSteelDoorLeft) {

							steelFence.isActivated = true;
							activateCounter = 0f;
							isUsingSteelDoor = false;
						}
					}
				} else if (steelFence.transform.eulerAngles.y == 180) {

					if (!GameManagerScript.Instance.player.rotateCamera.isLookBack) {
						if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Right && steelFence.canSteelDoorLeft) {

							steelFence.isActivated = true;
							activateCounter = 0f;
							isUsingSteelDoor = false;
						}
						if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Left && steelFence.canSteelDoorRight) {

							steelFence.isActivated = true;
							activateCounter = 0f;
							isUsingSteelDoor = false;
						}
					} else if (GameManagerScript.Instance.player.rotateCamera.isLookBack) {
						if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Right && steelFence.canSteelDoorRight) {

							steelFence.isActivated = true;
							activateCounter = 0f;
							isUsingSteelDoor = false;
						}
						if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Left && steelFence.canSteelDoorLeft) {

							steelFence.isActivated = true;
							activateCounter = 0f;
							isUsingSteelDoor = false;
						}
					}
				} 
			} else if (GameManagerScript.Instance.player.rigidController.transform.eulerAngles.y >= 170
			           && GameManagerScript.Instance.player.rigidController.transform.eulerAngles.y <= 190f) {

				if (steelFence.transform.eulerAngles.y == 180) {

					if (!GameManagerScript.Instance.player.rotateCamera.isLookBack) {
						if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Left && steelFence.canSteelDoorLeft) {

							steelFence.isActivated = true;
							activateCounter = 0f;
							isUsingSteelDoor = false;
						}
						if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Right && steelFence.canSteelDoorRight) {

							steelFence.isActivated = true;
							activateCounter = 0f;
							isUsingSteelDoor = false;
						}
					} else if (GameManagerScript.Instance.player.rotateCamera.isLookBack) {
						if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Left && steelFence.canSteelDoorRight) {

							steelFence.isActivated = true;
							activateCounter = 0f;
							isUsingSteelDoor = false;
						}
						if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Right && steelFence.canSteelDoorLeft) {

							steelFence.isActivated = true;
							activateCounter = 0f;
							isUsingSteelDoor = false;
						}
					}
				} else if (steelFence.transform.eulerAngles.y == 0) {

					if (!GameManagerScript.Instance.player.rotateCamera.isLookBack) {
						if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Right && steelFence.canSteelDoorLeft) {

							steelFence.isActivated = true;
							activateCounter = 0f;
							isUsingSteelDoor = false;
						}
						if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Left && steelFence.canSteelDoorRight) {

							steelFence.isActivated = true;
							activateCounter = 0f;
							isUsingSteelDoor = false;
						}
					} else if (GameManagerScript.Instance.player.rotateCamera.isLookBack) {
						if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Right && steelFence.canSteelDoorRight) {

							steelFence.isActivated = true;
							activateCounter = 0f;
							isUsingSteelDoor = false;
						}
						if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Left && steelFence.canSteelDoorLeft) {

							steelFence.isActivated = true;
							activateCounter = 0f;
							isUsingSteelDoor = false;
						}
					}
				}
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
		SoundManagerScript.Instance.PlayOneShotSFX3D(AudioClipID.SFX_FENCE, steelFence1);
		SoundManagerScript.Instance.PlayOneShotSFX3D(AudioClipID.SFX_FENCE, steelFence2);
		}
	}
}
