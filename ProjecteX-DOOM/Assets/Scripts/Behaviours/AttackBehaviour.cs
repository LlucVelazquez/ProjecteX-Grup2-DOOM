using UnityEngine;

public class AttackBehaviour : MonoBehaviour
{
    public bool IsAttacking { get => _attackCollider.enabled; set => _attackCollider.enabled = value; }

    [SerializeField] private Collider _attackCollider;
    [SerializeField] private string _targetLayerName = "Player";
    [SerializeField] private float _damage = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(_targetLayerName))
        {
            if (other.TryGetComponent<ITargeteable>(out ITargeteable target))
            {
                Attack(target);
            }
            else
            {
                Debug.LogWarning($"The object '{other.gameObject.name}' on layer '{_targetLayerName}' does not implement ITargeteable. AttackBehaviour will not function properly.");
            }
        }
    }

    public void Attack(ITargeteable target)
    {
        if (target != null)
        {
            target.Health -= _damage;
        }
        else
        {
            Debug.LogWarning("Attack target is null. AttackBehaviour will not function properly.");
        }
    }
}
