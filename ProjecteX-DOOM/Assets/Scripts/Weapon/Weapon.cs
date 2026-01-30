using UnityEngine;

public abstract class Weapon : ScriptableObject
{
    [SerializeField] protected string _name;
    [SerializeField] protected string _description;
    [SerializeField] protected float _baseDamage = 1f;
}
