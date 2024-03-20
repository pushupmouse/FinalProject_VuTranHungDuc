using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance;

    public RoomLayout roomLayoutPrefab;
    public Transform gridParent;
    public Transform player;
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

    public void MoveToRoom(RoomNode nextRoom, Direction enteredFromDirection)
    {
        if (currentRoom != null)
        {
            Destroy(currentRoom.gameObject);
        }

        currentRoom = Instantiate(roomLayoutPrefab, gridParent);

        dungeonLayout.CurrentPlayerLocation = nextRoom;

        currentRoom.AddDoors(dungeonLayout.GetAvailableDirections());

        Debug.Log("Current Room: " + dungeonLayout.CurrentPlayerLocation.type);

        // Calculate the opposite direction
        Direction oppositeDirection = GetOppositeDirection(enteredFromDirection);

        // Get the door position for the opposite direction
        Vector2Int oppositeDoorPosition = currentRoom.GetDoorPosition(oppositeDirection);

        // Set the player's position to be next to the door of the opposite direction
        Vector3 playerPosition = gridParent.TransformPoint(new Vector3(oppositeDoorPosition.x, oppositeDoorPosition.y, 0));

        // Offset the player according to the direction
        playerPosition += GetPlayerOffset(oppositeDirection);

        // Update player's position
        player.transform.position = playerPosition;
    }

    private Direction GetOppositeDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return Direction.South;
            case Direction.South:
                return Direction.North;
            case Direction.East:
                return Direction.West;
            case Direction.West:
                return Direction.East;
            default:
                Debug.LogError("Invalid direction.");
                return Direction.Invalid;
        }
    }

    // Method to offset the player according to the direction
    private Vector3 GetPlayerOffset(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return new Vector3(0f, -1f, 0f); // Offset to the south
            case Direction.South:
                return new Vector3(0f, 2f, 0f); // Offset to the north
            case Direction.East:
                return new Vector3(-1f, 1f, 0f); // Offset to the west
            case Direction.West:
                return new Vector3(2f, 1f, 0f); // Offset to the east
            default:
                Debug.LogError("Invalid direction.");
                return Vector3.zero;
        }
    }

}