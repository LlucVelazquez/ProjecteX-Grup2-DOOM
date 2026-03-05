using System.Collections.Generic;
using UnityEngine;

public class ProjectileFiringBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private float _shootInterval = 3f;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private int projectileDamageMulti = 3;
    [SerializeField] private int projectileMaxBaseDamage = 8;

    public Vector3 ShootPointPosition => _shootPoint.position;

    public static Stack<GameObject> ProjectileStack = new Stack<GameObject>();

    public string targetLayerName;

    private float _lastShootTime;

    public void Shoot(Vector3 direction)
    {
        if (Time.time - _lastShootTime >= _shootInterval)
        {
            if (ProjectileStack.Count == 0)
            {
                SpawnProjectile(direction);
            }
            else
            {
                ProjectileStackPop(direction);
            }

            _lastShootTime = Time.time;
        }
    }

    private void SpawnProjectile(Vector3 direction)
    {
        GameObject projectile = Instantiate(_projectile, _shootPoint.position, Quaternion.LookRotation(direction));
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

        go.transform.position = _shootPoint.position;
        go.transform.rotation = Quaternion.LookRotation(direction);
        go.GetComponent<ProjectileBehaviour>().direction = direction;
        go.SetActive(true);
    }
}
