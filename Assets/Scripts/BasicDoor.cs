using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BasicDoor : MonoBehaviour
{
    [SerializeField]
    string direction;
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            GameObject dungeon = GameObject.FindGameObjectWithTag("Dungeon");
            DungeonGeneration dungeonGeneration = dungeon.GetComponent<DungeonGeneration>();
            Room room = dungeonGeneration.CurrentRoom();
            dungeonGeneration.MoveToRoom(room.Neighbor(this.direction));
            SceneManager.LoadScene("Demo");
        }
    }
}