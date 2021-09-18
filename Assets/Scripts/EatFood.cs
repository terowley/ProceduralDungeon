using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatFood : MonoBehaviour {
	
	void OnTriggerEnter2D(Collider2D col)
	{
		//GameObject player = GameObject.FindGameObjectWithTag("Player");
		//PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
	//	int health = PlayerMovement.currentHealth;

		if (col.gameObject.tag == "Player")
		{
			PlayerMovement.currentHealth += 20;
			Destroy(this.gameObject);
		}
	}
}

