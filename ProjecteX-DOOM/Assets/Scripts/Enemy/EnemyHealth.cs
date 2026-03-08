using System;
using UnityEngine;

[RequireComponent(typeof(EnemySound))]
public class EnemyHealth : MonoBehaviour, ITargeteable
{
    [SerializeField] private int _health = 10;

    public int Health { get => _health; set => _health = value; }

    public event Action OnDamaged;
    public event Action<bool> OnEnemyDeath;

    private int _initialHealth;

    private void Awake()
    {
        _initialHealth = _health;
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        
        if (_health <= 0)
        {
            OnEnemyDeath?.Invoke(true);
        }
        else
        {
            OnDamaged?.Invoke();
        }
    }

    public void ResetHealth()
    {
        _health = _initialHealth;
        OnEnemyDeath?.Invoke(false);
    }
}
