using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum RoomType
{
    Fighting,
    Shop,
    Heal,
    Boss
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
    public RoomNode[] nextRooms = new RoomNode[4]; // Array to store next rooms for each direction
    public Vector2Int position; // Position of the room in the XY coordinate system

    public RoomNode(RoomType type, Vector2Int position)
    {
        this.type = type;
        this.position = position;
    }
}

public class DungeonLayout : MonoBehaviour
{
    [SerializeField] private int _maxMainRooms;
    [SerializeField] private int _maxBranchRooms;

    private RoomNode[,] _roomGrid; // 2D array to store the rooms based on their XY positions
    public RoomNode StartingRoom;
    public RoomNode CurrentPlayerLocation;

    private void Start()
    {
        // Define room types available for branching paths (excluding boss room)
        RoomType[] branchRoomTypes = { RoomType.Fighting, RoomType.Shop, RoomType.Heal };

        // Create the starting room
        RoomNode startingRoom = new RoomNode(RoomType.Fighting, new Vector2Int(0, 0));

        // Initialize the room grid and add the starting room to it
        _roomGrid = new RoomNode[100, 100]; // Hard coded grid size for example
        AddRoomToGrid(startingRoom);

        // Create rooms for the main path leading to the boss
        RoomNode previousRoom = startingRoom;
        for (int i = 1; i <= _maxMainRooms; i++) // Three rooms distance to boss room
        {
            // Randomly select a room type for the current room
            RoomType currentRoomType = (i < _maxMainRooms) ? branchRoomTypes[Random.Range(0, branchRoomTypes.Length)] : RoomType.Boss;

            // Get a list of available directions (excluding the direction back to the previous room)
            List<Direction> availableDirections = new List<Direction> { Direction.North, Direction.East, Direction.South, Direction.West };
            if (i > 1)
            {
                Direction directionBack = GetOppositeDirection(previousRoom.position, startingRoom.position);
                availableDirections.Remove(directionBack);
            }

            // Ensure the boss room is placed in a direction different from the previous room's position
            if (currentRoomType == RoomType.Boss)
            {
                availableDirections.Remove(GetDirection(previousRoom.position, startingRoom.position));
            }

            // Remove directions that lead to existing rooms
            for (int j = availableDirections.Count - 1; j >= 0; j--)
            {
                Direction direction = availableDirections[j];
                Vector2Int nextPosition = previousRoom.position + DirectionToVector(direction);
                if (IsRoomOccupied(nextPosition))
                {
                    availableDirections.RemoveAt(j);
                }
            }

            // Check if there are available directions to place the room
            if (availableDirections.Count > 0)
            {
                // Randomly select a direction from the available directions
                Direction randomDirection = availableDirections[Random.Range(0, availableDirections.Count)];

                // Calculate the position of the current room based on the random direction
                Vector2Int currentPosition = previousRoom.position + DirectionToVector(randomDirection);

                // Create the current room and add it to the grid
                RoomNode currentRoom = new RoomNode(currentRoomType, currentPosition);
                AddRoomToGrid(currentRoom);

                // Connect the current room to the previous room
                ConnectRooms(previousRoom, currentRoom);

                // Update previous room for the next iteration
                previousRoom = currentRoom;
            }
            else
            {
                Debug.LogWarning("No available directions to place the room. Exiting loop.");
                break;
            }
        }

        // Branching logic
        foreach (RoomNode mainRoom in _roomGrid)
        {
            if (mainRoom == null || mainRoom.type == RoomType.Boss)
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
                // Randomize the number of branch rooms (0 to _maxBranchRooms)
                int numBranches = Random.Range(0, Mathf.Min(_maxBranchRooms + 1, availableDirections.Count));

                for (int i = 0; i < numBranches; i++)
                {
                    // Randomly select a direction from available directions
                    Direction randomDirection = availableDirections[Random.Range(0, availableDirections.Count)];

                    // Calculate the position of the branch room
                    Vector2Int branchPosition = mainRoom.position + DirectionToVector(randomDirection);

                    // Create the branch room and add it to the grid
                    RoomNode branchRoom = new RoomNode(RoomType.Fighting, branchPosition);
                    AddRoomToGrid(branchRoom);

                    // Connect the branch room to the main room
                    ConnectRooms(mainRoom, branchRoom);

                    // Remove the selected direction to avoid creating additional branches from this direction
                    availableDirections.Remove(randomDirection);
                }
            }
        }

        // Set the starting room as the current player location
        CurrentPlayerLocation = startingRoom;

