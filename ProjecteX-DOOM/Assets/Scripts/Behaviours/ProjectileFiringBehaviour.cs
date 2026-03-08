using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFiringBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private int _shootProjectilesPerShot = 1;
    [SerializeField] private float _shootIntervalPerShot = 0.1f;
    [SerializeField] private float _shootNoiseStrength = 0.1f;
    [SerializeField] private float _shootInterval = 3f;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private int projectileDamageMulti = 3;
    [SerializeField] private int projectileMaxBaseDamage = 8;

    public Vector3 ShootPointPosition => _shootPoint.position;

    public Stack<GameObject> ProjectileStack = new Stack<GameObject>();

    public string targetLayerName;

    private Transform _projectileContainer;
    private float _lastShootTime;

    private void Awake()
    {
        GameObject container = new GameObject($"{gameObject.name}_Projectiles");
        _projectileContainer = container.transform;
    }

    public void Shoot(Vector3 direction)
    {
        if (Time.time - _lastShootTime >= _shootInterval)
        {
            if (_shootProjectilesPerShot > 1)
            {
                StartCoroutine(ShotCorrutine(direction));
            }
            else
            {
                if (ProjectileStack.Count == 0)
                {
                    SpawnProjectile(direction);
                }
                else
                {
                    ProjectileStackPop(direction);
                }
            }
            _lastShootTime = Time.time;
        }
    }

    private IEnumerator ShotCorrutine(Vector3 direction)
    {
        for (int i = 0; i < _shootProjectilesPerShot; i++)
        {
            direction = AddNoise(direction, _shootNoiseStrength);

            if (ProjectileStack.Count == 0)
            {
                SpawnProjectile(direction);
            }
            else
            {
                ProjectileStackPop(direction);
            }
            yield return new WaitForSeconds(_shootIntervalPerShot);
        }
    }

    private Vector3 AddNoise(Vector3 direction, float strength)
    {
        Vector3 noise = new Vector3(
            Random.Range(-strength, strength),
            Random.Range(-strength, strength),
            Random.Range(-strength, strength)
        );
        return (direction + noise).normalized;
    }

    private void SpawnProjectile(Vector3 direction)
    {
        GameObject projectile = Instantiate(_projectile, _shootPoint.position, Quaternion.LookRotation(direction), _projectileContainer);
        ProjectileBehaviour pb = projectile.GetComponent<ProjectileBehaviour>();

        pb.shooter = this;
        pb.speed = projectileSpeed;
        pb.damageMulti = projectileDamageMulti;
        pb.maxBaseDamage = projectileMaxBaseDamage;
        pb.direction = direction;
    }

    public void ProjectileStackPush(GameObject go)
    {
        ProjectileStack.Push(go);
        go.SetActive(false);
    }

    private void ProjectileStackPop(Vector3 direction)
    {
        GameObject go = ProjectileStack.Pop();

        go.transform.SetParent(_projectileContainer);
        go.transform.position = _shootPoint.position;
        go.transform.rotation = Quaternion.LookRotation(direction);
        go.GetComponent<ProjectileBehaviour>().direction = direction;
        go.SetActive(true);
    }
}
