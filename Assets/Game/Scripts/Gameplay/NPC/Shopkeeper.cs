using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shopkeeper : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("hi im keeper");
    }
}
