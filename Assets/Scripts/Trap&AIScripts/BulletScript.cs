using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

	public float bulletSpeed;
	public float selfDestructTimer = 5.0f;
	public bool fromTopHardpoint;

	private Rigidbody bulletRigidbody;
	private SphereCollider bulletSphereCollider;


	void Awake()
	{
		bulletRigidbody = GetComponent<Rigidbody>();
		bulletSphereCollider = GetComponent<SphereCollider>();
	}
		

	void Start() 
	{
		bulletRigidbody.velocity = transform.forward * bulletSpeed;
	}


	void Update() 
	{
		transform.LookAt(GameManagerScript.Instance.player.transform);
		collisionChecker();
		selfDestructFunction();
	}


	void selfDestructFunction()
	{
		selfDestructTimer -= Time.deltaTime;

		if(selfDestructTimer <= 0)
		{
			Destroy(this.gameObject); 
		}
	}


	void collisionChecker()
	{
		if(fromTopHardpoint == true)
		{
			if(GameManagerScript.Instance.player.rigidController.isSliding == true)
			{
				this.bulletSphereCollider.enabled = false;
			}
		}

		if(fromTopHardpoint == false)
		{
			if(GameManagerScript.Instance.player.rigidController.Jumping == true)
			{
				this.bulletSphereCollider.enabled = false;
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			GameManagerScript.Instance.player.status.currentHealth -= 1;
		}
		Destroy(gameObject);
	}
}
