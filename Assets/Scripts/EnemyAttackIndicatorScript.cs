using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackIndicatorScript : MonoBehaviour {

	public float ratio;
	private float defaultScale;
	public float largestScale;
	public float destroyTime;
	private Transform player;
	public float enlargeDelay;
	private float enlargeTimer;

	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindWithTag("Player").transform;
		defaultScale = transform.localScale.x;

		transform.LookAt(player.position);
		Destroy(gameObject, destroyTime);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(enlargeTimer >= enlargeDelay)
		{
			enlargeTimer = 0;
			Vector3 temp = transform.localScale;
			transform.localScale = new Vector3(temp.x += ratio, temp.y += ratio, temp.z += ratio);
		}
		else
		{
			enlargeTimer += 1f * Time.deltaTime;
		}

		if(transform.localScale.x > largestScale)
		{
			transform.localScale = new Vector3(largestScale, largestScale, largestScale);
		}
		/*if(!enlarging)
		{
			Vector3 temp = transform.localScale;
			transform.localScale = new Vector3(temp.x -= ratio, temp.y -= ratio, temp.z -= ratio);

			if(transform.localScale.x <= smallestScale)
			{
				enlarging = true;
			}
		}
		else if(enlarging)
		{
			Vector3 temp = transform.localScale;
			transform.localScale = new Vector3(temp.x += ratio, temp.y += ratio, temp.z += ratio);

			if(transform.localScale.x >= defaultScale)
			{
				enlarging = false;
			}
		}*/
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == ("Bullet"))
		{
			Destroy(gameObject);
		}
	}
}
