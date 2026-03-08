using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, ITargeteable
{
    [SerializeField] private int _health = 100;
    [SerializeField] private int _armor = 0;
    [SerializeField] private int _megaarmor = 0;
    [SerializeField] private int _lowHealthValue = 20;

    public static event Action<int> OnHealthChange;
    public static event Action<int> OnArmorChange;
    public static event Action<int> OnMegaarmorChange;
    public static event Action<bool> OnLowHealth;
    public static event Action OnPlayerHurt;
    public static event Action OnPlayerDeath;

    public int Health
    {
        get => _health;
        set
        {
            _health = value;

            if (_health <= _lowHealthValue && !_isLowHealth)
            {
                _isLowHealth = true;
                OnLowHealth?.Invoke(_isLowHealth);
            }

            if (_health > _lowHealthValue && _isLowHealth)
            {
                _isLowHealth = false;
                OnLowHealth?.Invoke(_isLowHealth);
            }

            if (_health <= 0)
            {
                _health = 0;
            }

            OnHealthChange?.Invoke(value);
        }
    }
    public int Armor
    {
        get => _armor;
        set
        {
            _armor = value;
            if (value != 0 || _restarted) OnArmorChange?.Invoke(value);
        }
    }
    public int Megaarmor
    {
        get => _megaarmor;
        set
        {
            _megaarmor = value;
            if (value != 0 || _restarted) OnMegaarmorChange?.Invoke(value);
        }
    }

    private bool _isLowHealth = false;
    private int _initialHealth, _initialArmor, _initialMegaarmor;
    private bool _restarted = false;

    private void OnEnable() => UIManager.OnRestartGame += ResetHealth;
    private void OnDisable() => UIManager.OnRestartGame -= ResetHealth;

    private void Awake()
    {
        _initialHealth = _health;
        _initialArmor = _armor;
        _initialMegaarmor = _megaarmor;
    }

    private void Start()
    {
        OnHealthChange?.Invoke(_health);
        OnArmorChange?.Invoke(_armor);
    }

    private void ResetHealth(bool fullRestart)
    {
        _restarted = true;

        Health = _initialHealth;
        Armor = _initialArmor;
        Megaarmor = _initialMegaarmor;

        _restarted = false;
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
        OnPlayerHurt?.Invoke();

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
