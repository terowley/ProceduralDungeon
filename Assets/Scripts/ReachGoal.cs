using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReachGoal : MonoBehaviour
{

	GameObject winImage;

	private void Start()
    {
		winImage = GameObject.Find("WinImage");
	}
    

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player")
		{
			GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
			if (enemies.Length == 0)
			{
				winImage.SetActive(true);
				Invoke("EndOfGame", 3);

			}
		}
	}


	void EndOfGame()
	{
		//Test if in editor or play mode
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
		Application.Quit();

		winImage.SetActive(false);
	}

}