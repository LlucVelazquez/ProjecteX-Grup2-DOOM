using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField] private int _damage = 10;
    [SerializeField] private string _targetLayerName;

    [HideInInspector] public ProjectileFiringBehaviour shooter;

    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();

        if (_collider != null)
        {
            _collider.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(_targetLayerName))
        {
            if (other.gameObject.TryGetComponent<ITargeteable>(out ITargeteable target))
            {
                target.TakeDamage(_damage);
                ReturnToShooter();
            }
            else
            {
                Debug.LogWarning($"The object '{other.gameObject.name}' on layer '{_targetLayerName}' does not implement ITargeteable. AttackBehaviour will not function properly.");
            }
        }
    }

    private void ReturnToShooter()
    {
        gameObject.SetActive(false);
        shooter.StackPush(gameObject);
    }
}
