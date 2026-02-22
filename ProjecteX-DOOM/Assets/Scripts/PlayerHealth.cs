using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, ITargeteable
{
    [SerializeField] private float _health = 100f;
    [SerializeField] private float _armor = 0f;
    [SerializeField] private float _megaarmor = 0f;

    public static event Action OnPlayerDeath = delegate { };

    public float Health
    {
        get => _health;
        set => _health = value >= MAX_HEALTH ? MAX_HEALTH : value;
    }
    public float Armor
    {
        get => _armor;
        set => _armor = value >= MAX_ARMOR ? MAX_ARMOR : value;
    }
    public float Megaarmor
    {
        get => _megaarmor;
        set => _megaarmor = value >= MAX_MEGAARMOR ? MAX_MEGAARMOR : value;
    }

    private const float MAX_HEALTH = 200;
    private const float MAX_ARMOR = 100;
    private const float MAX_MEGAARMOR = 200;

    public void TakeDamage(float damage)
    {
        float damageResult = damage;
        float damageReduction;

        if (_megaarmor > 0f)
        {
            damageReduction = damage / 2;
            damageResult = Mathf.Ceil(damageReduction);

            _megaarmor -= (int)damageReduction;
        }
        else if (_armor > 0f)
        {
            damageReduction = damage / 3;
            damageResult = Mathf.Ceil(damageReduction) * 2;

            _armor -= (int)damageReduction;
        }

        _health -= damageResult;

        if (_health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        OnPlayerDeath.Invoke();
    }
}
