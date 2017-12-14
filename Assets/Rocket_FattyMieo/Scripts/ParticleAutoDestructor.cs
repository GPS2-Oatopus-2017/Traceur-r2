using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAutoDestructor : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
		ParticleSystem p = GetComponentInChildren<ParticleSystem>();
		if (!p.loop)
		{
			Destroy(this.gameObject, p.duration);
		}
	}
}
