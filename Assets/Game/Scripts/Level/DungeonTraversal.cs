using System.Collections.Generic;
using UnityEngine;

public class DungeonTraversal : MonoBehaviour
{
    public DungeonLayout dungeonLayout;

    public void Init()
    {
        Debug.Log("Current Room: " + dungeonLayout.CurrentPlayerLocation.type);

        List<Direction> availableDirections = dungeonLayout.GetAvailableDirections();
        foreach (Direction availableDirection in availableDirections)
        {
            Debug.Log(availableDirection.ToString());
        }
    }

    void Update()
    {
        CheckMovement(KeyCode.W, Direction.North, "north");
        CheckMovement(KeyCode.A, Direction.West, "west");
        CheckMovement(KeyCode.S, Direction.South, "south");
        CheckMovement(KeyCode.D, Direction.East, "east");
    }

    void CheckMovement(KeyCode key, Direction direction, string directionName)
    {
        if (Input.GetKeyDown(key))
        {
            RoomNode nextRoom = dungeonLayout.CurrentPlayerLocation.nextRooms[(int)direction];
            if (nextRoom != null)
            {
                MoveToRoom(nextRoom);
            }
            else
            {
                Debug.Log($"There is no path to the {directionName}!");
            }
        }
    }

    void MoveToRoom(RoomNode nextRoom)
    {
        dungeonLayout.CurrentPlayerLocation = nextRoom;
        Debug.Log("Current Room: " + dungeonLayout.CurrentPlayerLocation.type);

        List<Direction> availableDirections = dungeonLayout.GetAvailableDirections();
        foreach (Direction availableDirection in availableDirections)
        {
            Debug.Log(availableDirection.ToString());
        }
    }
}