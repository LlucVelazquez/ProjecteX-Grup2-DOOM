using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FollowBehaviour : MonoBehaviour
{
    private NavMeshAgent _agent;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    public void FollowTarget(Vector3 targetPosition)
    {
        _agent.SetDestination(targetPosition);

        Debug.DrawLine(transform.position, targetPosition, Color.green);
    }

    public bool ReturnToInitialPosition()
    {
        _agent.SetDestination(initialPosition);

        if (HasReachedTheDestination())
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, initialRotation, _agent.angularSpeed * Time.deltaTime);
            return true;
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
}
