using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPointHandler : MonoBehaviour
{
    [SerializeField] private TextMesh _text;
    [SerializeField] private int _fontSizeDamage = 75;
    [SerializeField] private int _fontSizeDamageCrit = 100;
    [SerializeField] private int _fontSizeHeal = 25;

    public void DisplayDamageText(float damage, bool isCritical)
    {
        Destroy(gameObject, 1f);
        transform.localPosition += new Vector3(0, 1f, 0);
        
        if (isCritical)
        {
            _text.color = Color.red;
            _text.fontSize = _fontSizeDamageCrit;
            _text.text = "-" + damage.ToString() + "!";
        }
        else
        {
            _text.color = Color.white;
            _text.fontSize = _fontSizeDamage;
            _text.text = "-" + damage.ToString();
        }
    }

    public void DisplayHealText(float healAmount)
    {
        Destroy(gameObject, 1f);
        transform.localPosition += new Vector3(0, 0.5f, 0);
        _text.color = Color.green;
        _text.fontSize = _fontSizeHeal;
        _text.text = "+" + healAmount.ToString();
    }
}
