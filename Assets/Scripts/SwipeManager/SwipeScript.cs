using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwipeDirection
{
	None = -1,

	Up = 0,
	Down,
	Left,
	Right,

	Total
}

public class SwipeScript : MonoBehaviour
{
	private static SwipeScript mInstance;
	public static SwipeScript Instance
	{
		get { return mInstance; }
	}

	[Header("State (Read Only)")]
	public SwipeDirection swipeDirection = SwipeDirection.None;

	private float fingerStartTime = 0.0f;
	private Vector2 fingerStartPos = Vector2.zero;
	private bool isSwipe = false;
	private bool swipeDown = false;

	[Header("Settings")]
	public float minSwipeDist = 100.0f;
	public float maxSwipeTime = 0.5f;

	void Awake()
	{
		if(mInstance == null) mInstance = this;
		else if(mInstance != this) Destroy(this.gameObject);
	}

	// Use this for initialization
	void Start ()
	{
		swipeDirection = SwipeDirection.None;
	}

	// Update is called once per frame
	void Update ()
	{
		#if UNITY_ANDROID && !UNITY_EDITOR

		swipeDown = false; //Resets the swiping

		if(Input.touchCount <= 0) return;

		Touch[] touches = Input.touches;

		for(int i = 0; i < Input.touchCount; i++)
		{
			switch(touches[i].phase)
			{
				case TouchPhase.Began:
					if(!isSwipe)
					{
						isSwipe = true;
						fingerStartTime = Time.time;
						fingerStartPos = touches[i].position;
						swipeDirection = SwipeDirection.None;
					}
					break;

				case TouchPhase.Canceled:
					if(isSwipe)
					{
						isSwipe = false;
					}
					break;

				case TouchPhase.Ended :
					float gestureTime = Time.time - fingerStartTime;
					float gestureDist = Vector2.Distance(touches[i].position, fingerStartPos);

					if(isSwipe && gestureTime < maxSwipeTime && gestureDist > minSwipeDist)
					{
						Vector2 direction = touches[i].position - fingerStartPos;
						Vector2 swipeType = Vector2.zero;

						if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
						{
							//the swipe is horizontal:
							swipeType = Vector2.right * Mathf.Sign(direction.x);
						}
						else
						{
							//the sipe is vertical:
							swipeType = Vector2.up * Mathf.Sign(direction.y);
						}

						if(swipeType.x != 0.0f)
						{
							if(swipeType.x > 0.0f)
							{
								swipeDirection = SwipeDirection.Right;
							}
							else
							{
								swipeDirection = SwipeDirection.Left;
							}

							swipeDown = true;
							isSwipe = false;
						}
						else if(swipeType.y != 0.0f)
						{
							if(swipeType.y > 0.0f)
							{
								swipeDirection = SwipeDirection.Up;
							}
							else
							{
								swipeDirection = SwipeDirection.Down;
							}

							swipeDown = true;
							isSwipe = false;
						}
					}
					else
					{
						isSwipe = false;
					}
					break;
			}
		}

		#elif UNITY_EDITOR

		swipeDown = false; //Resets the swiping
		Vector2 touchPos = Input.mousePosition;

		if(Input.GetMouseButtonDown(0))
		{
			if(!isSwipe)
			{
				isSwipe = true;
				fingerStartTime = Time.time;
				fingerStartPos = touchPos;
				swipeDirection = SwipeDirection.None;
			}
		}
		else if(Input.GetMouseButtonUp(0))
		{
			float gestureTime = Time.time - fingerStartTime;
			float gestureDist = Vector2.Distance(touchPos, fingerStartPos);

			if(isSwipe && gestureTime < maxSwipeTime && gestureDist > minSwipeDist)
			{
				Vector2 direction = touchPos - fingerStartPos;
				Vector2 swipeType = Vector2.zero;

				if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
				{
					//the swipe is horizontal:
					swipeType = Vector2.right * Mathf.Sign(direction.x);
				}
				else
				{
					//the sipe is vertical:
					swipeType = Vector2.up * Mathf.Sign(direction.y);
				}

				if(swipeType.x != 0.0f)
				{
					if(swipeType.x > 0.0f)
					{
						swipeDirection = SwipeDirection.Right;
					}
					else
					{
						swipeDirection = SwipeDirection.Left;
					}

					swipeDown = true;
					isSwipe = false;
				}
				else if(swipeType.y != 0.0f)
				{
					if(swipeType.y > 0.0f)
					{
						swipeDirection = SwipeDirection.Up;
					}
					else
					{
						swipeDirection = SwipeDirection.Down;
					}

					swipeDown = true;
					isSwipe = false;
				}
			}
			else
			{
				isSwipe = false;
			}
		}

		#endif
	}

	public SwipeDirection GetSwipe()
	{
		if(!swipeDown) return SwipeDirection.None;
		else return swipeDirection;
	}
}