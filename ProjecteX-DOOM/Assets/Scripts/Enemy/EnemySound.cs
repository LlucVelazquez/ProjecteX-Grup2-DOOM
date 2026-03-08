using UnityEngine;

public class EnemySound : MonoBehaviour
{
    [SerializeField] private SoundType _sightSound;
    [SerializeField] private SoundType _attackSound;
    [SerializeField] private SoundType _deathSound;
    [SerializeField] private SoundType _hurtSound;

    private void OnEnable()
    {
        if (TryGetComponent<AttackBehaviour>(out var ab)) ab.OnAttack += PlayAttack;
        if (TryGetComponent<EnemyHealth>(out var health)) health.OnDamaged += PlayHurt;
    }

    private void OnDisable()
    {
        if (TryGetComponent<AttackBehaviour>(out var ab)) ab.OnAttack -= PlayAttack;
        if (TryGetComponent<EnemyHealth>(out var health)) health.OnDamaged -= PlayHurt;
    }

    public void PlaySight() => AudioManager.Instance.PlaySoundAtPoint(_sightSound, transform.position);
    public void PlayAttack() => AudioManager.Instance.PlaySoundAtPoint(_attackSound, transform.position);
    public void PlayDeath() => AudioManager.Instance.PlaySoundAtPoint(_deathSound, transform.position);
    public void PlayHurt() => AudioManager.Instance.PlaySoundAtPoint(_hurtSound, transform.position);
}
