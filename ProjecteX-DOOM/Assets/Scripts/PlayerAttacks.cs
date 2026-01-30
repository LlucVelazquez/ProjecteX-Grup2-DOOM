using NUnit.Framework;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(PlayerInputs))]
public class PlayerAttacks : MonoBehaviour
{
    [Header("Player First Person Camera")]
    [SerializeField] private CinemachineCamera _playerCamera;

    [SerializeField] private Weapon[] _allWeapons;
    [SerializeField] private List<Weapon> _collectedWeapons;

    private PlayerInputs _playerInputs;
    private Weapon _currentWeapon;

    private void Awake()
    {
        _playerInputs = GetComponent<PlayerInputs>();
    }

    private void Update()
    {
        if (_playerInputs.Attack)
        {
            if (_currentWeapon is RangedWeapon rangedWeapon)
            {
                rangedWeapon.Shoot(transform, _playerCamera.GetComponent<Camera>());
            }
            else if (_currentWeapon is MeleeWeapon meleeWeapon)
            {
                meleeWeapon.Attack();
            }
        }
    }
}
