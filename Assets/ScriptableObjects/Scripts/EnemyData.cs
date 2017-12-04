using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject 
{
	public float movementSpeed;
	public float turnSpeed;
	public float attackCount;
	public float attackSpeed;
	public float taserSlow;
	public float taserSlowDuration;
	public float damage;
	public float alertDistance;
	public float safeDistance;
	public int spawnHDAmount;
	public float increaseReputation;
	public float timeForTopOrBottom;
	public float keptDistance;
	public float atkIndicatorOffset;
}
