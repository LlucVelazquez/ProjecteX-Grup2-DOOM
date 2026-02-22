using UnityEngine;

public class HealPlayer : MonoBehaviour, ICollectible
{
    public string Name { get => _healthItem.name; }

    [SerializeField] private HealthItemSO _healthItem;

    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();

        if (_healthItem == null) Debug.LogError($"The collectible item '{gameObject.name}' does not have a HealItemSO.");

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
            if (pHealth.Health >= _healthItem.maxHealth) return;

            int health = pHealth.Health + _healthItem.healAmount;
            if (health > _healthItem.maxHealth)
            {
                health = _healthItem.maxHealth;
            }

            pHealth.Health = health;

            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Player does not have a PlayerHealth component attached.");
        }
    }
}
