using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAutoDestructor : MonoBehaviour
{
	public bool useCustomTime = false;
	public float customTime = 3.0f;

	// Use this for initialization
	void Start ()
	{
		if(!useCustomTime)
		{
			ParticleSystem p = GetComponentInChildren<ParticleSystem>();
			if (!p.loop)
			{
				Destroy(this.gameObject, p.duration);
			}
		}
		else
		{
			Destroy(this.gameObject, customTime);
		}
	}
}
