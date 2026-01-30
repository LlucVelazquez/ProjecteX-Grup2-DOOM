using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamageArea : MonoBehaviour
{
    [SerializeField] private float _damage = 10f;

    private Collider collider;

    private void Awake()
    {
        collider = GetComponent<Collider>();
        collider.isTrigger = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collider.gameObject.layer == 10)
        {
            
        }
    }

    private void DamagePlayer()
    {
        PlayerHealth player = collider.gameObject.GetComponent<PlayerHealth>();

        if (player != null)
        {
            player.TakeDamage(_damage);
        }
    }
}
