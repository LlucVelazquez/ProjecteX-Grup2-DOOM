using UnityEngine;
[RequireComponent(typeof(PruebaInputsPlayer))]
public class WeaponSwitching : MonoBehaviour
{
    private PruebaInputsPlayer _pruebaInputsPlayer;
    public int selectedWeapon = 0;
    private void Awake()
    {
        _pruebaInputsPlayer = GetComponent<PruebaInputsPlayer>();
    }
    void Start()
    {
        SelectWeapon();
    }

    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        if (_pruebaInputsPlayer.SelectedWeapon == 1)
        {
            if (selectedWeapon >= transform.childCount - 1)
                selectedWeapon = 0;
            else
                selectedWeapon++;
        }
        if (_pruebaInputsPlayer.SelectedWeapon == -1)
        {
            if (selectedWeapon <= 0)
                selectedWeapon = transform.childCount -1;
            else
                selectedWeapon--;
        }
        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }
    }
    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }
}
