using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

	public float bulletSpeed;
	public float selfDestructTimer = 5.0f;

	private Rigidbody bulletRigidbody;

	void Awake()
	{
		bulletRigidbody = GetComponent<Rigidbody>();
	}
		

	void Start() 
	{
		bulletRigidbody.velocity = transform.forward * bulletSpeed;
	}


	void Update() 
	{
		selfDestructTimer -= Time.deltaTime;

		if(selfDestructTimer <= 0)
		{
			Destroy(this.gameObject); 
		}
	}
}
