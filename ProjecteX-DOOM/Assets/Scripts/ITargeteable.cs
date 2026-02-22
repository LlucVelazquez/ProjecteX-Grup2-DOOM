using UnityEngine;

public interface ITargeteable
{
    public int Health { get; set; }

    public void TakeDamage(int damage)
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
