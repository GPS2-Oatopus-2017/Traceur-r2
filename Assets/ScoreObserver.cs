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
		if(ScoreManagerScript.Instance)
		{
			m_Text.text = ScoreManagerScript.Instance.finalScore.ToString();
			Destroy(ScoreManagerScript.Instance.gameObject);
		}
		else
		{
			Debug.LogWarning("ScoreObserver: ScoreManagerScript is not found!");
		}
	}
}
