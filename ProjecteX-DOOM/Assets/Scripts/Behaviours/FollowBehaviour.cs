using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FollowBehaviour : MonoBehaviour
{
    public float DistanceToTarget { get => Vector3.Distance(_followTarget.transform.position, transform.position); }

    [Header("Follow Settings")]
    [SerializeField] private GameObject _followTarget;
    [SerializeField] private float _followDistance = 10f;

    [HideInInspector] public bool isFollowingTarget = false;

    private NavMeshAgent _agent;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    public void Follow()
    {
        isFollowingTarget = DistanceToTarget <= _followDistance;

        if (_followTarget != null)
        {
            if (isFollowingTarget)
            {
                _agent.SetDestination(_followTarget.transform.position);
            }
            else
            {
                _agent.SetDestination(initialPosition);

                if (_agent.remainingDistance <= _agent.stoppingDistance)
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, initialRotation, _agent.angularSpeed * Time.deltaTime);
                }
            }
        }
        else
        {
            Debug.LogWarning("Follow target is null. FollowBehaviour will not function properly.");
        }

        Debug.DrawLine(transform.position, _followTarget.transform.position, Color.red);
    }
}
