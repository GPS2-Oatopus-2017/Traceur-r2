using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereOptimizer : MonoBehaviour
{

	MeshRenderer mRender;

	void Awake ()
	{
		mRender = GetComponent<MeshRenderer> ();
	}

	void Start ()
	{
		
	}

	void Update ()
	{
		
	}

	void OnTriggerEnter (Collider other)
	{
		if (this.gameObject.layer == 8 && other.gameObject.layer == 9) {
			mRender.enabled = true;
		}

	}

	void OnTriggerStay (Collider other)
	{
		if (this.gameObject.layer == 8 && other.gameObject.layer == 9) {
			mRender.enabled = true;
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (this.gameObject.layer == 8 && other.gameObject.layer == 9) {
			mRender.enabled = false;
		}
	}
}
