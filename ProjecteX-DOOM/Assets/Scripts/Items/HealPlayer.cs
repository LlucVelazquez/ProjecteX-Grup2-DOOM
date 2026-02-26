using UnityEngine;

[RequireComponent(typeof(ReiniciableBehaviour))]
public class HealPlayer : MonoBehaviour, ICollectible
{
    [SerializeField] private HealthItemSO _healthItem;

    public string Name { get => _healthItem.name; }
    public string CollectMessage { get => $"Has recollit {Name}"; }

    private ReiniciableBehaviour _rb;
    private Collider _collider;

    private void Awake()
    {
        _rb = GetComponent<ReiniciableBehaviour>();

        if (_rb != null)
        {
            _rb.SetValues(_collider, GetComponent<MeshRenderer>(), GetComponentsInChildren<MeshRenderer>());
        }
        else
        {
            Debug.LogError($"The GameObject '{gameObject.name}' does not have a ReiniciableBehaviour attached.");
        }
        
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
            Collect(other.gameObject);
        }
    }

    public void Collect(GameObject collector)
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

            CollectibleEvents.RaiseOnCollect(CollectMessage);

            //gameObject.SetActive(false);
            _rb.DisableObject();
        }
        else
        {
            Debug.LogError("Player does not have a PlayerHealth component attached.");
        }
    }
}
