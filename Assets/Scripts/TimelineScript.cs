 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineScript : MonoBehaviour {

	public static TimelineScript Instance;

	public GameObject character;
	public GameObject surveillanceDrone;
	public GameObject huntingDrone;
	public int SDCount = 0;
	public int HDCount = 0;
	public float enemySpawnPointOffsetA;
	public float enemySpawnPointOffsetB;
	private Vector3 startPoint;
	private Vector3 endPoint;
	private float distance;

	//temporary variables
	[Range(0.0f, 100.0f)]
	public float characterTimeline;
	[Range(0.0f, 100.0f)]
	public float enemyTimelineA;
	[Range(0.0f, 100.0f)]
	public float enemyTimelineB;

	public float characterDistanceTraveled;
	public float enemyDistanceTraveledA;
	public float enemyDistanceTraveledB;

	public float calculatedDistanceAwayFromEnd;

	public float speed;
	public bool createEnemyA;
	public bool createEnemyB;
	public bool destroyEnemyA;
	public bool destroyEnemyB;

	void Awake()
	{
		Instance = this;
	}

	// Use this for initialization
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () 
	{
		character = GameObject.Find("Character");
		surveillanceDrone = GameObject.Find("SurveillanceDrone");
		huntingDrone = GameObject.Find("HuntingDrone");

		startPoint = transform.Find("StartPoint").position;
		endPoint = transform.Find("EndPoint").position;

		character.transform.position = startPoint;
		surveillanceDrone.transform.position = startPoint;
		huntingDrone.transform.position = startPoint;

		calculatedDistanceAwayFromEnd = WaypointDistanceScript.Instance.totalDistance;

		distance = endPoint.x - startPoint.x;

		characterDistanceTraveled = 0;
		enemyDistanceTraveledA = 0;
		enemyDistanceTraveledB = 0;

		surveillanceDrone.SetActive(false);
		huntingDrone.SetActive(false);

		//temporary assignment of variables
		createEnemyA = false;
		createEnemyA = false;
		destroyEnemyA = false;
		destroyEnemyB = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		calculatedDistanceAwayFromEnd = WaypointDistanceScript.Instance.totalDistance;
		characterDistanceTraveled = WaypointDistanceScript.Instance.playerDistanceTraveled;
		//temporary character running simulation
//		if(characterDistanceTraveled >= calculatedDistanceAwayFromEnd)
//		{
//			characterDistanceTraveled = calculatedDistanceAwayFromEnd;
//		}
//		else
//		{
//			characterDistanceTraveled += Time.deltaTime * speed;
//		}

		//Calculate character timeline from total distance
		characterTimeline = (characterDistanceTraveled/ calculatedDistanceAwayFromEnd) * 100;
		character.transform.position = new Vector3(startPoint.x + CalculateDistance(characterTimeline), startPoint.y, startPoint.z);

		//Calculate enemies timelines from total distance km
		if(surveillanceDrone.activeSelf)
		{
			if(enemyDistanceTraveledA >= calculatedDistanceAwayFromEnd)
			{
				enemyDistanceTraveledA = calculatedDistanceAwayFromEnd;
			}
			else
			{
				enemyDistanceTraveledA += Time.deltaTime * speed;
			}
			enemyTimelineA = (enemyDistanceTraveledA/ calculatedDistanceAwayFromEnd) * 100;
			surveillanceDrone.transform.position = new Vector3(startPoint.x + CalculateDistance(enemyTimelineA), startPoint.y, startPoint.z);
		}
		if(huntingDrone.activeSelf)
		{
			if(enemyDistanceTraveledB >= calculatedDistanceAwayFromEnd)
			{
				enemyDistanceTraveledB = calculatedDistanceAwayFromEnd;
			}
			else
			{
				enemyDistanceTraveledB += Time.deltaTime * speed;
			}
			enemyTimelineB = (enemyDistanceTraveledB/ calculatedDistanceAwayFromEnd) * 100;
			huntingDrone.transform.position = new Vector3(startPoint.x + CalculateDistance(enemyTimelineB), startPoint.y, startPoint.z);
		}

		//temporary spawning enemy
		if(createEnemyA)
		{
			CreateEnemyIcon("Surveillance_Drone", 1);
			createEnemyA = false;
		}
		if(destroyEnemyA)
		{
			DestroyEnemyIcon("Surveillance_Drone", 1);
			destroyEnemyA = false;
		}
		if(createEnemyB)
		{
			CreateEnemyIcon("Hunting drone", 1);
			createEnemyB = false;
		}
		if(destroyEnemyB)
		{
			DestroyEnemyIcon("Hunting drone", 1);
			destroyEnemyB = false;
		}
	}

	float CalculateDistance(float timeline)
	{
		return (timeline/100) * distance;
	}

	//when enemy spawn call this function to show enemy icons
	public void CreateEnemyIcon(string enemy, int num)
	{
		GameObject enemyIcon = null;
		if(enemy == "Surveillance_Drone") enemyIcon = surveillanceDrone;
		else if(enemy == "Hunting_Droid") enemyIcon = huntingDrone;

		if(!enemyIcon.activeSelf)
		{
			Vector3 charPos = character.transform.position;

			if(enemyIcon == surveillanceDrone)
			{
				SDCount += num;
				surveillanceDrone.GetComponentInChildren<Text>().text = SDCount.ToString();
				enemyDistanceTraveledA = characterDistanceTraveled - enemySpawnPointOffsetA;
				enemyTimelineA = (enemyDistanceTraveledA/ calculatedDistanceAwayFromEnd) * 100;
				surveillanceDrone.transform.position = new Vector3(startPoint.x + CalculateDistance(enemyTimelineA), startPoint.y, startPoint.z);
			}
			else if(enemyIcon == huntingDrone)
			{
				HDCount += num;
				huntingDrone.GetComponentInChildren<Text>().text = HDCount.ToString();
				enemyDistanceTraveledB = characterDistanceTraveled - enemySpawnPointOffsetB;
				enemyTimelineB = (enemyDistanceTraveledB/ calculatedDistanceAwayFromEnd) * 100;
				huntingDrone.transform.position = new Vector3(startPoint.x + CalculateDistance(enemyTimelineB), startPoint.y, startPoint.z);
			}

			enemyIcon.SetActive(true);
		}
		else
		{
			if(enemyIcon == surveillanceDrone)
			{
				SDCount += num;
				surveillanceDrone.GetComponentInChildren<Text>().text = SDCount.ToString();
			}
			else if(enemyIcon == huntingDrone)
			{
				HDCount += num;
				huntingDrone.GetComponentInChildren<Text>().text = HDCount.ToString();
			}
		}
	}

	public void DestroyEnemyIcon(string enemy, int num)
	{
		GameObject enemyIcon = null;
		if(enemy == "Surveillance_Drone") enemyIcon = surveillanceDrone;
		else if(enemy == "Hunting drone") enemyIcon = huntingDrone;

		if(enemyIcon == surveillanceDrone)
		{
			if(SDCount >= 1)
			{
				SDCount -= num;
				surveillanceDrone.GetComponentInChildren<Text>().text = SDCount.ToString();
			}

			if(SDCount <= 0)
			{
				if(surveillanceDrone.activeSelf)
				{
					surveillanceDrone.SetActive(false);
				}
			}
		}
		else if(enemyIcon == huntingDrone)
		{
			if(HDCount >= 1)
			{
				HDCount -= num;
				huntingDrone.GetComponentInChildren<Text>().text = HDCount.ToString();
			}

			if(HDCount <= 0)
			{
				if(huntingDrone.activeSelf)
				{
					huntingDrone.SetActive(false);
				}
			}
		}
	}
}
