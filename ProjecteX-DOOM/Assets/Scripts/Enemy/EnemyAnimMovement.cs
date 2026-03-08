using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public class EnemyAnimMovement : MonoBehaviour
{
    [SerializeField] private float _transitionSpeed = 10f;
    [SerializeField] private string _velocityParam = "Velocity";

    private Animator _animator;
    private NavMeshAgent _agent;

    private float _currentBlendValue = 0f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        float target = _agent.velocity.magnitude;
        _currentBlendValue = Mathf.Lerp(_currentBlendValue, target, _transitionSpeed * Time.deltaTime);
        _animator.SetFloat(_velocityParam, _currentBlendValue);
    }
}
