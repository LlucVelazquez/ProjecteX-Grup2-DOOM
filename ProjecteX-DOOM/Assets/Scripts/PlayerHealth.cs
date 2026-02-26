using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, ITargeteable
{
    [SerializeField] private int _health = 100;
    [SerializeField] private int _armor = 0;
    [SerializeField] private int _megaarmor = 0;

    public static event Action<int> OnHealthChange;
    public static event Action<int> OnArmorChange;
    public static event Action<int> OnMegaarmorChange;
    public static event Action OnPlayerDeath;

    public int Health
    {
        get => _health;
        set
        {
            _health = value;
            OnHealthChange?.Invoke(value);
        }
    }
    public int Armor
    {
        get => _armor;
        set
        {
            _armor = value;
            if (value != 0) OnArmorChange?.Invoke(value);
        }
    }
    public int Megaarmor
    {
        get => _megaarmor;
        set
        {
            _megaarmor = value;
            if (value != 0) OnMegaarmorChange?.Invoke(value);
        }
    }

    private void Start()
    {
        OnHealthChange?.Invoke(_health);
        OnArmorChange?.Invoke(_armor);
    }

    public void TakeDamage(int damage)
    {
        float damageResult = damage;
        float damageReduction;

        if (Megaarmor > 0)
        {
            damageReduction = (float)damage / 2;
            damageResult = Mathf.Ceil(damageReduction);

            Megaarmor -= (int)Mathf.Ceil(damageReduction);
        }
        else if (Armor > 0)
        {
            damageReduction = (float)damage / 3;
            damageResult = Mathf.Ceil(damageReduction * 2);

            Armor -= (int)Mathf.Ceil(damageReduction);
        }

        Health -= (int)damageResult;

        if (Health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnPlayerDeath?.Invoke();
    }
}
