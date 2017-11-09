using UnityEngine;
using System.Collections;

public class ItemRotation : MonoBehaviour
{

	public float spinSpeed = 100f;

	void Start ()
	{
	
	}

	void Update ()
	{
		transform.Rotate (Vector3.up, spinSpeed * Time.deltaTime, Space.World);
	}
}

