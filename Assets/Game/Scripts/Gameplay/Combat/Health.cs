using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private GameObject _floatingPoints;

    private float _maxHealth = 100;
    private float _currentHealth;

    public Action OnTakeDamage;
    public Action OnDeath;

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float amount)
    {
        _currentHealth -= amount;

        GameObject points = Instantiate(_floatingPoints, transform.position, Quaternion.identity);
        points.transform.GetChild(0).GetComponent<TextMesh>().text = amount.ToString();

        OnTakeDamage?.Invoke();

        if(_currentHealth < 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
    }
}
