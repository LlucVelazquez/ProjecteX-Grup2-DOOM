using UnityEngine;

[RequireComponent(typeof(FollowBehaviour), typeof(AttackBehaviour))]
public class Enemy : MonoBehaviour, ITargeteable
{
    [SerializeField] private float _health = 10f;

    public float Health { get => _health; set => _health = value; }

    [SerializeField] private float _attackDistance = 2f;

    private FollowBehaviour _fb;
    private AttackBehaviour _ab;

    private void Awake()
    {
        _fb = GetComponent<FollowBehaviour>();
        _ab = GetComponent<AttackBehaviour>();
    }

    private void Update()
    {
        _fb.Follow();
        Attack();
    }

    private void Attack()
    {
        if (_fb.isFollowingTarget && _fb.DistanceToTarget <= _attackDistance)
        {
            _ab.IsAttacking = true;
        }
        else
        {
            _ab.IsAttacking = false;
        }
    }
}