using UnityEngine;

public abstract class MeleeWeapon : Weapon
{
    [SerializeField] protected Collider _collider;

    public virtual void Attack()
    {
        _collider.enabled = true;
    }
}
