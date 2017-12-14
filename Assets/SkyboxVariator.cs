using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxVariator : MonoBehaviour
{
	private int startRot = 0;
	private float speed = 0.1f;
	private bool negator = false;
	void Start ()
	{
		startRot = Random.Range(0, 360);
		negator = (Random.Range(0, 2) == 0 ? false : true);
	}

	// Update is called once per frame
	void Update ()
	{
		if(RenderSettings.skybox)
			RenderSettings.skybox.SetFloat("_Rotation", startRot + (Time.time * (negator ? -speed : speed)));
	}
}
