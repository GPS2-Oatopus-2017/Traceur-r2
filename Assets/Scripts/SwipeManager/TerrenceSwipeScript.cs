using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum _SwipeDirection
{
	NONE = 0,

	UP = 1,
	DOWN = 2,
	LEFT = 4,
	RIGHT = 8,

	//UPLEFT = 5,
	//UPRIGHT = 9,
	//DOWNLEFT = 6,
	//DOWNRIGHT = 10,

	TOTAL,
}

public class TerrenceSwipeScript : MonoBehaviour
{

	public static TerrenceSwipeScript instance;

	public _SwipeDirection Direction { set; get; }

	private Vector3 touchPosition;

	public float swipeThresholdX = 50.0f;
	public float swipeThresholdY = 50.0f;

	float startSwipeTime = 0.0f;
	public float maxSwipeTime = 0.5f;

	void Awake ()
	{

		if (instance == null) {
			instance = this;
		} else {
			Destroy (gameObject);
			return;
		}

	}

	void Start ()
	{
		
	}

	void Update ()
	{
		CheckSwipe ();
	}

	void CheckSwipe ()
	{

		Direction = _SwipeDirection.NONE;

		if (Input.GetMouseButtonDown (0)) {

			touchPosition = Input.mousePosition;

			startSwipeTime = Time.time;

		}

		if (Input.GetMouseButtonUp (0)) {

			Vector2 deltaSwipe = touchPosition - Input.mousePosition;

			float totalSwipeTime = Time.time - startSwipeTime;

			if (Mathf.Abs (deltaSwipe.x) > swipeThresholdX && totalSwipeTime < maxSwipeTime) {

				//Swipe on the x-axis. //

				Direction |= (deltaSwipe.x < 0) ? _SwipeDirection.RIGHT : _SwipeDirection.LEFT;

				Debug.Log ("TERRENCE SWIPE X!");

			}
			if (Mathf.Abs (deltaSwipe.y) > swipeThresholdY && totalSwipeTime < maxSwipeTime) {

				//Swipe on the y-axis. //

				Direction |= (deltaSwipe.y < 0) ? _SwipeDirection.UP : _SwipeDirection.DOWN;

				Debug.Log ("TERRENCE SWIPE Y!");
			}
		}
	}

	public bool IsSwiping (_SwipeDirection aDirection)
	{
		//return (Direction) == aDirection;
		return (Direction & aDirection) == aDirection;
	}﻿
}
