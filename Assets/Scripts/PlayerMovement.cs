using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour {

	//[SerializeField]
	public float speed;
	public int maxHealth = 200;
	
	private Rigidbody2D rb2D;

	public HealthBar  healthBar;
	
	public static string heading;
	public static int currentHealth = 100;

	void Start () {
		//currentHealth = 100;
		healthBar.SetMaxHealth(200);
		healthBar.SetHealth(currentHealth);

		//Get a component reference to this object's Rigidbody2D
		//rb2D = GetComponent<Rigidbody2D>();

		StartCoroutine(FoodCheck());
	}

	// Update is called once per frame
	void FixedUpdate () {
		Vector2 start = transform.position; // store start position

		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");
		
		GetComponent<Rigidbody2D>().velocity = new Vector2 (horizontal * speed, vertical * speed);
		//Vector2 new_position = transform.position;

	}


	
		IEnumerator FoodCheck()
		{
			while (true)
			{
				TakeDamage(1);

				//Debug.Log("Health is " + currentHealth);

				yield return new WaitForSeconds(1f); //wait 3 seconds

			}


		}
		void TakeDamage(int damage)
		{

			currentHealth -= damage;
			healthBar.SetHealth(currentHealth);
			if (currentHealth < 0)
			{
			//This is wrong - need class for starve death, GUI poster and end game

			//ReachGoal.EndGame();

			Debug.Log("STARVED!!");
			}

		}

	/*
		void OnEnable()
		{
			//Debug.Log("in OnEnable");
			currentHealth = PlayerPrefs.GetInt("health");
			
		}
	*/	
}
