using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpawnData : ScriptableObject 
{
	public float spawnDistance;
	public float spawnTime;
	public int[] spawnSDCount = new int[5];
	public int[] spawnHDCount = new int[5];
}
