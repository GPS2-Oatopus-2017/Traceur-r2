using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LineScript : MonoBehaviour
{
	public enum MotionDetectorState
	{
		Normal,
		Alerted,
		Deactivated
	}

	private LineRenderer lRend;

	public MotionDetectorState state;

	public Gradient normalColor;
	public Gradient alertColor;
	public Gradient deactivateColor;

	// Use this for initialization
	void Start ()
	{
		lRend = GetComponent<LineRenderer>();
	}

	// Update is called once per frame
	void Update ()
	{
		switch(state)
		{
			case MotionDetectorState.Normal:
				lRend.colorGradient = normalColor;
				break;
			case MotionDetectorState.Alerted:
				lRend.colorGradient = alertColor;
				break;
			case MotionDetectorState.Deactivated:
				lRend.colorGradient = deactivateColor;
				break;
		}
	}
}
