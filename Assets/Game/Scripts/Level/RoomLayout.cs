using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class RoomLayout : MonoBehaviour
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
    public DungeonLayout _dungeonLayout; // Reference to the dungeon layout script

    public void AddDoors()
    {
        List<Direction> availableDirections = _dungeonLayout.GetAvailableDirections();

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
        }
    }
}