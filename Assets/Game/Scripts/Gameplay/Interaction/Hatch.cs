using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hatch : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        if(LevelManager.Instance.CurrentLevel >= 3)
        {
            UIManager.Instance.ShowGameOverScreen(true);
        }
        else
        {
            LevelManager.Instance.IncreaseLevel();
        }
    }
}
