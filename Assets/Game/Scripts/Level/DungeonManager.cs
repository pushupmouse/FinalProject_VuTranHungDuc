using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public RoomLayout roomLayoutPrefab;
    public Transform gridParent; 

    public DungeonLayout dungeonLayout; 
    private RoomLayout currentRoom; 


    public void InitializeDungeonManager()
    {
        RoomNode firstRoom = dungeonLayout.CurrentPlayerLocation;
        if (firstRoom != null)
        {
            currentRoom = Instantiate(roomLayoutPrefab, gridParent);

            currentRoom.AddDoors(dungeonLayout.GetAvailableDirections());

            dungeonLayout.CurrentPlayerLocation = firstRoom;

            Debug.Log("Current Room: " + dungeonLayout.CurrentPlayerLocation.type);
        }
    }

    private void Update()
    {
        CheckMovement(KeyCode.I, Direction.North);
        CheckMovement(KeyCode.J, Direction.West);
        CheckMovement(KeyCode.K, Direction.South);
        CheckMovement(KeyCode.L, Direction.East);
    }

    void CheckMovement(KeyCode key, Direction direction)
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
                Debug.Log($"There is no path to the {(Direction)direction}!");
            }
        }
    }

    void MoveToRoom(RoomNode nextRoom)
    {
        if (currentRoom != null)
        {
            Destroy(currentRoom.gameObject);
        }

        currentRoom = Instantiate(roomLayoutPrefab, gridParent);

        dungeonLayout.CurrentPlayerLocation = nextRoom;

        currentRoom.AddDoors(dungeonLayout.GetAvailableDirections());

        Debug.Log("Current Room: " + dungeonLayout.CurrentPlayerLocation.type);
    }
}