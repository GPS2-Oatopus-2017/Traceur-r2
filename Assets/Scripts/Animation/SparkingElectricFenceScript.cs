using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkingElectricFenceScript : MonoBehaviour
{
	public LineRenderer[] rends;
//	public Material[] mats;
	private float spriteOffset;
	public int spriteAmount;
//	public Sprite[] sprites;
	private int[] spritesSelected;

	public float fps = 30.0f;
	private float timer = 0.0f;

	public bool allRandom = false;

	// Use this for initialization
	void Start ()
	{
		spriteOffset = 1.0f / spriteAmount;
		spritesSelected = new int[spriteAmount];

		for(int i = 0; i < rends.Length; i++)
		{
			spritesSelected[i] = Random.Range(0, spriteAmount);

			rends[i].material.mainTextureOffset = new Vector2(0.0f, spritesSelected[i] * spriteOffset);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(spriteAmount < 2)
		{
			Debug.LogError("SparkingElectricFence: Not enough sprites to animate");
			Debug.Break();
			return;
		}
		else
		{
			timer += Time.deltaTime;
			if(timer >= 1.0f / fps)
			{
				timer = 0.0f;
				for(int i = 0; i < rends.Length; i++)
				{
					if(allRandom)
					{
						int random = 0;
						do
						{
							random = Random.Range(0, spriteAmount);
						}
						while(random == spritesSelected[i]);
		
						spritesSelected[i] = random;
					}
					else
					{
						spritesSelected[i] = (spritesSelected[i] + 1) % spriteAmount;
					}

					rends[i].material.mainTextureOffset = new Vector2(0.0f, spritesSelected[i] * spriteOffset);
				}
			}
		}
	}
}
