using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileBehaviour : MonoBehaviour
{
    [HideInInspector] public ProjectileFiringBehaviour shooter;
    [HideInInspector] public Vector3 direction;
    [HideInInspector] public float speed;
    [HideInInspector] public int damage;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _rigidbody.linearVelocity = direction.normalized * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != shooter.gameObject.layer)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer(shooter.targetLayerName))
            {
                if (other.gameObject.TryGetComponent<ITargeteable>(out ITargeteable target))
                {
                    target.TakeDamage(damage);
                }
                else
                {
                    Debug.LogWarning($"The object '{other.gameObject.name}' on layer '{shooter.targetLayerName}' does not implement ITargeteable. AttackBehaviour will not function properly.");
                }
            }
            ReturnToShooter();
        }
    }

    private void ReturnToShooter()
    {
        gameObject.SetActive(false);
        shooter.ProjectileStackPush(gameObject);
    }
}
