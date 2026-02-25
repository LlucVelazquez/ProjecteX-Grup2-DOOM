using UnityEngine;

public class ShotgunController : MonoBehaviour
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

    public float Ammunition;

    private void Awake()
    {
        Ammunition = _initialAmmunition;
    }

    public void Shoot()
    {
        if (Ammunition <= 0) return;

        Ammunition--;

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
        Ammunition += ammo;
    }

    private int GetDamage()
    {
        return Random.Range(1, 4) * _damageMulti;
    }
}
