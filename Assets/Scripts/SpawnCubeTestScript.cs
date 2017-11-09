using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCubeTestScript : MonoBehaviour
{
	public GameObject cubePrefab;
	public Vector3 positionA;
	public Vector3 positionB;
	public int amount;
	public List<GameObject> spawnedCubes;

	// Use this for initialization
	void Start ()
	{
		spawnedCubes = new List<GameObject>();
		for(int i=0; i<amount;i++)
		{
			float x = Random.Range(positionA.x,positionB.x);
			float y = Random.Range(positionA.y,positionB.y);
			float z = Random.Range(positionA.z,positionB.z);
			GameObject obj = Instantiate(cubePrefab, new Vector3(x,y,z), Quaternion.identity);
			spawnedCubes.Add(obj);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
