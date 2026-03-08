using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimAttack : MonoBehaviour
{
    [SerializeField] private string _attackParam = "Attack";

    private Animator _animator;

    private void OnEnable()
    {
        if (TryGetComponent<AttackBehaviour>(out var ab)) ab.OnAttack += PlayAttack;
    }

    private void OnDisable()
    {
        if (TryGetComponent<AttackBehaviour>(out var ab)) ab.OnAttack -= PlayAttack;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayAttack() => _animator.SetTrigger(_attackParam);
}
