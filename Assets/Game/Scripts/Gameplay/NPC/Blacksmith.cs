using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Blacksmith : MonoBehaviour, IInteractable
{
    private Transform _target;

    public void Interact()
    {
        Debug.Log("hi im blacksmith");
    }

    public void SetTarget(Transform transform)
    {
        _target = transform;
    }
}
