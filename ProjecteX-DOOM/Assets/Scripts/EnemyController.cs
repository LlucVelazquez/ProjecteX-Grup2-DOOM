using UnityEngine;

[RequireComponent(typeof(EnemyHealth), typeof(FollowBehaviour), typeof(AttackBehaviour))]
public class EnemyController : MonoBehaviour
{
    // In chase and attack states the enmey contiunes shooting
    // And in attack state continues chasing
    private enum State { Idle, Return, Chase, Attack, Die }

    private EnemyHealth _health;

    private FollowBehaviour _fb;
    private AttackBehaviour _ab;

    private State _currentState;

    private void Awake()
    {
        _health = GetComponent<EnemyHealth>();

        _fb = GetComponent<FollowBehaviour>();
        _ab = GetComponent<AttackBehaviour>();

        _currentState = State.Idle;
    }

    private void Update()
    {
        switch (_currentState)
        {
            case State.Idle:
                Idle();
                break;

            case State.Return:
                Return();
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
        if (_health.Health <= 0)
        {
            _currentState = State.Die;
        }

        if (_fb.IsTargetWithinTheLineOfSight())
        {
            _currentState = State.Chase;
        }
    }

    private void Return()
    {
        if (_health.Health <= 0)
        {
            _currentState = State.Die;
        }

        if (_fb.IsTargetWithinTheLineOfSight())
        {
            _currentState = State.Chase;
        }

        bool returned = _fb.ReturnToInitialPosition();
        if (returned && !_fb.IsTargetWithinTheLineOfSight())
        {
            _currentState = State.Idle;
        }
    }

    private void Chase()
    {
        if (_health.Health <= 0)
        {
            _currentState = State.Die;
        }

        if (!_fb.IsTargetWithinTheLineOfSight())
        {
            _currentState = State.Return;
        }

        _fb.FollowTarget();

        if (_currentState != State.Attack && DistanceUtils.IsInRange(transform.position, _fb.TargetPosition, _ab.attackRange))
        {
            _currentState = State.Attack;
            _ab.CanAttack = true;
        }
    }

    private void Attack()
    {
        if (!DistanceUtils.IsInRange(transform.position, _fb.TargetPosition, _ab.attackRange))
        {
            _currentState = State.Chase;
            _ab.CanAttack = false;
        }

        Chase();
        _ab.Attack();
    }

    public void Die()
    {
        _fb.StopFollow();

        Debug.Log("Enemy Die");
    }
}