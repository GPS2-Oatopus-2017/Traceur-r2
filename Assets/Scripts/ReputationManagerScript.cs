using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReputationManagerScript : MonoBehaviour {

	private static ReputationManagerScript mInstance;
	public static ReputationManagerScript Instance
	{
		get { return mInstance; }
	}

	void Awake()
	{
		if(mInstance == null) mInstance = this;
		else if(mInstance != this) Destroy(this.gameObject);
	}

	public ReputationData reputation_Data;

	public int currentRep;
	public int lastRep;

	public int deadSD;
	public int deadHD;

	public int deadSDMax;
	public int deadHDMax;

	public float resetCounter;

	public Text enemyAmountText;
	public Text playerStatus;
	string status;
	public Sprite lightUp;
	public List<Image> starList = new List<Image>();
	public List<Image> statusList = new List<Image>();

	// Use this for initialization
	void Start () {
		//maxRep = starList.Count;
		//currentRep = maxRep;
	}
	
	// Update is called once per frame
	void Update () {
		if(resetCounter >= reputation_Data.timeToDecrease)
		{
			DecreaseReputation();
			resetCounter = 0;
		}
		UpdateBar();
		UpdateStatus();
		UpdateDeadDrones();
	}
		

	public void IncreaseReputation()
	{
		currentRep += reputation_Data.increaseValue;
		resetCounter = 0;
		if(currentRep > reputation_Data.maxReputation)
		{
			currentRep = reputation_Data.maxReputation;
		}
	}

	void DecreaseReputation()
	{
		currentRep -= reputation_Data.decreaseValue;
		if(currentRep <= 0)
		{
			currentRep = 0;
		}
	}

	void UpdateBar()
	{
		for(int i=0; i<starList.Count; i++)
		{
			if(i < currentRep)
			{
				starList[i].enabled = true;
			}
			else
			{
				starList[i].enabled = false;
			}
		}
	}

//	void UpdateCount() //need to change to sd and hd
//	{
		//displayECount = "SD Count: " + SpawnManagerScript.Instance.sdCount + "\nHD Count: "+ SpawnManagerScript.Instance.hdCount;
		//enemyAmountText.text = displayECount;
//	}

	void UpdateStatus()
	{
		for(int i = 0; i <= reputation_Data.maxReputation; i++)
		{
			if(currentRep == i)
			{
				statusList[i].enabled = true;
			}
			else
			{
				statusList[i].enabled = false;
			}
		}
	}

	void UpdateDeadDrones()
	{
		switch(currentRep)
		{
			case 1:
				deadSDMax = reputation_Data.deadSDCountList[0];
				deadHDMax = reputation_Data.deadHDCountList[0];
				break;
			case 2:
				deadSDMax = reputation_Data.deadSDCountList[1];
				deadHDMax = reputation_Data.deadHDCountList[1];
				break;
			case 3:
				deadSDMax = reputation_Data.deadSDCountList[2];
				deadHDMax = reputation_Data.deadHDCountList[2];
				break;
			case 4:
				deadSDMax = reputation_Data.deadSDCountList[3];
				deadHDMax = reputation_Data.deadHDCountList[3];
				break;
			default:
				deadSDMax = -1;
				deadHDMax = -1;
				break;
		}

		if(deadSDMax > 0)
		{
			if(deadSD >= deadSDMax)
			{
				deadSD = 0;
				currentRep++;
			}
		}

		if(deadHDMax > 0)
		{
			if(deadHD >= deadHDMax)
			{
				deadHD = 0;
				currentRep++;
			}
		}
	}

	void LateUpdate()
	{
		if(!GameManagerScript.Instance.player.status.isAlive) return;

		if(currentRep == lastRep && currentRep != 0)
		{
			resetCounter += Time.deltaTime;
		}
		if(lastRep!= currentRep)
		{
			resetCounter = 0;
		}
		lastRep = currentRep;
	}
}
