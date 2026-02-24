using UnityEngine;

[RequireComponent(typeof(FollowBehaviour), typeof(AttackBehaviour))]
public class Enemy : MonoBehaviour, ITargeteable
{
    [SerializeField] private int _health = 10;

    public int Health { get => _health; set => _health = value; }

    [SerializeField] private float _attackDistance = 2f;

    // In chase and attack states the enmey contiunes shooting
    // And in attack state continues chasing
    private enum State { Idle, Chase, Attack, Die }

    private FollowBehaviour _fb;
    private AttackBehaviour _ab;

    private State _currentState = State.Idle;

    private void Awake()
    {
        _fb = GetComponent<FollowBehaviour>();
        _ab = GetComponent<AttackBehaviour>();
    }

    private void Update()
    {
        /*_fb.Follow();
        CheckIfCanAttack();
        _ab.Attack();*/

        switch (_currentState)
        {
            case State.Idle:
                Idle();
                break;
            case State.Chase:
                Chase();
                break;
            case State.Attack:
                Attack();
                break;
            case State.Die:
                Die();
                break;
            default:
                _currentState = State.Idle;
                break;
        }
    }

    private void Idle()
    {
        if (_fb.IsTargetInRange() && _fb.IsTargetWithinTheVision())
        {
            _currentState = State.Chase;
        }
    }

    private void Chase()
    {

    }

    private void Attack()
    {

    }

    private void Die()
    {

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