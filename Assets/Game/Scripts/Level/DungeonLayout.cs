using UnityEngine;
using System.Collections.Generic;

public enum RoomType
{
    Fighting,
    Shop,
    Heal,
    Boss
}

public class RoomNode
{
    public RoomType type;
    public RoomNode[] nextRooms = new RoomNode[4]; // Array to store next rooms for each direction (excluding entrance)

    public RoomNode(RoomType _type)
    {
        type = _type;
    }
}

public class DungeonLayout : MonoBehaviour
{
    public RoomNode startingRoom;
    public RoomNode currentPlayerLocation;

    void Start()
    {
        RoomNode room1 = new RoomNode(RoomType.Fighting);
        RoomNode room2 = new RoomNode(RoomType.Shop);
        RoomNode room3 = new RoomNode(RoomType.Heal);
        RoomNode room4 = new RoomNode(RoomType.Boss);

        ConnectRooms(room1, room2, Direction.North);
        ConnectRooms(room1, room3, Direction.East);
        ConnectRooms(room3, room4, Direction.South);

        startingRoom = room1; // Assign the starting room to the DungeonLayout

        // Assign the starting room to the currentPlayerRoom after setting up the layout
        currentPlayerLocation = startingRoom;
    }

    void ConnectRooms(RoomNode roomA, RoomNode roomB, Direction direction)
    {
        roomA.nextRooms[(int)direction] = roomB;
        roomB.nextRooms[(int)OppositeDirection(direction)] = roomA; // Bidirectional connection
    }

    Direction OppositeDirection(Direction direction)
    {
        // Calculate opposite direction
        return (Direction)(((int)direction + 2) % 4);
    }

    //void PrintDungeonLayout(RoomNode startNode)
    //{
    //    Queue<RoomNode> queue = new Queue<RoomNode>();
    //    HashSet<RoomNode> visited = new HashSet<RoomNode>();

    //    queue.Enqueue(startNode);

    //    while (queue.Count > 0)
    //    {
    //        RoomNode currentRoom = queue.Dequeue();

    //        if (!visited.Contains(currentRoom))
    //        {
    //            Debug.Log("Room Type: " + currentRoom.type);

    //            for (int i = 0; i < currentRoom.nextRooms.Length; i++)
    //            {
    //                RoomNode nextRoom = currentRoom.nextRooms[i];
    //                if (nextRoom != null)
    //                {
    //                    // Get the direction corresponding to the index
    //                    Direction direction = (Direction)i;
    //                    Debug.Log("   Next Room Type (" + direction + "): " + nextRoom.type);
    //                    queue.Enqueue(nextRoom);
    //                }
    //            }

    //            visited.Add(currentRoom);
    //        }
    //    }
    //}
}

public enum Direction
{
    North = 0,
    East = 1,
    South = 2,
    West = 3,
}