using UnityEngine;

public class Sword : MonoBehaviour
{
    public Animator animator;
    public Collider swordCollider;
    private void Awake()
    {
        swordCollider = GetComponent<Collider>();
        swordCollider.enabled = false;
        swordCollider.isTrigger = true;
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        ITargeteable target = other.GetComponent<ITargeteable>();
        if (target != null)
        {
            target.TakeDamage(10); // You can adjust the damage value as needed
        }
    }
    private void Attack()
    {
        animator.SetTrigger("Attack");
    }
    public void StartAttack()
    {
        swordCollider.enabled = true;
    }
    public void EndAttack()
    {
        swordCollider.enabled = false;
    }
}
