using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PruebaInputsPlayer))]
public class WeaponSwitching : MonoBehaviour
{
    [SerializeField] private List<GameObject> _weapons;

    private PlayerInputs _playerInputs;

    private GameObject _currentWeapon;

    public int selectedWeapon = 0;

    private void Awake()
    {
        _playerInputs = GetComponent<PlayerInputs>();
    }

    void Update()
    {
        int index = _playerInputs.SelectedWeapon - 1;

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
