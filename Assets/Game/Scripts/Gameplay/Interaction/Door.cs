using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [HideInInspector] public Direction DoorDirection;

    private SpawnManager spawnManager;

    private void Start()
    {
        spawnManager = SpawnManager.Instance;
    }

    public void Interact()
    {
        if (!spawnManager.EnemiesAlive) // Check if all enemies are defeated
        {
            DungeonTraversalManager dungeonTraversalManager = DungeonTraversalManager.Instance;

            if (dungeonTraversalManager != null)
            {
                RoomNode nextRoom = dungeonTraversalManager._dungeonManager.CurrentPlayerLocation.nextRooms[(int)DoorDirection];
                if (nextRoom != null)
                {
                    dungeonTraversalManager.MoveToRoom(nextRoom, DoorDirection);
                }
            }
        }
    }
}
