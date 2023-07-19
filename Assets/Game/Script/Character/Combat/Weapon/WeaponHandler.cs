using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [field: SerializeField] public GameObject WeaponCollider { get; private set; }

    public void EnableWeapon()
    {
        WeaponCollider.SetActive(true);
    }

    public void DisableWeapon()
    {
        WeaponCollider.SetActive(false);
    }
}
