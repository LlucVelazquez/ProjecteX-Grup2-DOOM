using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorStillInStateBehaviourScript : MonoBehaviour
{
    [SerializeField] private string _animStateName;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public bool IsStillInState()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(_animStateName) && stateInfo.normalizedTime < 1f;
    }
}
