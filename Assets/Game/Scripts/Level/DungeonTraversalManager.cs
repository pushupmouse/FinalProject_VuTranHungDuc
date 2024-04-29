using UnityEngine;

public class DungeonTraversalManager : MonoBehaviour
{
    public static DungeonTraversalManager Instance;

    [SerializeField] private Room _room;
    [SerializeField] private Transform _gridParent;
    [SerializeField] private Transform _player;
    
    private Room currentRoom;
    public DungeonManager _dungeonManager;

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
        _dungeonManager = DungeonManager.Instance;
    }

    public void InitializeTraversal()
    {
        RoomNode firstRoom = _dungeonManager.CurrentPlayerLocation;
        if (firstRoom != null)
        {
            currentRoom = Instantiate(_room, _gridParent);

            currentRoom.AddDoors(_dungeonManager.GetAvailableDirections());

            _dungeonManager.CurrentPlayerLocation = firstRoom;
        }
    }

    public void MoveToRoom(RoomNode nextRoom, Direction enteredFromDirection)
    {
        if (currentRoom != null)
        {
            Destroy(currentRoom.gameObject);
        }

        currentRoom = Instantiate(_room, _gridParent);

        _dungeonManager.CurrentPlayerLocation = nextRoom;

        currentRoom.AddDoors(_dungeonManager.GetAvailableDirections());

        // Calculate the opposite direction
        Direction oppositeDirection = GetOppositeDirection(enteredFromDirection);

        // Get the door position for the opposite direction
        Vector2Int oppositeDoorPosition = currentRoom.GetDoorPosition(oppositeDirection);

        // Set the player's position to be next to the door of the opposite direction
        Vector3 playerPosition = _gridParent.TransformPoint(new Vector3(oppositeDoorPosition.x, oppositeDoorPosition.y, 0));

        // Offset the player according to the direction
        playerPosition += GetPlayerOffset(oppositeDirection);

        SpawnManager spawnManager = SpawnManager.Instance;

        // Update player's position
        spawnManager.TeleportPlayer(playerPosition);

        switch (_dungeonManager.CurrentPlayerLocation.type)
        {
            case RoomType.Start:
                break;
            case RoomType.Boss:
                spawnManager.SpawnBoss(currentRoom);
                break;
            case RoomType.Fighting:
                spawnManager.SpawnEnemy(currentRoom);
                break;
            case RoomType.Upgrade:
                spawnManager.SpawnUpgradeNPCs(currentRoom);
                break;
            case RoomType.Treasure:
                spawnManager.SpawnTreasure(currentRoom);
                break;
            case RoomType.Branch:
                spawnManager.SpawnEnemy(currentRoom);
                break;
            default:
                break;
        }
    }

    public Room GetCurrentRoom()
    {
        return currentRoom;
    }

    public void RecreateDungeon()
    {
        if (currentRoom != null)
        {
            Destroy(currentRoom.gameObject);
        }

        _dungeonManager.CreateDungeon();
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