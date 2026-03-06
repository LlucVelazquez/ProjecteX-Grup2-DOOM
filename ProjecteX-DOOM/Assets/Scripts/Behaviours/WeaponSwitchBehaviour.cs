using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputs))]
public class WeaponSwitchBehaviour : MonoBehaviour
{
    [SerializeField] private List<GameObject> _weapons;

    public GameObject CurrentWeapon { get => _currentWeapon; }

    public GameObject _currentWeapon;

    private void Awake()
    {
        _currentWeapon = _weapons[0];

        foreach (GameObject weapon in _weapons)
        {
            if (weapon == _currentWeapon)
                weapon.SetActive(true);
            else
                weapon.SetActive(false);
        }
    }

    public IUsable GetCurrentUsable() => _currentWeapon.GetComponent<IUsable>();

    public bool CanSwitchWeapon() => !_currentWeapon.TryGetComponent<IUsable>(out var usable) || !usable.Using;

    public void SwitchWeapon(int index)
    {
        if (index < _weapons.Count)
        {
            if (_currentWeapon.TryGetComponent<IUsable>(out var usable) && usable.Using)
                return;

            _currentWeapon = _weapons[index];

            foreach (GameObject weapon in _weapons)
            {
                if (weapon == _currentWeapon)
                    weapon.SetActive(true);
                else
                    weapon.SetActive(false);
            }
        }
    }
}
