using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackTimingManagerScript : MonoBehaviour {

	private static AIAttackTimingManagerScript mInstance;
	public static AIAttackTimingManagerScript Instance
	{
		get { return mInstance; }
	}

	void Awake()
	{
		if(mInstance == null) mInstance = this;
		else if(mInstance != this) Destroy(this.gameObject);
	}

	public EnemyData ai_attack_timing_manager;

	public float timer;
	public bool isTop;


	void Start()
	{
		timer = ai_attack_timing_manager.timeForTopOrBottom;

		int randNum = Random.Range(0,2);

		if(randNum == 0)
		{
			isTop = true;
		}
		else
		{
			isTop = false;
		}
	}


	void Update()
	{
		timerMainFunction();
	}


	void timerMainFunction()
	{
		timer -= Time.deltaTime;

		if(timer <= 0)
		{
			if(isTop == true)
			{
				isTop = false;
			}
			else
			{
				isTop = true;
			}

			timer = ai_attack_timing_manager.timeForTopOrBottom;
		}
	}
}
