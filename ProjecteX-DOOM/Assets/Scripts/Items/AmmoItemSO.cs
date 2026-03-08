using UnityEngine;

[CreateAssetMenu(fileName = "AmmoItem", menuName = "Scriptable Objects/Ammo Item")]
public class AmmoItemSO : ScriptableObject, ICollectible
{
    [SerializeField] private string _name;
    [SerializeField] private string _collectMsg = "Has recollit ";

    [TextArea(3, 5)]
    [SerializeField] private string _description;

    [SerializeField] private WeaponType _weaponAmmo;
    [SerializeField] private int _ammoAmount;

    public string Name { get => _name; }
    public string CollectMessage { get => _collectMsg; }

    public void Collect(GameObject collectible, GameObject collector)
    {
        IRefillable[] weapons = collector.GetComponentsInChildren<IRefillable>();

        if (weapons != null && weapons.Length > 0)
        {
            foreach (IRefillable weapon in weapons)
            {
                if (weapon.Weapon == _weaponAmmo)
                {
                    weapon.AddAmmunition(_ammoAmount);
                    AudioManager.Instance.PlaySound(SoundType.AmmoItem);
                    collectible.SetActive(false);
                    CollectibleEvents.RaiseOnCollect(CollectMessage);
                    return;
                }
            }
        }
        else
        {
            Debug.LogWarning($"No s'ha trobat cap arma de tipus {_weaponAmmo} al collector!");
        }
    }
}