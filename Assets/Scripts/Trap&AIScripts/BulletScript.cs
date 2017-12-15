using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

	public float bulletSpeed;
	public float selfDestructTimer = 5.0f;
	public bool fromTopHardpoint;

	public GameObject explosionPrefab;

	private Rigidbody bulletRigidbody;
	private Collider bulletCollider;


	void Awake()
	{
		bulletRigidbody = GetComponent<Rigidbody>();
		bulletCollider = GetComponent<Collider>();
	}
		

	void Start() 
	{
		bulletRigidbody.velocity = transform.forward * bulletSpeed;
//		Vector3 dir = GameManagerScript.Instance.player.transform.position - transform.position;
//		dir.y = 0.0f;
//		transform.rotation = Quaternion.LookRotation(dir);
//		transform.LookAt(GameManagerScript.Instance.player.transform);
	}


	void Update() 
	{
//		transform.LookAt(GameManagerScript.Instance.player.transform);
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
				this.bulletCollider.enabled = false;
			}
		}

		if(fromTopHardpoint == false) // Bullet is from BOTTOM Hardpoint
		{
			if(GameManagerScript.Instance.player.rigidController.Jumping == true)
			{
				this.bulletCollider.enabled = false;
			}
		}
	}


	void OnTriggerEnter(Collider other)
	{
		if(this.bulletCollider.enabled == true)
		{
			if(other.tag == "PlayerBulletCollider")
			{
				GameManagerScript.Instance.player.status.currentHealth -= 1;
				GameManagerScript.Instance.player.animController.PlayDamagedAnim();
				SoundManagerScript.Instance.PlaySFX2D(AudioClipID.SFX_HIT, false);
				SoundManagerScript.Instance.PlaySFX2D(AudioClipID.SFX_GRUNT, false);
				Transform camTrans = GameManagerScript.Instance.player.rigidController.cam.transform;
				Vector3 spawnPos = camTrans.position;
				if(fromTopHardpoint) spawnPos.y += 2.0f;
				else spawnPos.y -= 2.0f;
				Quaternion spawnRot = camTrans.rotation;
				Instantiate(explosionPrefab, spawnPos, spawnRot, camTrans);
				if(GameManagerScript.Instance.player.status.currentHealth <= 0)
				{
					SoundManagerScript.Instance.StopSFX2D(AudioClipID.SFX_HIT);
					SoundManagerScript.Instance.StopSFX2D(AudioClipID.SFX_GRUNT);
				}

				Destroy(gameObject);
			}
		}
	}
}
