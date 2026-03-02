using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, ITargeteable
{
    [SerializeField] private int _health = 10;

    public int Health { get => _health; set => _health = value; }


    public void TakeDamage(int damage)
    {
        _health -= damage;
    }
}
