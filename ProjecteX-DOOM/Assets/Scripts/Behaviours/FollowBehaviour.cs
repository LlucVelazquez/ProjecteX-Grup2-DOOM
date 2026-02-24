using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FollowBehaviour : MonoBehaviour
{
    [Header("Follow Settings")]
    [SerializeField] private GameObject _followTarget;
    [SerializeField] private float _followDistance = 10f;

    [Tooltip("By default adds gameObject and Target Layers")]
    [SerializeField] private LayerMask _ignoreLayers;

    [HideInInspector] public bool isFollowingTarget = false;

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

    public void FollowTarget()
    {
        if (IsFollowTargetNotNull())
        {
            _agent.SetDestination(_followTarget.transform.position);

            Debug.DrawLine(transform.position, _followTarget.transform.position, Color.green);
        }
    }

    public void ReturnToInitialPosition()
    {
        if (IsFollowTargetNotNull())
        {
            _agent.SetDestination(initialPosition);

            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, initialRotation, _agent.angularSpeed * Time.deltaTime);
            }
        }
    }

    public bool IsTargetInRange()
    {
        return GetDistanceToTarget() <= _followDistance;
    }

    public bool IsTargetWithinTheVision()
    {
        Vector3 direction = (_followTarget.transform.position - transform.position).normalized;
        LayerMask layerMask = ~_ignoreLayers;

        if (Physics.Raycast(transform.position, direction, GetDistanceToTarget(), layerMask))
        {
            return false;
        }
        return true;
    }

    public float GetDistanceToTarget()
    {
        return Vector3.Distance(_followTarget.transform.position, transform.position);
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
