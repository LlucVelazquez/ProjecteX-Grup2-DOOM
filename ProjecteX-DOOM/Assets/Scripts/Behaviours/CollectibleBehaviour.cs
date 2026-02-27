using UnityEngine;

public class CollectibleBehaviour : MonoBehaviour
{
    [SerializeField] private ScriptableObject _item;

    private ICollectible _collectible;
    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();

        _collectible = _item as ICollectible;

        if (_item == null)
        {
            Debug.LogError($"El gameobject '{gameObject.name}' no té cap ScriptableObject assignat.");
        }
        else if (_collectible == null)
        {
            Debug.LogError($"El gameobject '{gameObject.name}': el ScriptableObject assignat no implementa ICollectible.");
        }

        if (_collider != null)
        {
            _collider.enabled = true;
            _collider.isTrigger = true;
        }
        else
        {
            Debug.LogError($"El gameobject '{gameObject.name}' no té cap collider.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_collectible == null) return;

        if (other.gameObject.layer == LayerMask.NameToLayer(GameManager.Instance.PlayerLayerName))
        {
            _collectible.Collect(gameObject, other.gameObject);
        }
    }
}