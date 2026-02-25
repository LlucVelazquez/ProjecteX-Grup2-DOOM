using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using Sys = System;

[RequireComponent(typeof(Animator))]
public class ShotgunController : MonoBehaviour, IUsable
{
    [Header("References")]
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Camera _camera;

    [Header("Shotgun Settings")]
    [SerializeField] private int _damageMulti = 5;
    [SerializeField] private float _range = 100f;
    [SerializeField] private int _pellets = 7;
    [SerializeField] private int _initialAmmunition = 8;

    [Header("Spread Settings")]
    [SerializeField] private float _spreadMean = 5.9f;
    [SerializeField] private float _spreadStandardDeviation = 1.5f;
    [SerializeField] private float _spreadMin = 2.2f;
    [SerializeField] private float _spreadMax = 9.8f;

    [Header("Animation Settings")]
    [SerializeField] private string _animPumpParName = "Pump";
    [SerializeField] private string _animPumpStateName = "Pump Handle";

    public static event Sys.Action<int> OnAmmunitionChange;

    private Animator _animator;

    private bool _isShooting = false;

    public int CurrentAmmunition
    {
        get => _currentAmmo;
        set
        {
            _currentAmmo = value;
            OnAmmunitionChange?.Invoke(value);
        }
    }

    private int _currentAmmo;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        _currentAmmo = _initialAmmunition;
    }

    private void Start()
    {
        OnAmmunitionChange?.Invoke(_currentAmmo);
    }

    public void Use()
    {
        Shoot();
    }

    public void Shoot()
    {
        if (CurrentAmmunition <= 0) return;
        if (_isShooting) return;

        _isShooting = true;

        _animator.SetTrigger(_animPumpParName);
        CurrentAmmunition--;

        for (int i = 0; i < _pellets; i++)
        {
            Vector3 spreadDirection = GetSpreadDirection(_camera.transform.forward);

            RaycastHit hit;
            if (Physics.Raycast(_shootPoint.position, spreadDirection, out hit, _range))
            {
                ITargeteable target = hit.transform.GetComponent<ITargeteable>();
                if (target != null)
                {
                    int damage = GetDamage();
                    target.TakeDamage(damage);
                    Debug.Log($"Hit to target {hit.collider.name} - Damage: {damage}");
                }
            }

            Debug.DrawRay(_shootPoint.transform.position, spreadDirection * _range, Color.red, 1f);
        }

        StartCoroutine(WaitForAnimation());
    }

    private IEnumerator WaitForAnimation()
    {
        yield return null;

        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        while (!stateInfo.IsName(_animPumpStateName))
        {
            yield return null;
            stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        }

        while (stateInfo.normalizedTime < 0.9f)
        {
            yield return null;
            stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        }

        _isShooting = false;
    }

    private Vector3 GetSpreadDirection(Vector3 forward)
    {
        float spreadAngle = GetNormalDistributedAngle();

        float randomRotation = Random.Range(0f, 360f);

        Quaternion spreadRotation = Quaternion.AngleAxis(randomRotation, forward) * Quaternion.AngleAxis(spreadAngle, Vector3.up);

        return spreadRotation * forward;
    }

    private float GetNormalDistributedAngle()
    {
        float u1 = Random.Range(0f, 1f);
        float u2 = Random.Range(0f, 1f);

        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);

        float angle = _spreadMean + _spreadStandardDeviation * randStdNormal;

        return Mathf.Clamp(angle, _spreadMin, _spreadMax);
    }

    public void AddAmmo(int ammo)
    {
        CurrentAmmunition += ammo;
    }

    private int GetDamage()
    {
        return Random.Range(1, 4) * _damageMulti;
    }
}
