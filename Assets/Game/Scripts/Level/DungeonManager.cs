using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

public enum RoomType
{
    None = -1,
    Start = 0,
    Boss = 1,
    Fighting = 2,
    Upgrade = 3,
    Treasure = 4,
    Branch = 5,
}

public enum Direction
{
    North = 0,
    East = 1,
    South = 2,
    West = 3,
    Invalid = 4,
}

public class RoomNode
{
    public RoomType type;
    public RoomNode[] nextRooms = new RoomNode[4]; // Array holding references to adjacent rooms (North, East, South, West)
    public Vector2Int position; // Position of the room in the grid
    public bool IsCleared = false;

    public RoomNode(RoomType type, Vector2Int position)
    {
        this.type = type; 
        this.position = position;
    }
}

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance;

    [SerializeField] private int _maxMainRooms; // Maximum number of main rooms to generate

    private RoomNode[,] _roomGrid; // 2D array representing the grid of rooms
    private DungeonTraversalManager _dungeonManager;
    private bool _treasureRoomSpawned = false;
    public RoomNode CurrentPlayerLocation;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _dungeonManager = DungeonTraversalManager.Instance;
        CreateDungeon();
    }

    public void CreateDungeon()
    {
        _treasureRoomSpawned = false;
        _maxMainRooms = LevelManager.Instance.GetLevelData().NumRooms;
        CreateMainRooms();
        CreateBranchRooms();
        _dungeonManager.InitializeTraversal();
    }

    private void CreateMainRooms()
    {
        RoomType[] mainRoomTypes = { RoomType.Fighting, RoomType.Treasure }; // Types of main rooms

        RoomNode startingRoom = new RoomNode(RoomType.Start, new Vector2Int(0, 0)); // Create the starting room at position (0, 0)

        _roomGrid = new RoomNode[100, 100]; // Initialize the room grid with a fixed size
        AddRoomToGrid(startingRoom); // Add the starting room to the grid

        CurrentPlayerLocation = startingRoom; // Set the current player location to the starting room

        RoomNode previousRoom = startingRoom; // Track the previous room while generating rooms
        for (int i = 1; i <= _maxMainRooms + 1; i++) // Iterate to create main rooms
        {
            RoomType currentRoomType = RoomType.None;
            if (i <= _maxMainRooms)
            {
                if (!_treasureRoomSpawned)
                {
                    currentRoomType = (i < _maxMainRooms) ? mainRoomTypes[Random.Range(0, mainRoomTypes.Length)] : RoomType.Upgrade; // Determine the type of the current room (main or upgrade)
                    if (currentRoomType == RoomType.Treasure)
                    {
                        _treasureRoomSpawned = true;
                    }
                }
                else
                {
                    currentRoomType = (i < _maxMainRooms) ? mainRoomTypes[0] : RoomType.Upgrade;
                }
            }
            else
            {
                currentRoomType = RoomType.Boss;
            }


            List<Direction> availableDirections = new List<Direction> { Direction.North, Direction.East, Direction.South, Direction.West }; // List of available directions for branching
            if (i > 1)
            {
                Direction directionBack = GetOppositeDirection(previousRoom.position, startingRoom.position); // Get the direction back to the starting room
                availableDirections.Remove(directionBack); // Remove the back direction from available directions
            }

            for (int j = availableDirections.Count - 1; j >= 0; j--) // Iterate through available directions
            {
                Direction direction = availableDirections[j]; // Get the current direction
                Vector2Int nextPosition = previousRoom.position + DirectionToVector(direction); // Calculate the position of the next room in the specified direction
                if (IsRoomOccupied(nextPosition)) // Check if the next room position is occupied
                {
                    availableDirections.RemoveAt(j); // Remove the direction if the room is occupied
                }
            }

            if (availableDirections.Count > 0) // If there are available directions to add a room
            {
                Direction randomDirection = availableDirections[Random.Range(0, availableDirections.Count)]; // Choose a random direction from available directions

                Vector2Int currentPosition = previousRoom.position + DirectionToVector(randomDirection); // Calculate the position of the current room based on the chosen direction

                RoomNode currentRoom = new RoomNode(currentRoomType, currentPosition); // Create the current room with the determined type and position

                AddRoomToGrid(currentRoom); // Add the current room to the grid

                ConnectRooms(previousRoom, currentRoom); // Connect the previous room to the current room

                previousRoom = currentRoom; // Update the previous room to the current room
            }
        }
    }

    private void CreateBranchRooms()
    {

        foreach (RoomNode mainRoom in _roomGrid)
        {

            if (mainRoom == null || mainRoom.type == RoomType.Boss || mainRoom.type == RoomType.Upgrade || mainRoom.type == RoomType.Branch)
                continue; // Skip null rooms or boss room

            List<Direction> availableDirections = new List<Direction>();
            // Check available adjacent positions
            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                if (dir != Direction.Invalid && mainRoom.nextRooms[(int)dir] == null && !IsRoomOccupied(mainRoom.position + DirectionToVector(dir)))
                {
                    availableDirections.Add(dir);
                }
            }

            if (availableDirections.Count > 0)
            {
                // Randomize the number of branch rooms (0 to 2)
                int numBranches = Random.Range(0, Mathf.Min(3, availableDirections.Count));

                for (int j = 0; j < numBranches; j++)
                {
                    // Randomly select a direction from available directions
                    Direction randomDirection = availableDirections[Random.Range(0, availableDirections.Count)];

                    // Calculate the position of the branch room
                    Vector2Int branchPosition = mainRoom.position + DirectionToVector(randomDirection);

                    // Create the branch room and add it to the grid
                    RoomNode branchRoom = new RoomNode(RoomType.Branch, branchPosition);
                    AddRoomToGrid(branchRoom);

                    // Connect the branch room to the main room
                    ConnectRooms(mainRoom, branchRoom);

                    // Remove the selected direction to avoid creating additional branches from this direction
                    availableDirections.Remove(randomDirection);
                }
            }
        }
    }

    private void AddRoomToGrid(RoomNode room)
    {
        int adjustedX = room.position.x + _roomGrid.GetLength(0) / 2;
        int adjustedY = room.position.y + _roomGrid.GetLength(1) / 2;

        if (adjustedX >= 0 && adjustedX < _roomGrid.GetLength(0) &&
            adjustedY >= 0 && adjustedY < _roomGrid.GetLength(1))
        {
            _roomGrid[adjustedX, adjustedY] = room;
        }
    }

    private void ConnectRooms(RoomNode roomA, RoomNode roomB)
    {
        if (roomA != null && roomB != null)
        {
            int deltaX = roomB.position.x - roomA.position.x;
            int deltaY = roomB.position.y - roomA.position.y;

            if (deltaX == 1)
            {
                roomA.nextRooms[(int)Direction.East] = roomB;
                roomB.nextRooms[(int)Direction.West] = roomA;
            }
            else if (deltaX == -1)
            {
                roomA.nextRooms[(int)Direction.West] = roomB;
                roomB.nextRooms[(int)Direction.East] = roomA;
            }
            else if (deltaY == 1)
            {
                roomA.nextRooms[(int)Direction.North] = roomB;
                roomB.nextRooms[(int)Direction.South] = roomA;
            }
            else if (deltaY == -1)
            {
                roomA.nextRooms[(int)Direction.South] = roomB;
                roomB.nextRooms[(int)Direction.North] = roomA;
            }
        }
    }

    private bool IsRoomOccupied(Vector2Int position)
    {
        int adjustedX = position.x + _roomGrid.GetLength(0) / 2;
        int adjustedY = position.y + _roomGrid.GetLength(1) / 2;

        if (adjustedX >= 0 && adjustedX < _roomGrid.GetLength(0) &&
            adjustedY >= 0 && adjustedY < _roomGrid.GetLength(1))
        {
            return _roomGrid[adjustedX, adjustedY] != null;
        }
        else
        {
            return false;
        }
    }

    private Vector2Int DirectionToVector(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return new Vector2Int(0, 1);
            case Direction.East:
                return new Vector2Int(1, 0);
            case Direction.South:
                return new Vector2Int(0, -1);
            case Direction.West:
                return new Vector2Int(-1, 0);
            default:
                return Vector2Int.zero;
        }
    }

    private Direction GetDirection(Vector2Int currentPosition, Vector2Int previousPosition)
    {
        int deltaX = currentPosition.x - previousPosition.x;
        int deltaY = currentPosition.y - previousPosition.y;

        if (deltaX == 1)
            return Direction.East;
        else if (deltaX == -1)
            return Direction.West;
        else if (deltaY == 1)
            return Direction.North;
        else if (deltaY == -1)
            return Direction.South;
        else
            return Direction.Invalid; 
    }

    private Direction GetOppositeDirection(Vector2Int currentPosition, Vector2Int previousPosition)
    {
        int deltaX = currentPosition.x - previousPosition.x;
        int deltaY = currentPosition.y - previousPosition.y;

        if (deltaX == 1)
            return Direction.West;
        else if (deltaX == -1)
            return Direction.East;
        else if (deltaY == 1)
            return Direction.South;
        else if (deltaY == -1)
            return Direction.North;
        else
            return Direction.Invalid; 
    }

    public List<Direction> GetAvailableDirections()
    {
        List<Direction> availableDirections = new List<Direction>();

        foreach (Direction dir in Enum.GetValues(typeof(Direction)))
        {
            if (dir != Direction.Invalid)
            {
                RoomNode nextRoom = CurrentPlayerLocation.nextRooms[(int)dir];
                if (nextRoom != null)
                {
                    availableDirections.Add(dir);
                }
            }
        }

        return availableDirections;
    }
}