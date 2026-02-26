using UnityEngine;

public interface ITargeteable
{
    public int Health { get; set; }

    public void TakeDamage(int damage);
}
