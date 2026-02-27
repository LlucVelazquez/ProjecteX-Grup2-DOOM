using UnityEngine;

[CreateAssetMenu(fileName = "ArmorItem", menuName = "Scriptable Objects/Armor Item")]
public class ArmorItemSO : ScriptableObject, ICollectible
{
    [SerializeField] private string _name;
    [SerializeField] private string _collectMsg = "Has recollit ";

    [TextArea(3, 5)]
    [SerializeField] private string _description;

    [SerializeField] private ArmorType _armorType;
    [SerializeField] private int _armorAmount;
    [SerializeField] private int _maxArmor;

    public string Name { get => _name; }
    public string CollectMessage { get => _collectMsg; }

    public void Collect(GameObject collectible, GameObject collector)
    {
        PlayerHealth pHealth = collector.GetComponent<PlayerHealth>();

        if (pHealth != null)
        {
            if (_armorType == ArmorType.Armor)
            {
                // Cannot collect normal armor if you have more mega armor than the armor limit
                if (pHealth.Megaarmor < _maxArmor)
                {
                    ArmorSetted(pHealth, collectible);

                    // Collecting normal armor overwrites megaarmor
                    pHealth.Megaarmor = 0;
                }
            }
            else if (_armorType == ArmorType.Megaarmor)
            {
                MegaarmorSetted(pHealth, collectible);

                // Collecting megaarmor overwrites normal armor
                pHealth.Armor = 0;
            }
            else if (_armorType == ArmorType.Both)
            {
                if (pHealth.Megaarmor > 0)
                {
                    MegaarmorSetted(pHealth, collectible);
                }
                else
                {
                    ArmorSetted(pHealth, collectible);
                }
            }
        }
        else
        {
            Debug.LogError("Player does not have a PlayerHealth component attached.");
        }
    }

    private void ArmorSetted(PlayerHealth pHealth, GameObject collectible)
    {
        if (pHealth.Armor >= _maxArmor) return;

        int armor = pHealth.Armor + _armorAmount;
        if (armor > _maxArmor)
        {
            armor = _maxArmor;
        }

        pHealth.Armor = armor;
        CollectibleEvents.RaiseOnCollect(CollectMessage);
        collectible.SetActive(false);
    }

    private void MegaarmorSetted(PlayerHealth pHealth, GameObject collectible)
    {
        if (pHealth.Megaarmor >= _maxArmor) return;

        int armor = pHealth.Megaarmor + _armorAmount;
        if (armor > _maxArmor)
        {
            armor = _maxArmor;
        }

        pHealth.Megaarmor = armor;
        CollectibleEvents.RaiseOnCollect(CollectMessage);
        collectible.SetActive(false);
    }
}

public enum ArmorType { Both = 0, Armor = 1, Megaarmor = 2 };