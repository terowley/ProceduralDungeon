using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class EnterDoor : MonoBehaviour
{
    [SerializeField]
    string direction;

    
    void OnCollisionEnter2D(Collision2D col)
    {
        //Debug.Log(" in collision");
        
        
        if (col.gameObject.tag == "Player")
        {
            
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            //Debug.Log(" NO ENIMIES " + enemies.Length);
            if (enemies.Length == 0)
            {
                
                GameObject dungeon = GameObject.FindGameObjectWithTag("Dungeon");
                DungeonGeneration dungeonGeneration = dungeon.GetComponent<DungeonGeneration>();
                Room room = dungeonGeneration.CurrentRoom();
                PlayerMovement.heading = this.direction;
                //Debug.Log("col heading " + heading);

                dungeonGeneration.MoveToRoom(room.Neighbor(this.direction));// was this.direction
                SceneManager.LoadScene("Demo");
            }
        }
    }

    /*
    void OnDisable()
    {
        //GameObject player = GameObject.FindGameObjectWithTag("Player");
        //PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        // EnterDoor door = playerMovement.EnterDoor;
        // string heading = door.heading;

        // GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
        //Debug.Log("in OnDisable");
        PlayerPrefs.SetInt("health", PlayerMovement.currentHealth);
        PlayerPrefs.SetString("heading", heading);

    }
    */
}
