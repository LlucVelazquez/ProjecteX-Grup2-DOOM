using UnityEngine;

[RequireComponent(typeof(FollowBehaviour), typeof(AttackBehaviour))]
public class Enemy : MonoBehaviour, ITargeteable
{
    [SerializeField] private int _health = 10;

    public int Health { get => _health; set => _health = value; }

    [SerializeField] private float _attackDistance = 2f;

    // In chase and attack states the enmey contiunes shooting
    // And in attack state continues chasing
    private enum EnemyState { Idle, Chase, Attack, Die }

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
        if (_fb.isFollowingTarget && _fb.GetDistanceToTarget() <= _attackDistance)
        {
            _ab.CanAttack = true;
        }
        else
        {
            _ab.CanAttack = false;
        }
    }
}