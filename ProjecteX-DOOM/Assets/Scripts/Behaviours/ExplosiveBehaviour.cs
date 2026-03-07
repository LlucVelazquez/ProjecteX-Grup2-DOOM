using UnityEngine;

public class ExplosiveBehaviour : MonoBehaviour
{
    [SerializeField] private float _explosionRadius = 4f;

    [Tooltip("El dom utilitza unitats de mapa, en comptes de metres, 1 metre = 32 unitats de mapa")]
    [SerializeField] private float _units = 32f;
    [SerializeField] private GameObject _explosion;
    [SerializeField] private LayerMask _targetLayers;
    [SerializeField] private LayerMask _obstacleLayers;

    public void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRadius, _targetLayers);

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent<ITargeteable>(out var target))
            {
                Vector3 delta = hit.bounds.center - transform.position;
                float chebyshevDist = Mathf.Max(Mathf.Abs(delta.x), Mathf.Abs(delta.y));
                float objectRadius = Mathf.Max(hit.bounds.extents.x, hit.bounds.extents.y);
                float effectiveDist = Mathf.Max(chebyshevDist - objectRadius, 0f);

                if (effectiveDist < _explosionRadius)
                {
                    Vector3 direction = hit.bounds.center - transform.position;
                    float distance = direction.magnitude;
                    LayerMask rayMask = _obstacleLayers | _targetLayers;

                    bool blockedByObstacle = Physics.Raycast(transform.position, direction.normalized, out RaycastHit rayHit, distance, rayMask)
                                            && rayHit.collider != hit;

                    if (!blockedByObstacle)
                    {
                        float mapRadius = _explosionRadius * _units;
                        float mapEffectiveDist = effectiveDist * _units;
                        int damage = (int)Mathf.Ceil(mapRadius - mapEffectiveDist);
                        target.TakeDamage(damage);
                    }
                }
            }
        }

        AudioManager.Instance.PlaySound(SoundType.Explosion);
        Instantiate(_explosion, transform.position, Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}