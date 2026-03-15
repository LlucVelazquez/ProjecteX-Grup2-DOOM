using UnityEngine;
using UnityEngine.Events;

public class TriggerBehaviour : MonoBehaviour, ICollectible, IResettable
{
    [SerializeField] private string _name;
    [SerializeField] private string _collectMsg = "Has recollit ";
    [SerializeField] private UnityEvent _collectEvent;
    [SerializeField] private UnityEvent _resetEvent;

    public string Name { get => _name; }
    public string CollectMessage { get => _collectMsg; }

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
            Debug.LogError($"El gameobject '{gameObject.name}' no té cap collider.");
        }
    }

    private void Start()
    {
        GameManager.Instance.RegisterResettable(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(GameManager.Instance.PlayerLayerName))
        {
            Collect(gameObject, other.gameObject);
        }
    }

    public void Collect(GameObject collectible, GameObject collector)
    {
        AudioManager.Instance.PlaySound(SoundType.SecretFound);
        _collectEvent.Invoke();
        gameObject.SetActive(false);
        CollectibleEvents.RaiseOnCollect(CollectMessage);
    }

    public void OnReset(bool fullRestart)
    {
        _resetEvent.Invoke();
        gameObject.SetActive(true);
    }
}
