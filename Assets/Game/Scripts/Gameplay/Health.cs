using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private float _maxHealth = 100;
    private float _currentHealth;

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float amount)
    {
        _currentHealth -= amount;

        //Hurt animation

        if(_currentHealth < 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy killed");
        Destroy(gameObject);
    }
}
