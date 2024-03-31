using System.Collections.Generic;
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
    
    public List<Transform> SpawnPoints;

    public void AddDoors(List<Direction> availableDirections)
    {
        BoundsInt bounds = _wallTilemap.cellBounds;

        foreach (Direction dir in availableDirections)
        {
            int middleIndex;
            switch (dir)
            {
                case Direction.North:
                case Direction.South:
                    middleIndex = bounds.min.x + bounds.size.x / 2;
                    break;
                case Direction.East:
                case Direction.West:
                    middleIndex = bounds.min.y + bounds.size.y / 2;
                    break;
                default:
                    Debug.LogError("Invalid door direction.");
                    return;
            }

            Vector3Int localPositionLeft;
            Vector3Int localPositionRight;

            switch (dir)
            {
                case Direction.North:
                    localPositionLeft = new Vector3Int(middleIndex - 1, bounds.max.y - 1, 0);
                    localPositionRight = new Vector3Int(middleIndex, bounds.max.y - 1, 0);
                    break;
                case Direction.South:
                    localPositionLeft = new Vector3Int(middleIndex - 1, bounds.min.y, 0);
                    localPositionRight = new Vector3Int(middleIndex, bounds.min.y, 0);
                    break;
                case Direction.East:
                    localPositionLeft = new Vector3Int(bounds.max.x - 1, middleIndex, 0);
                    localPositionRight = new Vector3Int(bounds.max.x - 1, middleIndex + 1, 0);
                    break;
                case Direction.West:
                    localPositionLeft = new Vector3Int(bounds.min.x, middleIndex, 0);
                    localPositionRight = new Vector3Int(bounds.min.x, middleIndex + 1, 0);
                    break;
                default:
                    Debug.LogError("Invalid door direction.");
                    continue;
            }

            Vector3Int worldPositionLeft = _wallTilemap.LocalToCell(localPositionLeft);
            Vector3Int worldPositionRight = _wallTilemap.LocalToCell(localPositionRight);

            _wallTilemap.SetTile(worldPositionLeft, _doorInfos[(int)dir].DoorTileLeft);
            _wallTilemap.SetTile(worldPositionRight, _doorInfos[(int)dir].DoorTileRight);

            Door interactDoor = Instantiate(_interactDoor, _wallTilemap.CellToWorld(worldPositionLeft), Quaternion.identity);
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

            interactDoor.transform.SetParent(_wallTilemap.transform);
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