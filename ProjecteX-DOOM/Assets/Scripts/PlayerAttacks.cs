using NUnit.Framework;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(PlayerInputs), typeof(WeaponsHolder))]
public class PlayerAttacks : MonoBehaviour
{
    private PlayerInputs _playerInputs;
    private WeaponsHolder _weaponHolder;

    private void Awake()
    {
        _playerInputs = GetComponent<PlayerInputs>();
        _weaponHolder = GetComponent<WeaponsHolder>();
    }

    private void Update()
    {
        if (_playerInputs.Attack)
        {
            Attack();
        }
    }

    private void Attack()
    {
        // Foreach weapon type, check if the current weapon has the corresponding component and call its Attack/Shoot method
        if (_weaponHolder._currentWeapon.TryGetComponent<Shotgun>(out var shotgun))
        {
            shotgun.Shoot();
        }
        else
        {
            Debug.LogError("Current weapon does not have a Existent Weapon component.");
        }
    }
}
