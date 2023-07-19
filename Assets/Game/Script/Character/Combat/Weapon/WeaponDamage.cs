using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [field: SerializeField] public Collider MyCollider { get; private set; }
    // [field:SerializeField] public Weapon Weapon { get; private set; }

    private List<Collider> alreadyCollidedWith = new List<Collider>();

    private float damage;

    private void OnEnable()
    {
        alreadyCollidedWith.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == MyCollider) return;

        if (alreadyCollidedWith.Contains(other)) return;

        alreadyCollidedWith.Add(other);

        if (other.TryGetComponent<Health>(out Health health))
        {
            // if (other.tag == "Enemy")
            health.DealDamage(damage);
        }
    }

    public void SetAttack(float damage)
    {
        this.damage = damage;
    }
}