using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackIndicatorScript : MonoBehaviour {

	public float ratio;
	private float defaultScale;
	public float smallestScale = 0.2f;
	private bool enlarging = false;

	// Use this for initialization
	void Start () 
	{
		defaultScale = transform.localScale.x;
		enlarging = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!enlarging)
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
		}
	}
}
