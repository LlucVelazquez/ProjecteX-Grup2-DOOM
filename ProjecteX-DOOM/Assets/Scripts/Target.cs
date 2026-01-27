using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private float health = 10f;

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Target Die: " + gameObject.name);
    }
}
