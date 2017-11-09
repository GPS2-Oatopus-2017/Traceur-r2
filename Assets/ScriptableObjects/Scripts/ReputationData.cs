using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ReputationData : ScriptableObject
{
	public int maxReputation;
	public float timeToDecrease;
	public int decreaseValue;
	public int increaseValue;

	public int[] deadSDCountList = new int[5];
	public int[] deadHDCountList = new int[5];
}