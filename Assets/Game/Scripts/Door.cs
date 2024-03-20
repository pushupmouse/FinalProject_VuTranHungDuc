using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public Direction DoorDirection;

    public void Interact()
    {
        DungeonManager dungeonManager = DungeonManager.Instance;

        if (dungeonManager != null)
        {
            RoomNode nextRoom = dungeonManager.dungeonLayout.CurrentPlayerLocation.nextRooms[(int)DoorDirection];
            if (nextRoom != null)
            {
                dungeonManager.MoveToRoom(nextRoom);
            }
        }
    }
}
