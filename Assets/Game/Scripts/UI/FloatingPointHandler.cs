using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPointHandler : MonoBehaviour
{
    [SerializeField] private TextMesh _text;

    public void DisplayDamageText(float damage, bool isCritical)
    {
        Destroy(gameObject, 1f);
        transform.localPosition += new Vector3(0, 1f, 0);
        
        if (isCritical)
        {
            _text.color = Color.red;
            _text.fontSize = 75;
            _text.text = "-" + damage.ToString() + "!";
        }
        else
        {
            _text.color = Color.white;
            _text.text = "-" + damage.ToString();
        }
    }

    public void DisplayHealText(float healAmount)
    {
        Destroy(gameObject, 1f);
        transform.localPosition += new Vector3(0, 0.5f, 0);
        _text.color = Color.green;
        _text.fontSize = 25;
        _text.text = "+" + healAmount.ToString();
    }
}
