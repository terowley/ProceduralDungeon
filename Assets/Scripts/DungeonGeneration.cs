using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGeneration : MonoBehaviour {

	[SerializeField]
	private int numberOfRooms;

	[SerializeField]
	private int numberOfObstacles;
	[SerializeField]
	private Vector2Int[] possibleObstacleSizes;

	[SerializeField]
	private int numberOfEnemies;
	[SerializeField]
	private GameObject[] possibleEnemies;

	[SerializeField]
	private GameObject goalPrefab;

	[SerializeField]
	private TileBase obstacleTile;

	private Room[,] rooms;

	List<Room> deadEnds = new List<Room>();

	private Room currentRoom;

	private static DungeonGeneration instance = null;

	private bool firstRoom = true;

	void Awake () {
		if (instance == null) {
			DontDestroyOnLoad (this.gameObject);
			instance = this;
			this.currentRoom = GenerateDungeon ();
			
		} else {
			string roomPrefabName = instance.currentRoom.PrefabName ();
			GameObject roomObject = (GameObject) Instantiate (Resources.Load (roomPrefabName));
			Tilemap tilemap = roomObject.GetComponentInChildren<Tilemap> ();

			instance.currentRoom.AddPopulationToTilemap (tilemap, instance.obstacleTile);
			Destroy (this.gameObject);
		}
	}

	void Start () {
		string roomPrefabName = this.currentRoom.PrefabName ();
		GameObject roomObject = (GameObject) Instantiate (Resources.Load (roomPrefabName));
		Tilemap tilemap = roomObject.GetComponentInChildren<Tilemap> ();

		this.currentRoom.AddPopulationToTilemap (tilemap, this.obstacleTile);
		
	}

	private Room GenerateDungeon() {
		int gridSize = 3 * numberOfRooms;

		rooms = new Room[gridSize, gridSize];

		Vector2Int initialRoomCoordinate = new Vector2Int ((gridSize / 2) - 1, (gridSize / 2) - 1);

		Queue<Room> roomsToCreate = new Queue<Room> ();
		roomsToCreate.Enqueue (new Room(initialRoomCoordinate.x, initialRoomCoordinate.y));
		List<Room> createdRooms = new List<Room> ();
		int room_cnt = 0;
		while (roomsToCreate.Count > 0 && createdRooms.Count < numberOfRooms) {
			Room currentRoom = roomsToCreate.Dequeue ();
			this.rooms [currentRoom.roomCoordinate.x, currentRoom.roomCoordinate.y] = currentRoom;
			createdRooms.Add (currentRoom);
			room_cnt += 1;
			
			AddNeighbors (currentRoom, roomsToCreate);
		}
		// Dead End Room (stores, quests, etc)
		Debug.Log(" No of Dead Ends =" + deadEnds.Count);

		
		int maximumDistanceToInitialRoom = 0;
		Room finalRoom = null;
		
		// connect all the room with apropriate neighbors room
		foreach (Room room in createdRooms) {
			List<Vector2Int> neighborCoordinates = room.NeighborCoordinates ();
			foreach (Vector2Int coordinate in neighborCoordinates) {
				Room neighbor = this.rooms [coordinate.x, coordinate.y];
				if (neighbor != null) {
					room.Connect (neighbor);
				}
			}

			// populate Obstacle (tiles) and Prefabs ( enimies and NPC)
			room.PopulateObstacles (this.numberOfObstacles, this.possibleObstacleSizes);
			room.PopulatePrefabs (this.numberOfEnemies, this.possibleEnemies);


			// code to find Goal Room - most distant room
			int distanceToInitialRoom = Mathf.Abs (room.roomCoordinate.x - initialRoomCoordinate.x) + Mathf.Abs(room.roomCoordinate.y - initialRoomCoordinate.y);
			if (distanceToInitialRoom > maximumDistanceToInitialRoom) {
				maximumDistanceToInitialRoom = distanceToInitialRoom;
				finalRoom = room;
			}
		}


		GameObject[] goalPrefabs = { this.goalPrefab };
		finalRoom.PopulatePrefabs(1, goalPrefabs);

		return this.rooms [initialRoomCoordinate.x, initialRoomCoordinate.y];
	}

	private void AddNeighbors(Room currentRoom, Queue<Room> roomsToCreate) {
		
		List<Vector2Int> neighborCoordinates = currentRoom.NeighborCoordinates ();
		List<Vector2Int> availableNeighbors = new List<Vector2Int> ();
		
		foreach (Vector2Int coordinate in neighborCoordinates) {
		
			
			if (this.rooms[coordinate.x, coordinate.y] == null) {
				availableNeighbors.Add (coordinate);
			}
		}
		
		if (availableNeighbors.Count >0 ){ // ADDED TO FIX BUG IN ORIGINAL CODE
			
			int numberOfNeighbors = (int)Random.Range(1, availableNeighbors.Count);
			// MOD TO MAKE INITIAL ROOM ALWAYS HAVE 4 NEIGHBORS
			if (firstRoom) { numberOfNeighbors = 4; firstRoom = false; }
			if (numberOfNeighbors == 1) { deadEnds.Add(currentRoom); }
			for (int neighborIndex = 0; neighborIndex < numberOfNeighbors; neighborIndex++)
			{
				float randomNumber = Random.value;
				float roomFrac = 1f / (float)availableNeighbors.Count;
				Vector2Int chosenNeighbor = new Vector2Int(0, 0);
				foreach (Vector2Int coordinate in availableNeighbors)
				{
					if (randomNumber < roomFrac)
					{
						chosenNeighbor = coordinate;
						break;
					}
					else
					{
						roomFrac += 1f / (float)availableNeighbors.Count;
					}
				}
				
				roomsToCreate.Enqueue(new Room(chosenNeighbor));
				availableNeighbors.Remove(chosenNeighbor);
			}	


		}// END OF NO NEIGHBORS TEST
	}

	private void PrintGrid() {
		for (int rowIndex = 0; rowIndex < rooms.GetLength (1); rowIndex++) {
			string row = "";
			for (int columnIndex = 0; columnIndex < rooms.GetLength (0); columnIndex++) {
				if (rooms [columnIndex, rowIndex] == null) {
					row += "X";
				} else {
					row += "R";
				}
			}
			Debug.Log (row);
		}
	}

	public void MoveToRoom(Room room) {
		this.currentRoom = room;
	}

	public Room CurrentRoom() {
		return this.currentRoom;
	}

	public void ResetDungeon() {
		this.currentRoom = GenerateDungeon ();
	}
		
}
