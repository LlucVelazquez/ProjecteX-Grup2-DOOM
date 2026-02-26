using UnityEngine;

public class AttackBehaviour : MonoBehaviour
{
    [SerializeField] private Collider _attackCollider;
    [SerializeField] private string _targetLayerName;
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _damageInterval = 1f;

    [Tooltip("By default adds gameObject and Target Layers")]
    [SerializeField] private LayerMask _ignoreLayers;

    public float attackRange = 2f;

    public bool CanAttack { set => _attackCollider.enabled = value; }

    private ITargeteable _target;
    private Transform _targetTransform;

    private bool _isInRange;
    private float _lastDamageTime;

    private void Awake()
    {
        _attackCollider.enabled = false;
        _ignoreLayers |= (1 << gameObject.layer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(_targetLayerName))
        {
            if (other.gameObject.TryGetComponent<ITargeteable>(out ITargeteable target))
            {
                _target = target;
                _targetTransform = other.transform;
                _isInRange = true;
                _ignoreLayers |= (1 << other.gameObject.layer);

            }
            else
            {
                Debug.LogWarning($"The object '{other.gameObject.name}' on layer '{_targetLayerName}' does not implement ITargeteable. AttackBehaviour will not function properly.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(_targetLayerName))
        {
            _target = null;
            _targetTransform = null;
            _isInRange = false;
            _ignoreLayers &= ~(1 << other.gameObject.layer);
        }
    }

    public void Attack()
    {
        if (_target != null && _isInRange && IsTargetWithinTheLineOfSight())
        {
            if (Time.time - _lastDamageTime >= _damageInterval)
            {
                _target.TakeDamage(_damage);
                _lastDamageTime = Time.time;
            }
        }
    }

    private bool IsTargetWithinTheLineOfSight()
        => DistanceUtils.HasLineOfSight(transform.position, _targetTransform.position, (_targetTransform.position - transform.position).magnitude, _ignoreLayers);
}
