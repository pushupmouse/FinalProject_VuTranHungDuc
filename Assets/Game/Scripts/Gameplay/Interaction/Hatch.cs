using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hatch : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        LevelManager.Instance.IncreaseLevel();
    }
}
