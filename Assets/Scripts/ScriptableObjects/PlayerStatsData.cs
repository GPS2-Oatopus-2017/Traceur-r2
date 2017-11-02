using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerStatsData : ScriptableObject 
{
	public float maxHealth;
	public float movementSpeed;
	public float observeTime;
	public float cameraTUurnSpeed;
	public float turnThreshold;
}
