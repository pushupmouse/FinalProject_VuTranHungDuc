using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    [System.Serializable]
    public class DoorInfo
    {
        public Tile DoorTileLeft;
        public Tile DoorTileRight;
        public Direction DoorDirection;
    }

    [SerializeField] private DoorInfo[] _doorInfos;
    [SerializeField] private Tilemap _wallTilemap;
    [SerializeField] private Door _interactDoor;

    private int _middleIndex;
    private BoundsInt _bounds;

    
    public List<Transform> SpawnPoints;
    public Transform SpawnInstances;

    public void AddDoors(List<Direction> availableDirections)
    {
        _bounds = _wallTilemap.cellBounds;

        foreach (Direction direction in availableDirections)
        {
            switch (direction)
            {
                case Direction.North:
                case Direction.South:
                    _middleIndex = _bounds.min.x + _bounds.size.x / 2;
                    break;
                case Direction.East:
                case Direction.West:
                    _middleIndex = _bounds.min.y + _bounds.size.y / 2;
                    break;
                default:
                    Debug.LogError("Invalid door direction.");
                    return;
            }

            Vector3Int localPositionLeft;
            Vector3Int localPositionRight;

            AssignLocalPositions(direction, out localPositionLeft, out localPositionRight);

            Vector3Int worldPositionLeft = _wallTilemap.LocalToCell(localPositionLeft);
            Vector3Int worldPositionRight = _wallTilemap.LocalToCell(localPositionRight);

            _wallTilemap.SetTile(worldPositionLeft, _doorInfos[(int)direction].DoorTileLeft);
            _wallTilemap.SetTile(worldPositionRight, _doorInfos[(int)direction].DoorTileRight);

            Door interactDoor = Instantiate(_interactDoor, _wallTilemap.CellToWorld(worldPositionLeft), Quaternion.identity);

            SetDoorDirection(interactDoor, direction);

            interactDoor.transform.SetParent(_wallTilemap.transform);
        }
    }

    private void AssignLocalPositions(Direction dir, out Vector3Int localPositionLeft, out Vector3Int localPositionRight)
    {
        switch (dir)
        {
            case Direction.North:
                localPositionLeft = new Vector3Int(_middleIndex - 1, _bounds.max.y - 1, 0);
                localPositionRight = new Vector3Int(_middleIndex, _bounds.max.y - 1, 0);
                break;
            case Direction.South:
                localPositionLeft = new Vector3Int(_middleIndex - 1, _bounds.min.y, 0);
                localPositionRight = new Vector3Int(_middleIndex, _bounds.min.y, 0);
                break;
            case Direction.East:
                localPositionLeft = new Vector3Int(_bounds.max.x - 1, _middleIndex, 0);
                localPositionRight = new Vector3Int(_bounds.max.x - 1, _middleIndex + 1, 0);
                break;
            case Direction.West:
                localPositionLeft = new Vector3Int(_bounds.min.x, _middleIndex, 0);
                localPositionRight = new Vector3Int(_bounds.min.x, _middleIndex + 1, 0);
                break;
            default:
                Debug.LogError("Invalid door direction.");
                localPositionLeft = Vector3Int.zero;
                localPositionRight = Vector3Int.zero;
                break;
        }
    }

    private void SetDoorDirection(Door interactDoor, Direction dir)
    {
        interactDoor.DoorDirection = dir;

        switch (dir)
        {
            case Direction.North:
            case Direction.South:
                interactDoor.transform.position += new Vector3(0f, 0.5f, 0f);
                break;
            case Direction.East:
            case Direction.West:
                interactDoor.transform.position += new Vector3(0.5f, 0f, 0f);
                interactDoor.transform.Rotate(0f, 0f, 90f);
                break;
            default:
                Debug.LogError("Invalid door direction.");
                break;
        }
    }


    public Vector2Int GetDoorPosition(Direction direction)
    {
        BoundsInt bounds = _wallTilemap.cellBounds;

        switch (direction)
        {
            case Direction.North:
                return new Vector2Int(bounds.min.x + bounds.size.x / 2, bounds.max.y - 1);
            case Direction.South:
                return new Vector2Int(bounds.min.x + bounds.size.x / 2, bounds.min.y);
            case Direction.East:
                return new Vector2Int(bounds.max.x - 1, bounds.min.y + bounds.size.y / 2);
            case Direction.West:
                return new Vector2Int(bounds.min.x, bounds.min.y + bounds.size.y / 2);
            default:
                Debug.LogError("Invalid direction.");
                return Vector2Int.zero;
        }
    }
}