using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour {

	public PlayerStatsData data;
	public Sprite barFilled;
	public Image[] healthBars;

	private Image[] tempHealthBars;


	void Awake()
	{
	}

	// Use this for initialization
	void Start () 
	{
		tempHealthBars = gameObject.GetComponentsInChildren<Image>();
		healthBars = tempHealthBars;

		for(int i = 1; i < tempHealthBars.Length; i++)
		{
			if(tempHealthBars[i].name == "Health (0)")
			{
				healthBars[0] = tempHealthBars[i];
			}
			else if(tempHealthBars[i].name == "Health (1)")
			{
				healthBars[1] = tempHealthBars[i];
			}
			else if(tempHealthBars[i].name == "Health (2)")
			{
				healthBars[2] = tempHealthBars[i];
			}

			healthBars[i].sprite = barFilled;
		}
	}

	public void ResetHealthBar(int playerHealth)
	{
		healthBars[playerHealth].enabled = false;
	}

	void Update()
	{
		for(int i=0; i < data.maxHealth; i++)
		{
			if(GameManagerScript.Instance.player.status.currentHealth > i)
			{
				healthBars[i].enabled = true;
			}
			else
			{
				healthBars[i].enabled = false;
			}
		}
	}
}
