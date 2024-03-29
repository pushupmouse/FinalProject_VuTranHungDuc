using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Gradient _gradient;
    [SerializeField] private Image _fill;
    [SerializeField] private float _transitionSpeed = 0.5f;
    [SerializeField] private bool _isInactive;

    private Coroutine _healthCoroutine;

    private void Start()
    {
        if (_isInactive)
        {
            SetHealthBarVisibility(false);
        }
    }

    public void SetMaxHealth(float maxValue)
    {
        _slider.maxValue = maxValue;
        _slider.value = maxValue;

        _fill.color = _gradient.Evaluate(1f);
    }

    public void SetHealth(float value)
    {
        if (_isInactive)
        {
            SetHealthBarVisibility(true);
        }

        if (_healthCoroutine != null)
        {
            StopCoroutine(_healthCoroutine);
        }

        _healthCoroutine = StartCoroutine(UpdateHealth(value));
    }

    private IEnumerator UpdateHealth(float targetValue)
    {
        float currentValue = _slider.value;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * _transitionSpeed;
            _slider.value = Mathf.Lerp(currentValue, targetValue, t);
            _fill.color = _gradient.Evaluate(_slider.normalizedValue);
            yield return null;
        }

        _slider.value = targetValue;
        _fill.color = _gradient.Evaluate(_slider.normalizedValue);

        _healthCoroutine = null;
    }

    private void SetHealthBarVisibility(bool visible)
    {
        _slider.gameObject.SetActive(visible);
        _fill.gameObject.SetActive(visible);
    }
}
