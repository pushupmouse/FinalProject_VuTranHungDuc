using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum RoomType
{
    Start,
    Fighting,
    Shop,
    Treasure,
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
    public RoomNode[] nextRooms = new RoomNode[4];
    public Vector2Int position;

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

    private RoomNode[,] _roomGrid;
    public RoomNode CurrentPlayerLocation;

    public DungeonTraversal dungeonTraversal;
    public RoomLayout roomLayout;
    public DungeonManager dungeonManager;

    private void Start()
    {
        RoomType[] branchRoomTypes = {  RoomType.Start, RoomType.Fighting, RoomType.Shop, RoomType.Treasure };

        RoomNode startingRoom = new RoomNode(RoomType.Start, new Vector2Int(0, 0));

        _roomGrid = new RoomNode[100, 100];
        AddRoomToGrid(startingRoom);

        RoomNode previousRoom = startingRoom;
        for (int i = 1; i <= _maxMainRooms; i++) 
        {
            RoomType currentRoomType = (i < _maxMainRooms) ? branchRoomTypes[Random.Range(0, branchRoomTypes.Length)] : RoomType.Boss;

            List<Direction> availableDirections = new List<Direction> { Direction.North, Direction.East, Direction.South, Direction.West };
            if (i > 1)
            {
                Direction directionBack = GetOppositeDirection(previousRoom.position, startingRoom.position);
                availableDirections.Remove(directionBack);
            }

            if (currentRoomType == RoomType.Boss)
            {
                availableDirections.Remove(GetDirection(previousRoom.position, startingRoom.position));
            }

            for (int j = availableDirections.Count - 1; j >= 0; j--)
            {
                Direction direction = availableDirections[j];
                Vector2Int nextPosition = previousRoom.position + DirectionToVector(direction);
                if (IsRoomOccupied(nextPosition))
                {
                    availableDirections.RemoveAt(j);
                }
            }

            if (availableDirections.Count > 0)
            {
                Direction randomDirection = availableDirections[Random.Range(0, availableDirections.Count)];

                Vector2Int currentPosition = previousRoom.position + DirectionToVector(randomDirection);

                RoomNode currentRoom = new RoomNode(currentRoomType, currentPosition);
                AddRoomToGrid(currentRoom);

                ConnectRooms(previousRoom, currentRoom);

                previousRoom = currentRoom;
            }
        }

        foreach (RoomNode mainRoom in _roomGrid)
        {
            if (mainRoom == null || mainRoom.type == RoomType.Boss)
                continue;

            List<Direction> availableDirections = new List<Direction>();
            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                if (dir != Direction.Invalid && mainRoom.nextRooms[(int)dir] == null && !IsRoomOccupied(mainRoom.position + DirectionToVector(dir)))
                {
                    availableDirections.Add(dir);
                }
            }

            if (availableDirections.Count > 0)
            {
                int numBranches = Random.Range(0, Mathf.Min(_maxBranchRooms + 1, availableDirections.Count));

                for (int i = 0; i < numBranches; i++)
                {
                    Direction randomDirection = availableDirections[Random.Range(0, availableDirections.Count)];

                    Vector2Int branchPosition = mainRoom.position + DirectionToVector(randomDirection);

                    RoomNode branchRoom = new RoomNode(RoomType.Fighting, branchPosition);
                    AddRoomToGrid(branchRoom);

                    ConnectRooms(mainRoom, branchRoom);

                    availableDirections.Remove(randomDirection);
                }
            }
        }

        CurrentPlayerLocation = startingRoom;
        dungeonManager.InitializeDungeonManager();
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
        else
        {
            Debug.LogError("Room position is outside the bounds of the room grid.");
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