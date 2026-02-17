using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputs))]
public class WeaponSwitchBehaviour : MonoBehaviour
{
    [SerializeField] private List<GameObject> _weapons;

    public GameObject _currentWeapon;

    private PlayerInputs _playerInputs;

    private void Awake()
    {
        _playerInputs = GetComponent<PlayerInputs>();
    }

    /*void Update()
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
    }*/

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
}
