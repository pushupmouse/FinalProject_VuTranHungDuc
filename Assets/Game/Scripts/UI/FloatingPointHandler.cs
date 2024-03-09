using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPointHandler : MonoBehaviour
{
    [SerializeField] private TextMesh _text;

    public void DisplayDamageText(float damage)
    {
        Destroy(gameObject, 1f);
        transform.localPosition += new Vector3(0, 1f, 0);
        _text.text = damage.ToString();
    }
}
