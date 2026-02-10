using UnityEngine;

public interface ITargeteable
{
    public float Health { get; set; }

    public void TakeDamage(float damage)
    {
        Health -= damage;
    
        if (Health <= 0f)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log("Target Die: " + (this as MonoBehaviour)?.gameObject.name);
    }
}
