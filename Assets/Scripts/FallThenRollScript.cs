using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

[System.Serializable]
public class FallThenRollScript : MonoBehaviour
{

	RigidbodyFirstPersonController rbController;
	RotateCamera rotateCam;
	public Camera cam;

	public float timeToFall = 1f;
	public float timeFallCounter = 0f;

	public bool toRoll = false;

	float oldXRotation = 0.0f;
	float newXRotation = 0.0f;

	void Start ()
	{
		rbController = FindObjectOfType<RigidbodyFirstPersonController> ();

		rotateCam = FindObjectOfType<RotateCamera> ();

		oldXRotation = cam.transform.eulerAngles.x;
	}

	void Update ()
	{
		
	}

	void FixedUpdate ()
	{
		CheckFall ();
	}

	void CheckFall ()
	{

		if (!rbController.Grounded && !toRoll) {

			timeFallCounter += Time.deltaTime;

			if (timeFallCounter >= timeToFall) {

				//cam.transform.Rotate (new Vector3 (transform.rotation.x + 30.0f, transform.rotation.y, transform.rotation.z));
				newXRotation = Mathf.LerpAngle (oldXRotation, 30.0f, 10 * Time.deltaTime);
				//Quaternion.Lerp (transform.rotation, Quaternion.Euler (transform.rotation.x + 30f, transform.rotation.y, transform.rotation.z), 1f);

				timeFallCounter = 0f;

				toRoll = true;

				rotateCam.isEvent = true;
			}
		}

		if (rbController.Grounded && toRoll) {

			//cam.transform.Rotate (new Vector3 (transform.rotation.x - 30.0f, transform.rotation.y, transform.rotation.z));
			newXRotation = Mathf.LerpAngle (oldXRotation, 0.0f, 10 * Time.deltaTime);
			//Quaternion.Lerp (transform.rotation, Quaternion.Euler (transform.rotation.x - 30f, transform.rotation.y, transform.rotation.z), 1f);

			toRoll = false;

			//rotateCam.isAbleToRoll = true;

			rotateCam.isEvent = false;

			rotateCam.isRolling = true;
		}
	}
}

