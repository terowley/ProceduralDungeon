using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReachGoal : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "Player") {
			GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
			if (enemies.Length == 0)
            {
                EndGame();
            
			}
        }
	}

	public static void EndGame()
    {
		Debug.Log("in end game");
		
		GameObject dungeon = GameObject.FindGameObjectWithTag("Dungeon");
		DungeonGeneration dungeonGeneration = dungeon.GetComponent<DungeonGeneration>();
		dungeonGeneration.ResetDungeon();

		// TODO add a cooroutine to put up an end of game UI poster
		SceneManager.LoadScene("Demo");
	}
}
