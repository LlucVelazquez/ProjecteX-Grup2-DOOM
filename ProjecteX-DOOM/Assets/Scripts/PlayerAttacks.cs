using UnityEngine;

[RequireComponent(typeof(PlayerInputs), typeof(WeaponSwitchBehaviour))]
public class PlayerAttacks : MonoBehaviour
{
    private PlayerInputs _playerInputs;
    private WeaponSwitchBehaviour _weaponHolder;

    private void Awake()
    {
        _playerInputs = GetComponent<PlayerInputs>();
        _weaponHolder = GetComponent<WeaponSwitchBehaviour>();
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
        if (_weaponHolder._currentWeapon.TryGetComponent<ShotgunController>(out var shotgun))
        {
            shotgun.Shoot();
        }
        else
        {
            Debug.LogError("Current weapon does not have a Existent Weapon component.");
        }
    }
}
