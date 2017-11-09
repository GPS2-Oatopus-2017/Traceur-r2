using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasuringScript : MonoBehaviour {
	Vector3 BoundingBox;

	private static Bounds GetTotalMeshFilterBounds(Transform objectTransform)
	{
		var meshFilter = objectTransform.GetComponent<MeshFilter>();
		var result = meshFilter != null ? meshFilter.mesh.bounds : new Bounds();

		foreach (Transform transform in objectTransform)
		{
			var bounds = GetTotalMeshFilterBounds(transform);
			result.Encapsulate(bounds.min);
			result.Encapsulate(bounds.max);
		}
		var scaledMin = result.min;
		scaledMin.Scale(objectTransform.localScale);
		result.min = scaledMin;
		var scaledMax = result.max;
		scaledMax.Scale(objectTransform.localScale);
		result.max = scaledMax;
		Debug.Log(result);
		return result;

	}

	// Use this for initialization
	void Start () 
	{
		GetTotalMeshFilterBounds(gameObject.transform);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
