using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateMe : MonoBehaviour 
{       
    public GameObject dirIndicator;
	public GameObject text;
    public GameObject player;
    public float distanceFromObject;


	// Use this for initialization
	void Start () 
    {
        dirIndicator.SetActive(false);
		if(text != null)
		{
			text.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(Vector3.Distance(transform.position, player.transform.position) <= distanceFromObject)
        {
            dirIndicator.SetActive(true);
			if(text != null)
			{
				text.SetActive(true);
			}
        }
	}
}
