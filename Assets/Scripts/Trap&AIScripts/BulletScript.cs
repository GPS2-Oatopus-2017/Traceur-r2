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
		if(fromTopHardpoint == true) // Bullet is from TOP Hardpoint
		{
			if(GameManagerScript.Instance.player.rigidController.isSliding == true)
			{
				this.bulletSphereCollider.enabled = false;
			}
		}

		if(fromTopHardpoint == false) // Bullet is from BOTTOM Hardpoint
		{
			if(GameManagerScript.Instance.player.rigidController.Jumping == true)
			{
				this.bulletSphereCollider.enabled = false;
			}
		}
	}


	void OnTriggerEnter(Collider other)
	{
		if(this.bulletSphereCollider.enabled == true)
		{
			if(other.tag == "PlayerBulletCollider")
			{
				PlayerStatusScript.Instance.currentHealth -= 1;
				GameManagerScript.Instance.player.animController.PlayDamagedAnim();
				SoundManagerScript.Instance.PlaySFX2D(AudioClipID.SFX_HIT, false);
				SoundManagerScript.Instance.PlaySFX2D(AudioClipID.SFX_GRUNT, false);
				if(PlayerStatusScript.Instance.currentHealth <= 0)
				{
					SoundManagerScript.Instance.StopSFX2D(AudioClipID.SFX_HIT);
					SoundManagerScript.Instance.StopSFX2D(AudioClipID.SFX_GRUNT);
				}
			}
		}
			
		Destroy(gameObject);
	}
}
