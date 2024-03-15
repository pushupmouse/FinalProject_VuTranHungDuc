using UnityEngine;

public class DungeonTraversal : MonoBehaviour
{
    public DungeonLayout dungeonLayout; // Reference to the DungeonLayout script

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
        // Update currentPlayerRoom directly in the DungeonLayout script
        dungeonLayout.CurrentPlayerLocation = nextRoom;
        Debug.Log("Current Room: " + dungeonLayout.CurrentPlayerLocation.type);
        Debug.Log("Position: " + dungeonLayout.CurrentPlayerLocation.position);
    }
}