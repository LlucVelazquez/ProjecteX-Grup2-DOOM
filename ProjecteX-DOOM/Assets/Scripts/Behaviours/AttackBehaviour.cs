using UnityEngine;

public class AttackBehaviour : MonoBehaviour
{
    public bool CanAttack { get => _attackCollider.enabled; set => _attackCollider.enabled = value; }

    [SerializeField] private Collider _attackCollider;
    [SerializeField] private string _targetLayerName = "Player";
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _damageInterval = 1f;

    private float _lastDamageTime;
    private ITargeteable _target;
    private bool _isAttacking;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(_targetLayerName))
        {
            if (other.gameObject.TryGetComponent<ITargeteable>(out ITargeteable target))
            {
                _target = target;
                _isAttacking = true;
            }
            else
            {
                Debug.LogWarning($"The object '{other.gameObject.name}' on layer '{_targetLayerName}' does not implement ITargeteable. AttackBehaviour will not function properly.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _target = null;
        _isAttacking = false;
    }

    public void Attack()
    {
        if (_target != null)
        {
            if (_isAttacking && Time.time - _lastDamageTime >= _damageInterval)
            {
                _target.Health -= _damage;
                _lastDamageTime = Time.time;
            }
        }
        else
        {
            Debug.LogWarning("Attack target is null. AttackBehaviour will not function properly.");
        }
    }
}
