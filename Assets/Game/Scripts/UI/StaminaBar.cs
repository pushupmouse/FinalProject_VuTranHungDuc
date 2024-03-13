using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private float _transitionSpeed = 0.5f;

    private Coroutine _staminaCoroutine;

    public void SetMaxStamina(float maxValue)
    {
        _slider.maxValue = maxValue;
        _slider.value = maxValue;
    }

    public void SetStamina(float value)
    {
        if (_staminaCoroutine != null)
        {
            StopCoroutine(_staminaCoroutine);
        }

        _staminaCoroutine = StartCoroutine(UpdateStamina(value));
    }

    private IEnumerator UpdateStamina(float targetValue)
    {
        float currentValue = _slider.value;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * _transitionSpeed;
            _slider.value = Mathf.Lerp(currentValue, targetValue, t);
            yield return null;
        }

        _slider.value = targetValue;

        _staminaCoroutine = null;
    }
}
