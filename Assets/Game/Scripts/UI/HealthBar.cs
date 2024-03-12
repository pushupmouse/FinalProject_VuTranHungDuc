using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Gradient _gradient;
    [SerializeField] private Image _fill;

    public void SetMaxHealth(float maxValue)
    {
        _slider.maxValue = maxValue;
        _slider.value = maxValue;

        _fill.color = _gradient.Evaluate(1f);
    }

    public void SetHealth(float value)
    {
        _slider.value = value;

        _fill.color = _gradient.Evaluate(_slider.normalizedValue);
    }
}
