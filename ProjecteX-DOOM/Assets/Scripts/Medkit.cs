using UnityEngine;

public class Medkit : MonoBehaviour, ICollectible
{
    public string Name { get => _name; set => _name = value; }

    [SerializeField] private string _name = "Medkit";
    [SerializeField] private int _heal = 25;

    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();

        if (_collider != null)
        {
            _collider.enabled = true;
            _collider.isTrigger = true;
        }
        else
        {
            Debug.LogError($"The GameObject {gameObject.name} does not have a collider attached.");
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
            pHealth.Health += _heal;

            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Player does not have a PlayerHealth component attached.");
        }
    }
}
