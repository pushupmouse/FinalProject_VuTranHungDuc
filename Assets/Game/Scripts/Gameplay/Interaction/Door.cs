using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public Direction DoorDirection;

    public void Interact()
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
