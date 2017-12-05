using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreObserver : MonoBehaviour
{
	private	Text m_Text;

	// Use this for initialization
	void Start ()
	{
		m_Text = GetComponent<Text>();
		m_Text.text = ScoreDataHolder.Instance.lastScore.ToString();
	}
}
