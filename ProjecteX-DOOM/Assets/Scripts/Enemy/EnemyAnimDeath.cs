using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimDeath : MonoBehaviour
{
    [SerializeField] private string _deathParam = "Die";

    private Animator _animator;

    private void OnEnable() => GetComponent<EnemyHealth>().OnEnemyDeath += PlayDeath;
    private void OnDisable() => GetComponent<EnemyHealth>().OnEnemyDeath -= PlayDeath;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayDeath(bool die)
    {
        _animator.SetBool(_deathParam, die);
    }
}