        PrintDungeonLayout(startingRoom);
    }

    private void AddRoomToGrid(RoomNode room)
    {
        // Calculate the adjusted indices for negative coordinates
        int adjustedX = room.position.x + _roomGrid.GetLength(0) / 2;
        int adjustedY = room.position.y + _roomGrid.GetLength(1) / 2;

        // Check if the adjusted indices are within the bounds of the room grid array
        if (adjustedX >= 0 && adjustedX < _roomGrid.GetLength(0) &&
            adjustedY >= 0 && adjustedY < _roomGrid.GetLength(1))
        {
            // Assign the room to the grid at the adjusted indices
            _roomGrid[adjustedX, adjustedY] = room;
        }
        else
        {
            Debug.LogError("Room position is outside the bounds of the room grid.");
        }
    }

    private void ConnectRooms(RoomNode roomA, RoomNode roomB)
    {
        // Check if both rooms are not null
        if (roomA != null && roomB != null)
        {
            // Calculate the delta position between roomB and roomA
            int deltaX = roomB.position.x - roomA.position.x;
            int deltaY = roomB.position.y - roomA.position.y;

            // Deduce the direction from the delta position
            if (deltaX == 1)
            {
                // RoomB is to the east of RoomA
                roomA.nextRooms[(int)Direction.East] = roomB;
                roomB.nextRooms[(int)Direction.West] = roomA;
            }
            else if (deltaX == -1)
            {
                // RoomB is to the west of RoomA
                roomA.nextRooms[(int)Direction.West] = roomB;
                roomB.nextRooms[(int)Direction.East] = roomA;
            }
            else if (deltaY == 1)
            {
                // RoomB is to the north of RoomA
                roomA.nextRooms[(int)Direction.North] = roomB;
                roomB.nextRooms[(int)Direction.South] = roomA;
            }
            else if (deltaY == -1)
            {
                // RoomB is to the south of RoomA
                roomA.nextRooms[(int)Direction.South] = roomB;
                roomB.nextRooms[(int)Direction.North] = roomA;
            }
            // If delta position does not match any direction, rooms won't connect
            else
            {
                Debug.LogError("Rooms are not adjacent. Cannot connect.");
            }
        }
        else
        {
            Debug.LogError("One or both rooms are null. Cannot connect.");
        }
    }

    private bool IsRoomOccupied(Vector2Int position)
    {
        // Calculate the adjusted indices for negative coordinates
        int adjustedX = position.x + _roomGrid.GetLength(0) / 2;
        int adjustedY = position.y + _roomGrid.GetLength(1) / 2;

        // Check if the adjusted indices are within the bounds of the room grid array
        if (adjustedX >= 0 && adjustedX < _roomGrid.GetLength(0) &&
            adjustedY >= 0 && adjustedY < _roomGrid.GetLength(1))
        {
            // Check if a room exists at the given position
            return _roomGrid[adjustedX, adjustedY] != null;
        }
        else
        {
            // Position is outside the bounds of the room grid
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
        // Calculate the direction from the previous position to the current position
        int deltaX = currentPosition.x - previousPosition.x;
        int deltaY = currentPosition.y - previousPosition.y;

        // Deduce the direction based on the change in position
        if (deltaX == 1)
            return Direction.East;
        else if (deltaX == -1)
            return Direction.West;
        else if (deltaY == 1)
            return Direction.North;
        else if (deltaY == -1)
            return Direction.South;
        else
            return Direction.Invalid; // Default to North if the positions are the same or invalid
    }

    // Method to get the opposite direction between two positions
    private Direction GetOppositeDirection(Vector2Int currentPosition, Vector2Int previousPosition)
    {
        // Calculate the direction from the current position to the previous position
        int deltaX = currentPosition.x - previousPosition.x;
        int deltaY = currentPosition.y - previousPosition.y;

        // Deduce the opposite direction based on the change in position
        if (deltaX == 1)
            return Direction.West;
        else if (deltaX == -1)
            return Direction.East;
        else if (deltaY == 1)
            return Direction.South;
        else if (deltaY == -1)
            return Direction.North;
        else
            return Direction.Invalid; // Default to North if the positions are the same or invalid
    }

    private HashSet<RoomNode> visited = new HashSet<RoomNode>();

    private void PrintDungeonLayout(RoomNode currentRoom)
    {
        // Print the current room type and its position
        Debug.Log("Room Type: " + currentRoom.type + " | Position: " + currentRoom.position);

        // Mark the current room as visited
        visited.Add(currentRoom);

        // Traverse each adjacent room
        foreach (RoomNode adjacentRoom in currentRoom.nextRooms)
        {
            // Check if the adjacent room is not null and not visited
            if (adjacentRoom != null && !visited.Contains(adjacentRoom))
            {
                // Recursively traverse the adjacent room
                PrintDungeonLayout(adjacentRoom);
            }
        }
    }

}