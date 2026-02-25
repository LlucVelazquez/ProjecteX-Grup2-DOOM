using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputs))]
public class WeaponSwitchBehaviour : MonoBehaviour
{
    [SerializeField] private List<GameObject> _weapons;

    public GameObject _currentWeapon;

    public GameObject CurrentWeapon { get => _currentWeapon; }

    public IUsable GetCurrentUsable() => _currentWeapon.GetComponent<IUsable>();

    public void SwitchWeapon(int index)
    {
        _currentWeapon = _weapons[index];

        foreach(GameObject weapon in _weapons)
        {
            if (weapon == _currentWeapon)
                weapon.SetActive(true);
            else
                weapon.SetActive(false);
        }
    }

    public bool CurrentWeaponHasAmmo()
    {
        return _currentWeapon.TryGetComponent<IRefillable>(out _);
    }
}
