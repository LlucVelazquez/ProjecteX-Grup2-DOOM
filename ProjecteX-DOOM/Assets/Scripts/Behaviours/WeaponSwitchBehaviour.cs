using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputs))]
public class WeaponSwitchBehaviour : MonoBehaviour, IResettable
{
    [SerializeField] private List<GameObject> _weapons;

    public GameObject CurrentWeapon { get => _currentWeapon; }

    public GameObject _currentWeapon;

    private List<GameObject> _initialWeapons;

    private void Awake()
    {
        _initialWeapons = new List<GameObject>(_weapons);
        _currentWeapon = _weapons[0];

        foreach (GameObject weapon in _weapons)
        {
            if (weapon == _currentWeapon)
                weapon.SetActive(true);
            else
                weapon.SetActive(false);
        }
    }

    private void Start()
    {
        GameManager.Instance.RegisterResettable(this);
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

    public void AddWeapon(GameObject newWeapon)
    {
        if (newWeapon == null || _weapons.Contains(newWeapon))
            return;

        _weapons.Add(newWeapon);
        newWeapon.SetActive(false);
    }

    public void OnReset(bool fullRestart)
    {
        foreach (GameObject weapon in _weapons)
        {
            weapon.SetActive(false);
        }

        _weapons = new List<GameObject>(_initialWeapons);
        _currentWeapon = _weapons[0];

        foreach (GameObject weapon in _weapons)
        {
            if (weapon == _currentWeapon)
                weapon.SetActive(true);
            else
                weapon.SetActive(false);
        }
    }
}
