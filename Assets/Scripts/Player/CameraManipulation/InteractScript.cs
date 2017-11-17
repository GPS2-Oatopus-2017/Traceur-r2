using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class InteractScript : MonoBehaviour
{
	RaycastHit hit;
	Ray mouseRay;
	public float sphereCastThickness = 3.0f;

	PlayerCoreController player;

	RigidbodyFirstPersonController rbController;

	public float rayDistance = 10f;

	GameObject lightObject;
	Light lightSet;

	public bool toOpenDoor;
	public GameObject currentDoor = null;

	public static InteractScript _instance;

	public static InteractScript Instance { get { return _instance; } }

	public bool isUsingObject = false;
	public bool isUsingSteelDoor = false;

	public float activateObjectTimer = 2f;
	public float activateCounter = 0f;

	void Awake ()
	{
		player = GetComponent<PlayerCoreController> ();
		lightObject = GameObject.Find ("Directional Light");
		lightSet = FindObjectOfType<Light> ();

		if (_instance != null && _instance != this) {
			Destroy (this.gameObject);
		} else {
			_instance = this;
		}

	}

	void Start ()
	{
		rbController = FindObjectOfType<RigidbodyFirstPersonController> ();
	}

	void Update ()
	{
		Interaction ();
	}

//	void OnDrawGizmos()
//	{
//		Gizmos.color = Color.yellow;
//		Gizmos.DrawSphere(transform.position, sphereCastThickness);
//	}

	void FixedUpdate ()
	{
		//CheckInteract ();
		//CheckDoor ();
		//CheckSwitch ();
		CheckPushable ();
	}

	//When left mouse button is pressed, shoot out a ray cast from screen to pointer.//
	//If the player is within the radius of the object, it will go towards it.//

	void Interaction()
	{
		#if UNITY_ANDROID && !UNITY_EDITOR
		if((Input.touchCount > 0) && (Input.GetTouch (0).phase == TouchPhase.Began)) 
		{
			Ray raycast = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);
			RaycastHit raycastHit;

//			if(Physics.SphereCast (raycast, sphereCastThickness, out raycastHit)) 
			if(Physics.Raycast(raycast, out raycastHit, 1000.0f))
			{
				if(raycastHit.collider.CompareTag ("InteractableObjects")) 
				{
					Iinteractable interact = raycastHit.collider.GetComponent<Iinteractable>();
					interact.Interacted ();
				}
			}
		}
		#elif UNITY_EDITOR
		//* For Testing Only -- Mouse Input
		if(Input.GetMouseButtonDown (0)) 
		{
			Ray raycast = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit raycastHit;

//			if(Physics.SphereCast (raycast, sphereCastThickness, out raycastHit)) 
			if(Physics.Raycast(raycast, out raycastHit, 1000.0f))
			{
				//Debug.Log(raycastHit.collider.tag);
				//Debug.Log(raycastHit.collider.gameObject.name);
				if(raycastHit.collider.CompareTag ("InteractableObjects")) 
				{
					Iinteractable interact = raycastHit.collider.GetComponent<Iinteractable>();
					interact.Interacted ();
				}
			}
		}
		//* To Be Deleted When Testing Is Completed
		#endif
	}

	void CheckInteract ()
	{
		if (Input.GetMouseButton (0) || Input.touchCount > 0) {

			mouseRay = Camera.main.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (mouseRay, out hit, rayDistance)) {
				
				//Debug.DrawRay (transform.position, hit.transform.position, Color.red);
				//Debug.Log (hit.transform.name);

				if (hit.transform.tag == "MountainDew") {
					//lightObject.transform.rotation = Quaternion.Euler (20f, -90f, lightObject.transform.rotation.z);
					//lightObject.transform.rotation = Quaternion.Lerp (lightObject.transform.rotation, Quaternion.identity, Time.deltaTime);
					lightSet.color = Random.ColorHSV (0f, 1f, 0f, 1f, 0f, 1f, 0f, 1f);
					hit.transform.Translate (Vector3.up * Time.deltaTime * 3.0f);
					//hit.transform.gameObject.SetActive (false);
				}
				if (hit.transform.tag == "DoorSwitch") {
					
					Transform doorThing = hit.transform.GetChild (0); // Get the child of the switch and deactivates it

					Renderer doorRender = hit.transform.gameObject.GetComponent<Renderer> ();

					if (doorRender.material.color == Color.red) {
						doorRender.material.color = Color.green;
						doorThing.gameObject.SetActive (false);
						//Time.timeScale = 0.5f;
						//player.RotateTowards (hit.transform.position);
					} else {
						doorRender.material.color = Color.red;
						doorThing.gameObject.SetActive (true);
					}
				}

			}
		}
	}

	void CheckSwitch ()
	{
		if (Input.GetMouseButton (0) || Input.touchCount > 0) {

			mouseRay = Camera.main.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (mouseRay, out hit, rayDistance)) {
				
				if (hit.transform.tag == "TrapSwitch") {

					Transform trapThing = hit.transform.GetChild (1);

					Renderer trapRender0 = hit.transform.GetChild (0).gameObject.GetComponent<Renderer> ();
					Renderer trapRender1 = hit.transform.GetChild (1).gameObject.GetComponent<Renderer> ();
					Transform trapObject1 = hit.transform.GetChild (1).gameObject.GetComponent<Transform> ();

					if (trapRender0.material.color == Color.red || trapRender1.material.color == Color.red) {

						trapRender0.material.color = Color.green;
						trapRender1.material.color = Color.green;
						trapObject1.transform.eulerAngles = new Vector3 (120.0f, trapObject1.transform.rotation.y, trapObject1.transform.rotation.z);
						//trapObject1.transform.Rotate (new Vector3 (1f, 0f, 0f), 30f);
						//trapThing.gameObject.SetActive (false);

					} else {

						trapRender0.material.color = Color.red;
						trapRender1.material.color = Color.red;
						trapObject1.transform.eulerAngles = new Vector3 (-120.0f, trapObject1.transform.rotation.y, trapObject1.transform.rotation.z);
						//trapObject1.transform.Rotate (new Vector3 (1f, 0f, 0f), -90f);
						//trapThing.gameObject.SetActive (true);

					}
				}
			}
		}
	}

	void CheckDoor ()
	{
		if (Input.GetMouseButton (0) || Input.touchCount > 0) {

			mouseRay = Camera.main.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (mouseRay, out hit, rayDistance)) {

				//Debug.DrawRay (transform.position, hit.transform.position, Color.red);
				//Debug.Log (hit.transform.name);

				if (hit.transform.tag == "Interactable") {

					player.RotateTowards (hit.transform.position);

					currentDoor = hit.transform.gameObject;

					toOpenDoor = true;
				} else {
					toOpenDoor = false;
				}
			}
		}

		if (SwipeScript.Instance.GetSwipe () == SwipeDirection.Right || Input.GetKeyDown (KeyCode.D)) {
			
			if (toOpenDoor) {
				
				Debug.Log ("Open Door");

				currentDoor.SetActive (false);
			}
		}
	}

	GameObject objectToStore;

	public float pushDistance = 5f;

	void CheckPushable ()
	{
		if (Input.GetMouseButton (0) || Input.touchCount > 0) {

			mouseRay = Camera.main.ScreenPointToRay (Input.mousePosition);

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

			Vector3 playerPosition = new Vector3 (player.transform.position.x, objectToStore.transform.position.y, player.transform.position.z);

			objectToStore.transform.LookAt (playerPosition);

			if (activateCounter >= activateObjectTimer) {

				activateCounter = 0f;

				isUsingObject = false;

			}

			if (TerrenceSwipeScript.instance.IsSwiping (_SwipeDirection.LEFT)) {

				objectToStore.transform.Translate (-Vector3.left * pushDistance, Space.Self);

				//Vector3.Lerp (objectToStore.transform.position, new Vector3 (objectToStore.transform.position.x, objectToStore.transform.position.y + 10f, objectToStore.transform.position.x), 1f);

				activateCounter = 0f;

				isUsingObject = false;

			} 

			if (TerrenceSwipeScript.instance.IsSwiping (_SwipeDirection.RIGHT)) {

				objectToStore.transform.Translate (-Vector3.right * pushDistance, Space.Self);

				//Vector3.Lerp (objectToStore.transform.position, new Vector3 (objectToStore.transform.position.x, objectToStore.transform.position.y + 10f, objectToStore.transform.position.x), 1f);

				activateCounter = 0f;

				isUsingObject = false;

			}
		}

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

			if (TerrenceSwipeScript.instance.IsSwiping (_SwipeDirection.UP) && steelFence.canSteelDoorUp) {

				steelFence.isActivated = true;

				//objectToStore.transform.Translate (Vector3.up * pushDistance, Space.Self);

				//Vector3.Lerp (objectToStore.transform.position, new Vector3 (objectToStore.transform.position.x, objectToStore.transform.position.y + 10f, objectToStore.transform.position.x), 1f);

				activateCounter = 0f;

				isUsingSteelDoor = false;

				//steelFence.canSteelDoorUp = false;
				//steelFence.canSteelDoorDown = true;
			}
		
			if (TerrenceSwipeScript.instance.IsSwiping (_SwipeDirection.DOWN) && steelFence.canSteelDoorDown) {
				
				steelFence.isActivated = true;

				//objectToStore.transform.Translate (-Vector3.up * pushDistance, Space.Self);

				//Vector3.Lerp (objectToStore.transform.position, new Vector3 (objectToStore.transform.position.x, objectToStore.transform.position.y + 10f, objectToStore.transform.position.x), 1f);

				activateCounter = 0f;

				isUsingSteelDoor = false;

				//steelFence.canSteelDoorUp = true;
				//steelFence.canSteelDoorDown = false;
			}
		}
	}
}
