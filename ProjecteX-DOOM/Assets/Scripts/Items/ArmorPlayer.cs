using UnityEngine;

public class ArmorPlayer : MonoBehaviour, ICollectible
{
    public string Name { get => _armorItem.name; }
    
    [SerializeField] private ArmorItemSO _armorItem;

    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();

        if (_armorItem == null) Debug.LogError($"The collectible item '{gameObject.name}' does not have a ArmorItemSO.");

        if (_collider != null)
        {
            _collider.enabled = true;
            _collider.isTrigger = true;
        }
        else
        {
            Debug.LogError($"The GameObject '{gameObject.name}' does not have a collider attached.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(GameManager.Instance.PlayerLayerName))
        {
            OnCollect(other.gameObject);
        }
    }

    public void OnCollect(GameObject collector)
    {
        PlayerHealth pHealth = collector.GetComponent<PlayerHealth>();

        if (pHealth != null)
        {
            if (_armorItem.armorType == ArmorType.Armor)
            {
                // Cannot collect normal armor if you have more mega armor than the armor limit
                if (pHealth.Megaarmor < _armorItem.maxArmor)
                {
                    SetArmor(pHealth);

                    // Collecting normal armor overwrites megaarmor
                    pHealth.Megaarmor = 0;
                }
            }
            else if (_armorItem.armorType == ArmorType.Megaarmor)
            {
                SetMegaarmor(pHealth);

                // Collecting megaarmor overwrites normal armor
                pHealth.Armor = 0;
            }
            else if(_armorItem.armorType == ArmorType.Both)
            {
                if (pHealth.Megaarmor > 0)
                {
                    SetMegaarmor(pHealth);
                }
                else
                {
                    SetArmor(pHealth);
                }
            }
        }
        else
        {
            Debug.LogError("Player does not have a PlayerHealth component attached.");
        }
    }

    private void SetArmor(PlayerHealth pHealth)
    {
        if (pHealth.Armor >= _armorItem.maxArmor) return;

        int armor = pHealth.Armor + _armorItem.armorAmount;
        if (armor > _armorItem.maxArmor)
        {
            armor = _armorItem.maxArmor;
        }

        pHealth.Armor = armor;

        gameObject.SetActive(false);
    }

    private void SetMegaarmor(PlayerHealth pHealth)
    {
        if (pHealth.Megaarmor >= _armorItem.maxArmor) return;

        int armor = pHealth.Megaarmor + _armorItem.armorAmount;
        if (armor > _armorItem.maxArmor)
        {
            armor = _armorItem.maxArmor;
        }

        pHealth.Megaarmor = armor;

        gameObject.SetActive(false);
    }
}
