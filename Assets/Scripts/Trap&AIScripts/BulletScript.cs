using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

	public GameObject player;
	public float bulletSpeed;
	public float selfDestructTimer = 5.0f;
	public bool fromTopHardpoint;

	private Rigidbody bulletRigidbody;
	private SphereCollider bulletSphereCollider;


	void Awake()
	{
		player = GameObject.FindWithTag("Player");
		bulletRigidbody = GetComponent<Rigidbody>();
		bulletSphereCollider = GetComponent<SphereCollider>();
	}
		

	void Start() 
	{
		bulletRigidbody.velocity = transform.forward * bulletSpeed;
	}


	void Update() 
	{
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
		PlayerCoreController playerCoreController = player.GetComponent<PlayerCoreController>();

		if(fromTopHardpoint == true)
		{
			if(playerCoreController.rigidController.isSliding == true)
			{
				this.bulletSphereCollider.enabled = false;
			}
		}

		if(fromTopHardpoint == false)
		{
			if(playerCoreController.rigidController.Jumping == true)
			{
				this.bulletSphereCollider.enabled = false;
			}
		}
	}
}
