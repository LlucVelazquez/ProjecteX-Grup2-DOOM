using UnityEngine;

public abstract class RangedWeapon : Weapon
{
    [SerializeField] protected float _range = 100f;
    [SerializeField] protected int _ammunition = 0;

    public virtual void Shoot(Transform shootPoint, Camera cam)
    {
        if (_ammunition <= 0) return;
        _ammunition--;
    }
}
