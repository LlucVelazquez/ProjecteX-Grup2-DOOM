using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, ITargeteable
{
    [SerializeField] private int _health = 100;
    [SerializeField] private int _armor = 0;
    [SerializeField] private int _megaarmor = 0;

    public static event Action OnPlayerDeath = delegate { };

    public int Health { get => _health; set => _health = value; }
    public int Armor { get => _armor; set => _armor = value; }
    public int Megaarmor { get => _megaarmor; set => _megaarmor = value; }

    public void TakeDamage(int damage)
    {
        float damageResult = damage;
        float damageReduction;

        if (_megaarmor > 0)
        {
            damageReduction = (float)damage / 2;
            damageResult = Mathf.Ceil(damageReduction);

            _megaarmor -= (int)damageReduction;
        }
        else if (_armor > 0)
        {
            damageReduction = (float)damage / 3;
            damageResult = Mathf.Ceil(damageReduction) * 2;

            _armor -= (int)damageReduction;
        }

        _health -= (int)damageResult;

        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnPlayerDeath.Invoke();
    }
}
