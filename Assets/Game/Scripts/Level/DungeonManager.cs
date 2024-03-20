using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance;

    public RoomLayout roomLayoutPrefab;
    public Transform gridParent;

    public DungeonLayout dungeonLayout;
    private RoomLayout currentRoom;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple DungeonManager instances detected. Destroying duplicate.");
            Destroy(gameObject);
        }
    }

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

    public void MoveToRoom(RoomNode nextRoom)
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