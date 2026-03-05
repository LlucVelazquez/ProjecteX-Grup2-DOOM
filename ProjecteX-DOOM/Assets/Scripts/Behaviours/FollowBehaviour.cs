using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FollowBehaviour : MonoBehaviour
{
    [Header("Follow Settings")]
    [SerializeField] private GameObject _followTarget;
    [SerializeField] private float _followRange = 10f;

    [Tooltip("By default adds gameObject and Target Layers")]
    [SerializeField] private LayerMask _ignoreLayers;

    public Vector3 TargetPosition { get => _followTarget.transform.position; }

    private NavMeshAgent _agent;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _ignoreLayers |= (1 << gameObject.layer) | (1 << _followTarget.layer);

        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    public bool IsTargetWithinTheLineOfSight()
        => DistanceUtils.HasLineOfSight(transform.position, _followTarget.transform.position, _followRange, _ignoreLayers);

    public void FollowTarget()
    {
        if (IsFollowTargetNotNull())
        {
            _agent.SetDestination(_followTarget.transform.position);

            Debug.DrawLine(transform.position, _followTarget.transform.position, Color.green);
        }
    }

    public bool ReturnToInitialPosition()
    {
        if (IsFollowTargetNotNull())
        {
            _agent.SetDestination(initialPosition);

            if (HasReachedTheDestination())
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, initialRotation, _agent.angularSpeed * Time.deltaTime);
                return true;
            }
        }
        return false;
    }

    public void StopFollow()
    {
        _agent.ResetPath();
        _agent.velocity = Vector3.zero;
    }

    private bool HasReachedTheDestination()
    {
        return _agent.remainingDistance <= _agent.stoppingDistance;
    }

    private bool IsFollowTargetNotNull()
    {
        if (_followTarget != null)
        {
            return true;
        }
        else
        {
            Debug.LogWarning("Follow target is null. FollowBehaviour will not function properly.");
            return false;
        }
    }
}
