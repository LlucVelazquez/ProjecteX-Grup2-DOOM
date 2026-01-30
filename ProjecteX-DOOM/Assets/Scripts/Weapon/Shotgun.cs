using UnityEngine;

[CreateAssetMenu(fileName = "Shotgun", menuName = "Weapons/Shotgun")]
public class Shotgun : RangedWeapon
{
    [Header("Shotgun Settings")]
    [SerializeField] private float _damageMulti = 5f;
    //[SerializeField] private float _range = 100f;
    [SerializeField] private int _pellets = 7;
    [SerializeField] private int _initialAmmunition = 8;

    [Header("Spread Settings")]
    [SerializeField] private float _spreadMean = 5.9f;
    [SerializeField] private float _spreadStandardDeviation = 1.5f;
    [SerializeField] private float _spreadMin = 2.2f;
    [SerializeField] private float _spreadMax = 9.8f;

    //public float Ammunition;

    /*private void Awake()
    {
        _ammunition = _initialAmmunition;
    }*/

    public override void Shoot(Transform shootPoint, Camera cam)
    {
        base.Shoot(shootPoint, cam);

        /*if (Ammunition <= 0) return;

        Ammunition--;*/

        for (int i = 0; i < _pellets; i++)
        {
            Vector3 spreadDirection = GetSpreadDirection(cam.transform.forward);

            RaycastHit hit;
            if (Physics.Raycast(shootPoint.position, spreadDirection, out hit, _range))
            {
                Target target = hit.transform.GetComponent<Target>();
                if (target != null)
                {
                    float damage = GetDamage();
                    target.TakeDamage(damage);
                    Debug.Log($"Hit to target {hit.collider.name} - Damage: {damage}");
                }
            }

            Debug.DrawRay(shootPoint.transform.position, spreadDirection * _range, Color.red, 1f);
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
        _ammunition += ammo;
    }

    private float GetDamage()
    {
        return Random.Range(1, 4) * _damageMulti;
    }
}
