using System;
using UnityEngine;

[RequireComponent(typeof(EnemyHealth), typeof(EnemySound))]
[RequireComponent(typeof(TargetDetectionBehaviour))]
public class EnemyController : MonoBehaviour, IResettable
{
    // In chase and attack states the enmey contiunes shooting
    // And in attack state continues chasing
    private enum State { Idle, Return, Chase, Attack, Die }

    private EnemyHealth _health;
    private EnemySound _sound;

    private TargetDetectionBehaviour _tdb;

    private FollowBehaviour _fb;
    private AttackBehaviour _ab;
    private ProjectileFiringBehaviour _pfb;
    private RotateToTargetBehaviour _rtb;

    private State _currentState;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private bool _sightSoundPlayed = false;

    private void Awake()
    {
        _health = GetComponent<EnemyHealth>();
        _sound = GetComponent<EnemySound>();

        _tdb = GetComponent<TargetDetectionBehaviour>();

        _fb = GetComponent<FollowBehaviour>();
        _ab = GetComponent<AttackBehaviour>();
        _pfb = GetComponent<ProjectileFiringBehaviour>();
        _rtb = GetComponent<RotateToTargetBehaviour>();

        _currentState = State.Idle;
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
    }

    private void Start()
    {
        GameManager.Instance.RegisterResettable(this);
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

        if (_tdb.IsTargetWithinTheLineOfSight())
        {
            _currentState = State.Chase;
        }
    }

    private void Return()
    {
        _sightSoundPlayed = false;

        if (_health.Health <= 0)
        {
            _currentState = State.Die;
        }

        if (_tdb.IsTargetWithinTheLineOfSight())
        {
            _currentState = State.Chase;
        }

        if (_fb != null)
        {
            bool returned = _fb.ReturnToInitialPosition();
            if (returned && !_tdb.IsTargetWithinTheLineOfSight())
            {
                _currentState = State.Idle;
            }
        }
    }

    private void Chase()
    {
        if (!_sightSoundPlayed)
        {
            _sound.PlaySight();
            _sightSoundPlayed = true;
        }

        if (_health.Health <= 0)
        {
            _currentState = State.Die;
        }

        if (!_tdb.IsTargetWithinTheLineOfSight())
        {
            _currentState = State.Return;
        }

        if (_fb != null) _fb.FollowTarget(_tdb.TargetPosition);
        if (_rtb != null) _rtb.RotateTowards(_tdb.TargetPosition);

        if (_currentState != State.Attack)
        {
            if (_pfb != null) _pfb.Shoot(DistanceUtils.GetDirection(_pfb.ShootPointPosition, _tdb.TargetPosition));

            if (_ab != null && DistanceUtils.IsInRange(transform.position, _tdb.TargetPosition, _ab.attackRange))
            {
                _currentState = State.Attack;
                _ab.CanAttack = true;
            }
        }
    }

    private void Attack()
    {
        if (_ab == null) { _currentState = State.Chase; return; }

        if (!DistanceUtils.IsInRange(transform.position, _tdb.TargetPosition, _ab.attackRange))
        {
            _currentState = State.Chase;
            _ab.CanAttack = false;
            return;
        }

        Chase();
        _ab.Attack();
    }

    public void Die()
    {
        _sound.PlayDeath();
        if (_fb != null) _fb.StopFollow();

        gameObject.SetActive(false);
    }

    public void OnReset(bool fullRestart)
    {
        transform.SetPositionAndRotation(_initialPosition, _initialRotation);
        _health.ResetHealth();
        _sightSoundPlayed = false;
        _currentState = State.Idle;
        gameObject.SetActive(true);
    }
}