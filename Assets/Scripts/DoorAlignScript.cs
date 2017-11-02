using UnityEngine;
using System.Collections;

public class DoorAlignScript : MonoBehaviour
{


	public float alignDuration = 1.0f;
	private bool toAllignA = false;
	private bool toAllignB = false;
	private bool canAllign = false;
	private float alignTimer = 0.0f;

	PlayerCoreController player;

	public GameObject alignA;
	public GameObject alignB;

	void Awake ()
	{
		player = FindObjectOfType<PlayerCoreController> ();
	}

	void Start ()
	{
	
	}


	void Update ()
	{
		AlignPlayer ();
	}

	void AlignPlayer ()
	{
		if (toAllignA && canAllign) {
			alignTimer += Time.deltaTime;
			player.RotateTowards (alignB.transform.position);

			if (alignTimer >= alignDuration) {
				alignTimer = 0.0f;
				toAllignA = false;
				canAllign = false;
			}
		} else if (toAllignB && canAllign) {
			alignTimer += Time.deltaTime;
			player.RotateTowards (alignA.transform.position);

			if (alignTimer >= alignDuration) {
				alignTimer = 0.0f;
				toAllignB = false;
				canAllign = false;
			}
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Player") {
			toAllignA = true;
			toAllignB = true;
			canAllign = true;
		}
	}
}

