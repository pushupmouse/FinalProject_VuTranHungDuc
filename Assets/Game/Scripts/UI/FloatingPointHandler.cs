using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPointHandler : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 1f);
        transform.localPosition += new Vector3(0, 1f, 0);
    }
}
