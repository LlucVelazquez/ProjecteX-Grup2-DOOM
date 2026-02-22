using UnityEngine;

[RequireComponent(typeof(FollowBehaviour), typeof(AttackBehaviour))]
public class Enemy : MonoBehaviour, ITargeteable
{
    [SerializeField] private int _health = 10;

    public int Health { get => _health; set => _health = value; }

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
        CheckIfCanAttack();
        _ab.Attack();
    }

    private void CheckIfCanAttack()
    {
        if (_fb.isFollowingTarget && _fb.DistanceToTarget <= _attackDistance)
        {
            _ab.CanAttack = true;
        }
        else
        {
            _ab.CanAttack = false;
        }
    }
}