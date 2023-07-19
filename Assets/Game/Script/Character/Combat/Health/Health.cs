using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [field: SerializeField] public float MaxHealth { get; private set; }
    
    [NonSerialized] public bool IsAlive;

    private float currentHealth;

    private void Start()
    {
        currentHealth = MaxHealth;
        IsAlive = true;
    }

    public void DealDamage(float damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        if (currentHealth <= 0)
            IsAlive = false;
        Debug.Log(currentHealth);
    }

    public void DealHealing(float heal)
    {
        currentHealth += heal;
    }
}
