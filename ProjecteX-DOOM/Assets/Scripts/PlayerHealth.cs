using UnityEngine;

public class PlayerHealth : MonoBehaviour, ITargeteable
{
    [SerializeField] private float _health = 100f;

    public float Health { get => _health; set => _health = value; }

    public void TakeDamage(float damage)
    {
        _health -= damage;

        if (_health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Target Die: " + gameObject.name);
    }
}
